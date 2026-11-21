// 阅读：托管线程池
// https://learn.microsoft.com/zh-cn/dotnet/standard/threading/the-managed-thread-pool
//
// 待补充（核心完成后选读）：
// 1.线程复用：线程池中的线程执行完一个工作项后继续服务下一个。
// 2.池内线程默认是后台线程，主线程退出后不会阻止进程结束。
// 3.先不调整最小/最大线程数等线程池参数。
internal static partial class Program
{
    static void GetSetMaxThreads()
    {
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
        Console.WriteLine($"Max worker threads: {maxWorkerThreads}, Max completion port threads: {maxCompletionPortThreads}");
        // maxWorkerThreads是工作线程上限，64位.Net上通常默认32767
        // 工作线程用于执行通用的托管代码任务（主要是 CPU 计算、普通异步任务和大部分回调）
        // 不是程序一启动就分配这么多线程，而是根据需要动态分配，但不会超过这个上限
        // maxCompletionPortThreads是I/O 完成端口线程 / IOCP 线程上限，通常默认1000
        // I/O 完成端口线程用于处理异步 I/O 操作（如文件读写、网络通信等）
        ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
        Console.WriteLine($"Min worker threads: {minWorkerThreads}, Min completion port threads: {minCompletionPortThreads}");
        // 绝大多数情况下不应该调整这个设置
        // ThreadPool.SetMaxThreads(1000, 1000);
        // ThreadPool.SetMinThreads(100, 100);
    }
}
