internal static class Program
{
    private static async Task Main()
    {
        // 请按照当天 README.md 手写练习代码。
        #region Part1 使用线程和线程处理
        // 创建和启动新线程；Join等待线程结束；Sleep暂停当前线程。暂不读旧版Thread.Abort内容。
        #region new/sleep
        // ThreadCreateAndLaunch();
        #endregion

        #region Join
        // ThreadJoin();
        #endregion

        #region SleepForever
        // SleepForever();
        #endregion

        #region ExecuteInForeground
        // ExecuteInForeground();
        #endregion
        #endregion

        #region Part2 使用 async 和 await 进行异步编程
        // 重点找Task.WhenAll、异步异常与await的示例；无需抄完整早餐程序。
        // MakeBreakfast();
        // await MakeBreakfastAsyncV1();

        await MakeBreakfastAsyncV2();
        #endregion

        #region Part3 基于任务的异步编程
        #endregion

        #region Part4 托管线程池
        #endregion

        #region Part5 异步编程模型
        #endregion
    }

    #region Part1 使用线程和线程处理

    private static Thread thread1, thread2;
    /// <summary>
    /// 创建和启动新线程 / Sleep 暂停当前线程
    /// </summary>
    /// <remarks>
    /// <see cref="Thread.Start"/> 后，工作线程与主线程并发执行；
    /// <see cref="Thread.Sleep(int)"/> 只暂停调用它的线程，不会暂停其他线程。
    /// 两个线程的输出交替出现，但具体顺序由操作系统调度决定，并不固定。
    /// </remarks>
    private static void ThreadCreateAndLaunch()
    {
        // 本地函数：作为新线程的入口点（ThreadStart 委托）。
        static void DoWork()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Working thread... {i}");
                // 暂停工作线程 100 ms，给主线程获得执行的机会。
                Thread.Sleep(100);
            }
        }

        // 仅创建 Thread 对象，此时线程尚未运行（ThreadState 为 Unstarted）。
        Thread thread1 = new Thread(DoWork);
        // 启动线程：运行时会在新线程上调用 DoWork，而不是在主线程上直接调用。
        thread1.Start();

        // 主线程不等待工作线程，继续并发执行自己的循环。
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"In main... {i}");
            Thread.Sleep(100);
        }
        // 注意：此处没有调用 thread1.Join()，主线程可能先于工作线程结束循环。
    }

    /// <summary>
    /// 使用 <see cref="Thread.Join()"/> 等待另一个线程执行完毕
    /// </summary>
    /// <remarks>
    /// Join 会阻塞调用它的线程，直到目标线程终止后才继续往下执行。
    /// 本例中若 Thread1 检查时 Thread2 已经启动，Thread1 会停在 Join 处，
    /// 等 Thread2 全部执行完再恢复；但 Thread2 是否已启动取决于线程调度，
    /// 存在竞态，因此实际输出顺序并非绝对保证。
    /// </remarks>
    private static void ThreadJoin()
    {
        // 两个线程共用同一个入口点，靠线程名区分各自的执行路径。
        static void ThreadProc()
        {
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);
            // 只有 Thread1 才等待 Thread2；且目标线程未启动（Unstarted）时不能 Join，需先判断状态。
            if (Thread.CurrentThread.Name == "Thread1" &&
                thread2.ThreadState != ThreadState.Unstarted)
                // 阻塞 Thread1，直到 Thread2 终止；Thread2 本身不受影响，继续运行。
                thread2.Join();

            Console.WriteLine($"Continuing with the {Thread.CurrentThread.Name}");
            // 暂停当前线程 4 秒，期间可观察另一个线程的状态变化。
            Thread.Sleep(4000);
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);
            // ThreadState：被 Join/Sleep 阻塞的线程为 WaitSleepJoin，正在执行的为 Running，已终止的为 Stopped。
            Console.WriteLine("Thread1: {0}", thread1.ThreadState);
            Console.WriteLine("Thread2: {0}\n", thread2.ThreadState);
        }

        // 创建并启动 Thread1：Start 后主线程立即继续，不会等它执行完。
        thread1 = new Thread(ThreadProc) { Name = "Thread1" };
        thread1.Start();

        // 创建并启动 Thread2：它与 Thread1 并发运行同一个 ThreadProc。
        thread2 = new Thread(ThreadProc) { Name = "Thread2" };
        thread2.Start();

        // 预期执行顺序（Thread1 成功 Join 到 Thread2 时）：
        // thread1 里 ThreadProc 剩下的部分，需要等到 thread2 执行完成后才会执行
        // 先打印Current thread: Thread1
        // 再打印Current thread: Thread2
        // 打印的应该是Continuing with the Thread2
        // 4秒后，打印Current thread: Thread2
        // 打印Thread1: WaitSleepJoin(Block或者之类的状态)
        // 打印Thread2: Running
        // thread2的ThreadProc完成
        // 继续打印的应该是Continuing with the Thread1
        // 4秒后，打印Current thread: Thread1
        // 打印Thread1: Running
        // 打印Thread2: Stopped(完成我估计)
    }

    /// <summary>
    /// 无限睡眠的线程，以及用 Interrupt 唤醒、Join 等待其结束
    /// </summary>
    /// <remarks>
    /// <see cref="Thread.Sleep(int)"/> 传入 <see cref="Timeout.Infinite"/> 会让线程无限期阻塞，
    /// 只能被其他线程调用 <see cref="Thread.Interrupt"/> 打断：
    /// 阻塞中的线程会抛出 <see cref="ThreadInterruptedException"/>，随后 finally 仍会执行。
    /// </remarks>
    private static void SleepForever()
    {
        // 新线程的入口点：打印提示后进入无限睡眠。
        static void SleepIndefinitely()
        {
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' about to sleep indefinitely.");
            try
            {
                // 无限期阻塞当前线程，不占用 CPU，直到被 Interrupt 打断。
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                // 被 Interrupt 唤醒时进入这里，线程并不会被强制终止，而是恢复执行后续代码。
                Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' awoken.");
            }
            finally
            {
                // 无论是正常返回还是被打断，finally 都会执行，适合放清理逻辑。
                Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' executing finally block.");
            }
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' finishing normal execution.");
            Console.WriteLine();
        }

        // 创建并启动睡眠线程（命名便于在输出中识别）。
        var sleepingThread = new Thread(SleepIndefinitely) { Name = "Sleeping" };
        sleepingThread.Start();

        // 主线程先睡 2 秒，确保睡眠线程已经进入无限睡眠状态。
        Thread.Sleep(2000);
        // 打断睡眠线程的阻塞状态，使其抛出 ThreadInterruptedException 并继续执行。
        sleepingThread.Interrupt();
        // 等待睡眠线程真正执行完毕再返回，避免主线程提前退出。
        sleepingThread.Join();
    }

    private static void ExecuteInForeground()
    {
        static void ExecuteInForeground()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: {Thread.CurrentThread.ThreadState}, Priority {Thread.CurrentThread.Priority}");
            do
            {
                Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: Elapsed {sw.ElapsedMilliseconds / 1000.0:N2} seconds");
                Thread.Sleep(500);
            } while (sw.ElapsedMilliseconds <= 5000);
            sw.Stop();
        }
        var th = new Thread(ExecuteInForeground) { IsBackground = true }; // 默认为false，因此主线程需要等待th退出才能退出
        th.Start();
        Thread.Sleep(1000);
        Console.WriteLine($"Main thread ({Environment.CurrentManagedThreadId}) exiting...");
    }
    #endregion

    #region Part2 使用 async 和 await 进行异步编程
    /// <summary>
    /// 薯饼类
    /// </summary>
    internal class HashBrown { }
    /// <summary>
    /// 咖啡类
    /// </summary>
    internal class Coffee { }
    /// <summary>
    /// 鸡蛋类
    /// </summary>
    internal class Egg { }
    /// <summary>
    /// 橙汁类
    /// </summary>
    internal class Juice { }
    /// <summary>
    /// 面包类
    /// </summary>
    internal class Toast { }

    /// <summary>
    /// 1:倒一杯咖啡。
    /// 2:加热锅，然后炒两个鸡蛋。
    /// 3:烹制三个薯饼。
    /// 4:烤两块面包。
    /// 5:将黄油和果酱涂抹在烤面包片上。
    /// 6:倒一杯橙汁。
    /// 顺序执行大约需要15秒(FryEggs 6000 + FryHashBrowns 6000 + ToastBread 3000)
    /// </summary>
    private static void MakeBreakfast()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        Coffee cup = PourCoffee();
        Console.WriteLine("coffee is ready");

        Egg eggs = FryEggs(2);
        Console.WriteLine("eggs are ready");

        HashBrown hashBrown = FryHashBrowns(3);
        Console.WriteLine("hash browns are ready");

        Toast toast = ToastBread(2);
        ApplyButter(toast);
        ApplyJam(toast);
        Console.WriteLine("toast is ready");

        Juice oj = PourOrangeJuice();
        Console.WriteLine("oj is ready");
        Console.WriteLine("Breakfast is ready!");
        Console.WriteLine($"Sequential execution time: {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }

    /// <summary>
    /// 1:倒一杯咖啡。
    /// 2:加热锅，然后炒两个鸡蛋。
    /// 3:烹制三个薯饼。
    /// 4:烤两块面包。
    /// 5:将黄油和果酱涂抹在烤面包片上。
    /// 6:倒一杯橙汁。
    /// 虽然使用了async，但是所有的任务都需要await再继续
    /// 因此不会并发执行，而是顺序执行
    /// 最终仍然需要大约15秒
    /// </summary>
    private async static Task MakeBreakfastAsyncV1()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Coffee cup = PourCoffee();
        Console.WriteLine("coffee is ready");

        Egg eggs = await FryEggsAsync(2);
        Console.WriteLine("eggs are ready");

        HashBrown hashBrown = await FryHashBrownsAsync(3);
        Console.WriteLine("hash browns are ready");

        Toast toast = await ToastBreadAsync(2);
        ApplyButter(toast);
        ApplyJam(toast);
        Console.WriteLine("toast is ready");

        Juice oj = PourOrangeJuice();
        Console.WriteLine("oj is ready");
        Console.WriteLine("Breakfast is ready!");


        Console.WriteLine($"Async execution time: {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }

    /// <summary>
    /// 1:倒一杯咖啡。
    /// 2:加热锅，然后炒两个鸡蛋。
    /// 3:烹制三个薯饼。
    /// 4:烤两块面包。
    /// 5:将黄油和果酱涂抹在烤面包片上。
    /// 6:倒一杯橙汁。
    /// 大约需要6秒Max(FryEggs 3000, FryHashBrowns 3000, ToastBread 3000)
    /// </summary>
    /// <returns></returns>
    private async static Task MakeBreakfastAsyncV2()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Coffee cup = PourCoffee();
        Console.WriteLine("coffee is ready");
        // Task<Egg> eggsTask = FryEggsAsync(2);
        // Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
        // Task<Toast> toastTask = ToastBreadAsync(2);


        var eggsTask = FryEggsAsync(2);
        var hashBrownTask = FryHashBrownsAsync(3);
        var toastTask = ToastBreadAsync(2);

        var eggs = await eggsTask;
        Console.WriteLine("eggs are ready");

        var hashBrown = await hashBrownTask;
        Console.WriteLine("hash browns are ready");

        var toast = await toastTask;
        Console.WriteLine("toast is ready");
        // await Task.WhenAll(eggsTask, hashBrownTask, toastTask);

        Juice oj = PourOrangeJuice();
        Console.WriteLine("oj is ready");
        Console.WriteLine("Breakfast is ready!");
        Console.WriteLine($"Async execution time: {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }
    /// <summary>
    /// 倒一杯橙汁。
    /// 瞬时完成
    /// </summary>
    /// <returns></returns>
    private static Juice PourOrangeJuice()
    {
        Console.WriteLine("Pouring orange juice");
        return new Juice();
    }

    /// <summary>
    /// 将果酱涂抹在烤面包片上。
    /// 瞬时完成
    /// </summary>
    /// <param name="toast"></param>
    private static void ApplyJam(Toast toast) =>
        Console.WriteLine("Putting jam on the toast");

    /// <summary>
    /// 将黄油涂抹在烤面包片上。
    /// 瞬时完成
    /// </summary>
    /// <param name="toast"></param>
    private static void ApplyButter(Toast toast) =>
        Console.WriteLine("Putting butter on the toast");

    private static async Task<Toast> ToastBreadAsync(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        Console.WriteLine("Start toasting...");
        await Task.Delay(3000);
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }

    /// <summary>
    /// 烤两块面包。
    /// 面包片放入烤面包机，瞬时完成
    /// 烤面包片需要3000ms
    /// 面包片从烤面包机中取出，瞬时完成
    /// 总共需要3000ms
    /// </summary>
    /// <param name="slices">面包片数量</param>
    /// <returns></returns>
    private static Toast ToastBread(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine($"Putting a slice of bread in the toaster {slice + 1} of {slices}");
        }
        Console.WriteLine("Start toasting...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }

    private static async Task<HashBrown> FryHashBrownsAsync(int patties)
    {
        Console.WriteLine($"putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        await Task.Delay(3000);
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine("flipping a hash brown patty");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        await Task.Delay(3000);
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }

    /// <summary>
    /// 烹制三个薯饼。
    /// 放入薯饼，瞬时完成
    /// 煎薯饼第一面需要3000ms
    /// 煎薯饼第二面需要3000ms
    /// 放入餐盘，瞬时完成
    /// 总共需要6000ms
    /// </summary>
    /// <param name="patties">薯饼数量</param>
    /// <returns></returns>
    private static HashBrown FryHashBrowns(int patties)
    {
        Console.WriteLine($"putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        Task.Delay(3000).Wait();
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine($"flipping a hash brown patty {patty + 1} of {patties}");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }

    private static async Task<Egg> FryEggsAsync(int count)
    {
        Console.WriteLine("Warming the egg pan...");
        await Task.Delay(3000);
        Console.WriteLine($"cracking {count} eggs");
        Console.WriteLine("cooking the eggs ...");
        await Task.Delay(3000);
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    /// <summary>
    /// 加热锅，然后炒两个鸡蛋。
    /// 热锅需要3000ms
    /// 打蛋瞬时完成
    /// 炒鸡蛋需要3000ms
    /// 放入餐盘，瞬时完成
    /// 总共需要6000ms
    /// </summary>
    /// <param name="count">鸡蛋数量</param>
    /// <returns></returns>
    private static Egg FryEggs(int count)
    {
        Console.WriteLine("Warming the egg pan...");
        Task.Delay(3000).Wait();
        Console.WriteLine($"cracking {count} eggs");
        Console.WriteLine("cooking the eggs ...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    /// <summary>
    /// 倒一杯咖啡。
    /// 瞬间完成
    /// </summary>
    /// <returns></returns>
    private static Coffee PourCoffee()
    {
        Console.WriteLine("Pouring coffee");
        return new Coffee();
    }
    #endregion

    #region Part3 基于任务的异步编程
    #endregion

    #region Part4 托管线程池
    #endregion

    #region Part5 异步编程模型
    #endregion
}
