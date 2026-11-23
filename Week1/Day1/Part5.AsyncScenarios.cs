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

using System.Text.RegularExpressions;

internal static partial class Program
{

    class User
    {
        public bool isEnabled
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }
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

    static async Task GetUsersAsyncV4(IEnumerable<int> userIds)
    {
        var getUserTasks = userIds.Select(GetUserAsync).ToList();

        while (getUserTasks.Count > 0)
        {
            // 在需要动态管理任务时使用 ToList() ，例如 Task.WhenAny ，在任务完成时，可以从集合中删除已完成的任务。
            Task<User> completedTask = await Task.WhenAny(getUserTasks);
            getUserTasks.Remove(completedTask);

            User user = await completedTask;
            Console.WriteLine($"Processed user {user.id}");
        }

        // var getUserTasks2 = userIds.Select(GetUserAsync).ToArray();
        // // 在计划将所有任务一起处理时使用ToArray()，例如Task.WhenAll。对于集合大小固定的方案，数组是有效的。
        // var users2 = await Task.WhenAll(getUserTasks);
    }

    private static readonly IEnumerable<string> s_urlList = new string[]
    {
            "https://learn.microsoft.com",
            "https://learn.microsoft.com/aspnet/core",
            "https://learn.microsoft.com/azure",
            "https://learn.microsoft.com/azure/devops",
            "https://learn.microsoft.com/dotnet",
            // "https://learn.microsoft.com/dotnet/desktop/wpf/get-started/create-app-visual-studio",
            // "https://learn.microsoft.com/education",
            // "https://learn.microsoft.com/shows/net-core-101/what-is-net",
            // "https://learn.microsoft.com/enterprise-mobility-security",
            // "https://learn.microsoft.com/gaming",
            // "https://learn.microsoft.com/graph",
            // "https://learn.microsoft.com/microsoft-365",
            // "https://learn.microsoft.com/office",
            // "https://learn.microsoft.com/powershell",
            // "https://learn.microsoft.com/sql",
            // "https://learn.microsoft.com/surface",
            // "https://dotnetfoundation.org",
            // "https://learn.microsoft.com/visualstudio",
            // "https://learn.microsoft.com/windows",
            // "https://learn.microsoft.com/maui"
    };

    private static readonly HttpClient s_httpClient = new();
    // [HttpGet, Route("DotNetCount")]
    static public async Task<int> GetDotNetCountAsync(string URL)
    {
        // Suspends GetDotNetCountAsync() to allow the caller (the web server)
        // to accept another request, rather than blocking on this one.
        var html = await s_httpClient.GetStringAsync(URL);
        return Regex.Count(html, @"\.NET");
    }

    private static async Task ProcessTasksAsTheyCompleteAsync(IEnumerable<int> userIds)
    {
        var getUserTasks = userIds.Select(id => GetUserAsync(id)).ToList();

        while (getUserTasks.Count > 0)
        {
            Task<User> completedTask = await Task.WhenAny(getUserTasks);
            getUserTasks.Remove(completedTask);

            User user = await completedTask;
            Console.WriteLine($"Processed user {user.id}");
        }
    }
    static async Task GetUsersAsyncV5(IEnumerable<int> userIds)
    {
        Console.WriteLine("Application started.");

        Console.WriteLine("Counting '.NET' phrase in websites...");
        int total = 0;
        foreach (string url in s_urlList)
        {
            var result = await GetDotNetCountAsync(url);
            Console.WriteLine($"{url}: {result}");
            total += result;
        }
        Console.WriteLine("Total: " + total);

        Console.WriteLine("Retrieving User objects with list of IDs...");
        IEnumerable<int> ids = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        var users = await GetUsersAsync(ids);
        foreach (User? user in users)
        {
            Console.WriteLine($"{user.id}: isEnabled={user.isEnabled}");
        }

        Console.WriteLine("Processing tasks as they complete...");
        await ProcessTasksAsTheyCompleteAsync(ids);

        Console.WriteLine("Application ending.");
    }
}
