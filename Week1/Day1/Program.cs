internal static class Program
{
    private static Thread thread1, thread2;
    private static void Main()
    {
        // 请按照当天 README.md 手写练习代码。
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
        ExecuteInForeground();
        #endregion
    }

    #region 使用线程和线程处理

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

    #endregion

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
}
