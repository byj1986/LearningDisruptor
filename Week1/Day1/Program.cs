internal static partial class Program
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
        // 计时统一放在调用方：切换下面任一版本时计时口径不变；finally 保证 V4 抛异常时也能停表并打出耗时。
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            // MakeBreakfast();
            // await MakeBreakfastAsyncV1();
            // await MakeBreakfastAsyncV2();
            // await MakeBreakfastAsyncV3();
            // await MakeBreakfastAsyncV4();
            // await MakeBreakfastAsyncV5();
            // await MakeBreakfastAsyncV6();
            // await MakeBreakfastAsyncV7();
        }
        finally
        {
            sw.Stop();
            // Log("早餐", $"耗时 {sw.ElapsedMilliseconds}ms");
        }
        #endregion

        #region Part3 基于任务的异步编程
        // 阅读：基于任务的异步编程
        // https://learn.microsoft.com/zh-cn/dotnet/standard/parallel-programming/task-based-asynchronous-programming
        //TaskForEach();
        // TasksForWords();
        // CreateManualRunTask();
        // CreateTaskWithTaskRun();
        // CreateTaskWithTaskFactory();
        // CreateGenericTaskWithTaskFactory();
        // UnderstadingTaskOptions();
        // UnderstadingTaskCulture();
        // CreateContinuationTask();
        // CreateContinuationTaskV2();
        // CreateParentChildTasks();
        // CreateParentChildTasksV2();
        // UnderstandingWaitAll();
        #endregion

        #region Part4 托管线程池
        // 阅读：托管线程池
        // https://learn.microsoft.com/zh-cn/dotnet/standard/threading/the-managed-thread-pool
        // GetSetMaxThreads();
        #endregion

        #region Part5 异步编程模型
        // 阅读：异步编程场景
        // https://learn.microsoft.com/zh-cn/dotnet/csharp/asynchronous-programming/async-scenarios
        await GetUsersAsync([1, 2, 3]);
        #endregion
    }
}
