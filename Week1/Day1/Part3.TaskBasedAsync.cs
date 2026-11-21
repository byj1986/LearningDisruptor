// 阅读：基于任务的异步编程
// https://learn.microsoft.com/zh-cn/dotnet/standard/parallel-programming/task-based-asynchronous-programming
//
// 待补充（今天未手写）：
// 1.任务抽象：Task 表示操作及其完成状态，不承诺独占线程。
// 2.显式创建并运行任务：定位 Task.Run，与本文件外 Part2 的 async 方法做对照。
// 3.Run / StartNew 的取舍，以及任务结果与异常如何被观察。
using System.Diagnostics;
using System.Globalization;
using System.Text;

internal static partial class Program
{
    class ThreadData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
    }

    const string OblomovFragment = @"
        In the middle of a large and lofty room, in which were several windows,
        a great many pictures, and a quantity of furniture, there stood a bed
        with a silk quilted coverlet, and a down pillow on it, on which a man
        was lying. He was between thirty-two and thirty-five in appearance, of
        medium height, with pleasing, dark hair, but somewhat too flabby and
        soft. His face was not devoid of gentleness, and he was agreeable enough
        in his way, but his excessive laxity was reflected in every line of his
        features. There was nothing either firm or decided in them; and in his
        eyes and smiles, in his movements and gestures, there was a childlike
        softness.
        ";
    static readonly char[] Punctuation = [' ', '\u000A', ',', '.', ';', ':', '-', '_', '/'];

    static void TaskForEach()
    {
        int[] input = [4, 1, 6, 2, 9, 5, 10, 3];
        int sum = 10;
        var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        try
        {
            Parallel.ForEach(
                    input,
                    options,
                    () => 0,
                    (n, loopState, localSum) =>
                    {
                        Console.WriteLine($"localSum: {localSum}");
                        localSum += n;
                        Console.WriteLine("body: Thread={0}, n={1}, localSum={2}", Environment.CurrentManagedThreadId, n, localSum);
                        return localSum;
                    },
                    (localSum) =>
                    {
                        Interlocked.Add(ref sum, localSum);
                        // Console.WriteLine("localFinally: Thread={0}, localSum={1}, sum={2}", Environment.CurrentManagedThreadId, localSum, sum);
                    }
                );

            Console.WriteLine($"\nSum={sum}");
        }
        // No exception is expected in this example, but if one is still thrown from a task,
        // it will be wrapped in AggregateException and propagated to the main thread.
        catch (AggregateException e)
        {
            Console.WriteLine($"Parallel.ForEach has thrown an exception. THIS WAS NOT EXPECTED.\n{e}");
        }
    }

    static void TasksForWords()
    {
        // Retrieve Goncharov's "Oblomov" from Gutenberg.org.
        // no url for book, use sample text instead
        string[] words = OblomovFragment.Split(Punctuation, StringSplitOptions.RemoveEmptyEntries);
        #region ParallelTasks
        // Perform three tasks in parallel on the source array
        Parallel.Invoke(
            () =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },  // close first Action

            () =>
            {
                Console.WriteLine("Begin second task...");
                GetMostCommonWords(words);
            }, //close second Action

            () =>
            {
                Console.WriteLine("Begin third task...");
                GetCountForWord(words, "and");
            } //close third Action
        ); //close parallel.invoke

        Console.WriteLine("Returned from Parallel.Invoke");
        #endregion
    }


    #region HelperMethods
    private static void GetCountForWord(string[] words, string term)
    {
        var findWord = from word in words
                       where word.ToUpper().Contains(term.ToUpper())
                       select word;

        Console.WriteLine($@"Task 3 -- The word ""{term}"" occurs {findWord.Count()} times.");
    }

    private static void GetMostCommonWords(string[] words)
    {
        var frequencyOrder = from word in words
                             where word.Length > 6
                             group word by word into g
                             orderby g.Count() descending
                             select g.Key;

        var commonWords = frequencyOrder.Take(10);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Task 2 -- The most common words are:");
        foreach (var v in commonWords)
        {
            sb.AppendLine("  " + v);
        }
        Console.WriteLine(sb.ToString());
    }

    private static string GetLongestWord(string[] words)
    {
        var longestWord = (from w in words
                           orderby w.Length descending
                           select w).First();

        Console.WriteLine($"Task 1 -- The longest word is {longestWord}.");
        return longestWord;
    }
    #endregion

    static void CreateManualRunTask()
    {
        Thread.CurrentThread.Name = "Main";

        // Create a task and supply a user delegate by using a lambda expression.
        Task taskA = new Task(() => Console.WriteLine("Hello from taskA."));
        // Start the task.
        taskA.Start();

        // Output a message from the calling thread.
        Console.WriteLine($"Hello from thread '{Thread.CurrentThread.Name}'.");
        taskA.Wait();
    }

    static void CreateTaskWithTaskRun()
    {
        Thread.CurrentThread.Name = "Main";

        // Define and run the task.
        Task taskA = Task.Run(() => Console.WriteLine("Hello from taskA."));

        // Output a message from the calling thread.
        Console.WriteLine($"Hello from thread '{Thread.CurrentThread.Name}'.");
        taskA.Wait();
    }

    static void CreateTaskWithTaskFactory()
    {
        Task[] tasks = new Task[10];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Factory.StartNew(obj =>
            {
                var data = obj as ThreadData;
                if (data == null)
                {
                    return;
                }
                data.ThreadNum = Environment.CurrentManagedThreadId;
            }, new ThreadData { CreationTime = DateTime.Now.Ticks, Name = i });
        }
        Task.WaitAll(tasks);
        foreach (var task in tasks)
        {
            if (task.AsyncState is not ThreadData data)
                continue;
            Console.WriteLine($"Task {data.Name} created at {data.CreationTime} on thread {data.ThreadNum}");
        }
    }

    static void CreateGenericTaskWithTaskFactory()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Task<Double>[] taskArray = [Task<Double>.Factory.StartNew(() => DoComputation(1.0)),
         Task<Double>.Factory.StartNew(() => DoComputation(100.0)),
         Task<Double>.Factory.StartNew(() => DoComputation(1000.0)) ];
        var results = new Double[taskArray.Length];
        Double sum = 0;
        for (int i = 0; i < taskArray.Length; i++)
        {
            results[i] = taskArray[i].Result; // Result相当于Wait()+GetResult()，会同步等待task结束
            // Console.WriteLine($"Task {i} completed in {sw.ElapsedMilliseconds}ms");
            Console.Write("{0:N1} {1}", results[i], i == taskArray.Length - 1 ? "= " : "+ ");
            sum += results[i];
        }
        Console.WriteLine($"{sum:N1}");
    }

    /// <summary>
    /// 计算从start到start+10的和，步长为0.1
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    static Double DoComputation(Double start)
    {
        // 对于1.0和100.0，+=0.1的累计误差略小于理论上的 start + 10，因此循环次数是101次
        // Adding 10.999999999999979, Sum=605.9999999999994, Count=101
        // Adding 109.99999999999943, Sum=10604.999999999975, Count=101
        // Adding 1009.9000000000023, Sum=100495.00000000017, Count=100
        // 第101次时，1010.0000000000023，大于了start+10，因此不计算
        // 对于1000.0，+=0.1的累计误差略大于理论上的 start + 10，因此循环次数是100次
        // 所以最终输出为606.0 + 10605.0 + 100495.0 = 111,706.0
        // 项数 = 10 ÷ 0.1 + 1 = 100 + 1 = 101 项
        // ((1.0 + 11.0) / 2) * 101 =606.0
        // ((100.0 + 110.0) / 2) * 101 = 10605.0
        // ((1000.0 + 1009.9) / 2) * 100 = 100495.0
        Double sum = 0;
        // int count = 0;
        for (var value = start; value <= start + 10; value += .1)
        {
            sum += value;
            // count++;
            // Console.WriteLine($"Adding {value}, Sum={sum}, Count={count}");
        }

        return sum;
    }

    static void UnderstadingTaskOptions()
    {
        var task3 = new Task(() => DoComputation(1000.0),
                            TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness);
        task3.Start();
        Console.WriteLine($"TaskCreationOptions: {task3.CreationOptions}");
    }

    static void UnderstadingTaskCulture()
    {
        decimal[] values = [163025412.32m, 18905365.59m];
        string formatString = "C2";
        Func<String> formatDelegate = () =>
        {
            string output = String.Format("Formatting using the {0} culture on thread {1}.\n",
                                                                            CultureInfo.CurrentCulture.Name,
                                                                            Thread.CurrentThread.ManagedThreadId);
            foreach (var value in values)
                output += String.Format("{0}   ", value.ToString(formatString));

            output += Environment.NewLine;
            return output;
        };

        Console.WriteLine($"The example is running on thread {Thread.CurrentThread.ManagedThreadId}");
        // Make the current culture different from the system culture.
        Console.WriteLine($"The current culture is {CultureInfo.CurrentCulture.Name}");
        if (CultureInfo.CurrentCulture.Name == "fr-FR")
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

        Console.WriteLine($"Changed the current culture to {CultureInfo.CurrentCulture.Name}.\n");

        // Execute the delegate synchronously.
        Console.WriteLine("Executing the delegate synchronously:");
        Console.WriteLine(formatDelegate());

        // Call an async delegate to format the values using one format string.
        Console.WriteLine("Executing a task asynchronously:");
        var t1 = Task.Run(formatDelegate);
        Console.WriteLine(t1.Result);

        Console.WriteLine("Executing a task synchronously:");
        var t2 = new Task<String>(formatDelegate);
        t2.RunSynchronously();
        Console.WriteLine(t2.Result);
    }

    static void CreateContinuationTask()
    {
        var getData = Task.Factory.StartNew(() =>
        {
            Random rnd = new Random();
            int[] values = new int[100];
            for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
                values[ctr] = rnd.Next();
            Console.WriteLine($"getData: {string.Join(", ", values)}");
            return values;
        });
        var processData = getData.ContinueWith((x) =>
        {
            int n = x.Result.Length;
            long sum = 0;
            double mean;

            for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
                sum += x.Result[ctr];

            mean = sum / (double)n;
            Console.WriteLine($"processData: n={n}, sum={sum}, mean={mean}");
            return Tuple.Create(n, sum, mean);
        });
        var displayData = processData.ContinueWith((x) =>
        {
            return String.Format("N={0:N0}, Total = {1:N0}, Mean = {2:N2}",
                                 x.Result.Item1, x.Result.Item2,
                                 x.Result.Item3);
        });
        Console.WriteLine(displayData.Result);
    }

    static void CreateContinuationTaskV2()
    {
        var displayData = Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                int[] values = new int[100];
                for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
                    values[ctr] = rnd.Next();

                return values;
            }).ContinueWith(x =>
            {
                int n = x.Result.Length;
                long sum = 0;
                double mean;

                for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
                    sum += x.Result[ctr];

                mean = sum / (double)n;
                return Tuple.Create(n, sum, mean);
            }).
            ContinueWith(x =>
            {
                return String.Format("N={0:N0}, Total = {1:N0}, Mean = {2:N2}",
                                        x.Result.Item1, x.Result.Item2,
                                        x.Result.Item3);
            });
        Console.WriteLine(displayData.Result);
    }

    static void CreateParentChildTasks()
    {
        var outer = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Outer task beginning.");

            var child = Task.Factory.StartNew(() =>
            {
                //Thread.SpinWait(5000000);
                Thread.Sleep(100);
                Console.WriteLine("Detached task completed.");
            });
        });

        outer.Wait();
        Console.WriteLine("Outer task completed.");
    }

    static void CreateParentChildTasksV2()
    {
        var parent = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Parent task beginning.");
            for (int ctr = 0; ctr < 10; ctr++)
            {
                int taskNo = ctr;
                Task.Factory.StartNew((x) =>
                {
                    //Thread.SpinWait(5000000);
                    Thread.Sleep(100);
                    Console.WriteLine($"Attached child #{x} completed.");
                }, taskNo, TaskCreationOptions.AttachedToParent);
            }
        }, TaskCreationOptions.DenyChildAttach);
        // DenyChildAttach: 父任务直接返回
        parent.Wait();
        Console.WriteLine("Parent task completed.");
    }

    static void UnderstandingWaitAll()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Task[] tasks = new Task[3];
        tasks[0] = Task.Run(() => { Thread.Sleep(2000); Console.WriteLine("1"); });
        tasks[1] = Task.Run(() => { Thread.Sleep(1000); Console.WriteLine("2"); });
        tasks[2] = Task.Run(() => { Thread.Sleep(3000); Console.WriteLine("3"); });
        Task.WaitAll(tasks);
        Console.WriteLine($"All tasks completed. Time: {sw.ElapsedMilliseconds}ms");
    }
}
