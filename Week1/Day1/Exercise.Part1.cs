internal static partial class Program
{
    private static void CoreConcepts()
    {
        // 并发、并行、异步的区别。
        // 并发：同一时间段内处理多个任务
        // 并行：同一时刻处理多个任务
        // 异步：发起操作后不阻塞当前线程，等操作完成后再继续后续逻辑

        // Thread、线程池、Task、await 的关系；Join 与等待任务。
        // Thread：托管线程对象，可以手动新建，启动、停止、等待
        // 线程池：线程池是线程的容器，线程池中的线程是托管线程
        // Task：任务是线程的封装，任务是异步操作的封装
        // await：等待任务完成，非阻塞
        // Join：等待线程完成，阻塞
    }

    /// <summary>
    /// 计算start到end之和
    /// </summary>
    /// <param name="start">开始值</param>
    /// <param name="end">结束值</param>
    /// <returns>和</returns>
    private static long AccumulateCore(int start, int end)
    {
        long sum = 0;
        for (int i = start; i <= end; i++)
        {
            sum += i;
        }
        return sum;
    }

    /// <summary>
    /// 让主流程同步计算1到100000之和，打印开始、结束、线程编号、结果，保留为参考。
    /// </summary>
    private static long AccumulateSync(string name = "主流程")
    {
        System.Console.WriteLine($"开始计算1到100000之和");
        System.Console.WriteLine($"线程编号：{Thread.CurrentThread.ManagedThreadId}，名称：{name}");
        long sum = AccumulateCore(0, 100000);
        System.Console.WriteLine($"结束计算1到100000之和，结果：{sum}");
        return sum;
    }

    private static void AccumulateThread()
    {
        System.Console.WriteLine($"开始计算1到100000之和");
        System.Console.WriteLine($"线程编号：{Thread.CurrentThread.ManagedThreadId}");
        // long sum = AccumulateCore(0, 100000);
        Thread thread = new Thread(() =>
        {
            long sum = AccumulateCore(0, 100000);
            System.Console.WriteLine($"结束计算1到100000之和，结果：{sum}");
        });
        thread.Start();
        thread.Join();
    }

    private static async Task AccumulateTask()
    {
        System.Console.WriteLine($"开始计算1到100000之和");
        System.Console.WriteLine($"线程编号：{Thread.CurrentThread.ManagedThreadId}");
        await Task.Run(() =>
        {
            long sum = AccumulateCore(0, 100000);
            System.Console.WriteLine($"结束计算1到100000之和，结果：{sum}");
        });
    }


    /// <summary>
    /// 10~30分钟
    /// 谁负责执行工作，谁代表工作完成？写出同步计算、显式Thread、Task.Run三种方式各自的启动与等待办法。
    /// </summary>
    private static async Task AccumulateCompare()
    {
        AccumulateSync();

        AccumulateThread();

        await AccumulateTask();
    }

    /// <summary>
    /// 30~50分钟
    /// 把同一计算交给一个Thread，主流程在启动后打印一条标记，再Join。重复3次；
    /// 保证工作结果一致，观察两条流程标记的先后。
    /// </summary>
    /// <returns></returns>
    private static void AccumulateCompareV2()
    {
        for (int round = 1; round <= 3; round++)
        {
            long sum = 0;
            Console.WriteLine($"[第{round}次][主流程 {Environment.CurrentManagedThreadId}] 准备启动工作线程");

            Thread worker = new Thread(() =>
            {
                Console.WriteLine($"[第{round}次][工作线程 {Environment.CurrentManagedThreadId}] 开始计算");
                sum = AccumulateCore(1, 100000);
                Console.WriteLine($"[第{round}次][工作线程 {Environment.CurrentManagedThreadId}] 计算完成，结果={sum}");
            });

            worker.Start();
            // 关键观察点：启动后、Join 前，主流程立刻打印一条标记
            Console.WriteLine($"[第{round}次][主流程 {Environment.CurrentManagedThreadId}] 已启动工作线程，准备 Join");

            worker.Join();
            Console.WriteLine($"[第{round}次][主流程 {Environment.CurrentManagedThreadId}] Join 完成，结果={sum}\n");
        }
    }

    /// <summary>
    /// 50～75分钟
    /// 把同一计算改为Task.Run；
    /// 先等待一个任务，再同时启动两个独立任务并等待全部完成。结果分别保存，避免此时引入共享累加器。
    /// </summary>
    /// <returns></returns>
    private static async Task AccumulateCompareV3()
    {
        Task<long> task1 = Task.Run(() => AccumulateSync("任务1"));
        long sum1 = await task1;
        Console.WriteLine($"任务1完成，结果={sum1}");
        Task<long> task2 = Task.Run(() => AccumulateSync("任务2"));
        Task<long> task3 = Task.Run(() => AccumulateSync("任务3"));

        long[] results = await Task.WhenAll(task2, task3);
        Console.WriteLine($"任务2和任务3完成，结果={results[0]}和{results[1]}");
    }
}