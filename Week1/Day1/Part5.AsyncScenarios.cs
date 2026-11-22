// 阅读：异步编程场景
// https://learn.microsoft.com/zh-cn/dotnet/csharp/asynchronous-programming/async-scenarios
//
// 待补充（核心完成后选读）：
// 1.探索异步编程模型，查看基础概念。
// 2.对照 I/O 型工作（如 Task.Delay、文件/网络）与 CPU 型工作（如 Task.Run）适合的等待方式。
// 3.只记录谁启动、谁等待、异常由谁观察，不做性能排名。
// 可以对 I/O 绑定和 CPU 绑定代码使用异步代码，但实现不同。
//      I/O 绑定	使用async修饰符和await表达式，而不使用Task.Run方法。避免使用任务并行库。
//      CPU 绑定	使用async修饰符和await表达式，并使用Task.Run方法在另一个线程上生成工作。 
//                  此方法解决了 CPU 响应能力方面的问题。
//                  如果该工作同时适用于并发和并行，还应考虑使用任务并行库。

internal static partial class Program
{

    class User
    {
        public int id;
    }

    static async Task<User> GetUserAsync(int userId)
    {
        // Code omitted:
        //
        // Given a user Id {userId}, retrieves a User object corresponding
        // to the entry in the database with {userId} as its Id.

        await Task.Yield(); // Simulate asynchronous database retrieval
        return new User() { id = userId };
    }

    static async Task<IEnumerable<User>> GetUsersAsync(IEnumerable<int> userIds)
    {
        var getUserTasks = new List<Task<User>>();
        foreach (int userId in userIds)
        {
            getUserTasks.Add(GetUserAsync(userId));
        }

        return await Task.WhenAll(getUserTasks);
    }

    static async Task<IEnumerable<User>> GetUsersAsyncV2(IEnumerable<int> userIds)
    {
        var getUserTasks = userIds.Select(id => GetUserAsync(id)).ToArray();
        return await Task.WhenAll(getUserTasks);
    }

    static async Task<IEnumerable<User>> GetUsersAsyncV3(IEnumerable<int> userIds)
    {
        return await Task.WhenAll(userIds.Select(GetUserAsync));
    }
}
