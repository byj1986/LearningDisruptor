// 阅读：使用 async 和 await 进行异步编程
// https://learn.microsoft.com/zh-cn/dotnet/csharp/asynchronous-programming/
//
// 早餐流程（MakeBreakfast 系列方法共用）：
// V1：启动后立即逐个 await，仍近似顺序执行；V2：先全部启动再按需 await，实现并发。
// V3：把“烤面包+涂黄油+涂果酱”组合成一个异步操作；V4：改用会抛异常的烤面包方法，观察异步异常。
// 1:倒一杯咖啡。
// 2:加热锅，然后炒两个鸡蛋。
// 3:烹制三个薯饼。
// 4:烤两块面包。
// 5:将黄油和果酱涂抹在烤面包片上。
// 6:倒一杯橙汁。
internal static partial class Program
{
    #region 类型

    /// <summary>薯饼。</summary>
    internal class HashBrown { }

    /// <summary>咖啡。</summary>
    internal class Coffee { }

    /// <summary>鸡蛋。</summary>
    internal class Egg { }

    /// <summary>橙汁。</summary>
    internal class Juice { }

    /// <summary>面包。</summary>
    internal class Toast { }

    #endregion

    #region 同步

    /// <summary>
    /// 同步顺序执行早餐流程中的所有步骤。
    /// 大约需要15秒(FryEggs 6000 + FryHashBrowns 6000 + ToastBread 3000)。
    /// </summary>
    private static void MakeBreakfast()
    {
        LogPhase("同步顺序");

        Coffee cup = PourCoffee();
        Log("咖啡", "完成");

        Egg eggs = FryEggs(2);
        Log("鸡蛋", "完成");

        HashBrown hashBrown = FryHashBrowns(3);
        Log("薯饼", "完成");

        Toast toast = ToastBread(2);
        ApplyButter(toast);
        ApplyJam(toast);
        Log("面包", "完成");

        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");
        Log("早餐", "完成");
    }

    /// <summary>
    /// 加热锅，然后炒两个鸡蛋。
    /// 热锅需要3000ms，打蛋瞬时完成，炒鸡蛋需要3000ms，放入餐盘瞬时完成。总共需要6000ms。
    /// </summary>
    private static Egg FryEggs(int count)
    {
        Log("鸡蛋", "热锅");
        Task.Delay(3000).Wait();
        Log("鸡蛋", $"打入 {count} 个");
        Log("鸡蛋", "煎制");
        Task.Delay(3000).Wait();
        Log("鸡蛋", "装盘");

        return new Egg();
    }

    /// <summary>
    /// 烹制三个薯饼。
    /// 放入薯饼瞬时完成，两面各煎3000ms，放入餐盘瞬时完成。总共需要6000ms。
    /// </summary>
    private static HashBrown FryHashBrowns(int patties)
    {
        Log("薯饼", $"放入 {patties} 个，煎第一面");
        Task.Delay(3000).Wait();
        for (int patty = 0; patty < patties; patty++)
        {
            Log("薯饼", $"翻面 {patty + 1}/{patties}");
        }
        Log("薯饼", "煎第二面");
        Task.Delay(3000).Wait();
        Log("薯饼", "装盘");

        return new HashBrown();
    }

    /// <summary>
    /// 烤两块面包。
    /// 面包片放入烤面包机瞬时完成，烘烤需要3000ms，取出瞬时完成。总共需要3000ms。
    /// </summary>
    private static Toast ToastBread(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Log("面包", $"放入第 {slice + 1}/{slices} 片");
        }
        Log("面包", "开始烘烤");
        Task.Delay(3000).Wait();
        Log("面包", "取出");

        return new Toast();
    }

    #endregion

    #region 异步

    /// <summary>
    /// 虽然使用了 async，但是每个任务都 await 后再继续，因此不会并发执行。
    /// 最终仍然需要大约15秒。
    /// </summary>
    private static async Task MakeBreakfastAsyncV1()
    {
        LogPhase("异步但逐步等待");

        Coffee cup = PourCoffee();
        Log("咖啡", "完成");

        Egg eggs = await FryEggsAsync(2);
        Log("鸡蛋", "完成");

        HashBrown hashBrown = await FryHashBrownsAsync(3);
        Log("薯饼", "完成");

        Toast toast = await ToastBreadAsync(2);
        ApplyButter(toast);
        ApplyJam(toast);
        Log("面包", "完成");

        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");
        Log("早餐", "完成");
    }

    /// <summary>
    /// 先启动三个任务再依次 await，三个步骤并发执行。
    /// 大约需要6秒，即 Max(FryEggs 6000, FryHashBrowns 6000, ToastBread 3000)。
    /// </summary>
    private static async Task MakeBreakfastAsyncV2()
    {
        Coffee cup = PourCoffee();
        Log("咖啡", "完成");
        // 调用异步方法即启动任务（热任务）：先让三个任务并发跑起来，再按“先需要谁、先 await 谁”取结果。
        Task<Egg> eggsTask = FryEggsAsync(2);
        Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
        Task<Toast> toastTask = ToastBreadAsync(2);

        Toast toast = await toastTask;
        ApplyButter(toast);
        ApplyJam(toast);
        Log("面包", "完成");
        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");

        Egg eggs = await eggsTask;
        Log("鸡蛋", "完成");
        HashBrown hashBrown = await hashBrownTask;
        Log("薯饼", "完成");
    }


    /// <summary>
    /// 在 V2 并发启动的基础上，把有先后依赖的“烤面包 → 涂黄油 → 涂果酱”组合成
    /// <see cref="MakeToastWithButterAndJamAsync"/> 一个异步操作，再与鸡蛋、薯饼并发。
    /// 三个任务先全部启动、后依次 await；await 的顺序只决定等待与记录的顺序，不影响并发，总耗时约 6 秒。
    /// </summary>
    private static async Task MakeBreakfastAsyncV3()
    {
        var eggsTask = FryEggsAsync(2);
        var hashBrownTask = FryHashBrownsAsync(3);
        var toastTask = MakeToastWithButterAndJamAsync(2);

        var eggs = await eggsTask;
        Log("鸡蛋", "完成");

        var hashBrown = await hashBrownTask;
        Log("薯饼", "完成");

        var toast = await toastTask;
        Log("面包", "完成");

        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");
        Log("早餐", "完成");
    }

    /// <summary>
    /// 在 V2 的结构上改用会故障的 <see cref="ToastBreadAsyncV2"/>，演示异步异常的传播时机：
    /// 调用该方法只是启动任务并拿到一个将要故障的 Task，异常不会在调用处同步抛出；
    /// 直到 <c>await toastTask</c> 时才重新抛出，并经本方法与 Main 向上传播（当前未捕获，程序会以未处理异常结束）。
    /// 因此后面的涂抹、橙汁以及对鸡蛋/薯饼任务的 await 都不会执行，已启动的两个任务其结果也不再被观察。
    /// </summary>
    private static async Task MakeBreakfastAsyncV4()
    {
        Coffee cup = PourCoffee();
        Log("咖啡", "完成");
        Task<Egg> eggsTask = FryEggsAsync(2);
        Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
        Task<Toast> toastTask = ToastBreadAsyncV2(2);

        // 此处 await 会重新抛出 toastTask 中保存的异常，本方法后续代码不再执行。
        Toast toast = await toastTask;
        ApplyButter(toast);
        ApplyJam(toast);
        Log("面包", "完成");
        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");

        Egg eggs = await eggsTask;
        Log("鸡蛋", "完成");
        HashBrown hashBrown = await hashBrownTask;
        Log("薯饼", "完成");
    }

    /// <summary>
    /// 在 V2 的结构上改用 <see cref="Task.WhenAny"/> 实现“先完成谁、先吃谁”，演示非阻塞并发：
    /// 三个任务先全部启动、后依次 await；await 的顺序只决定等待与记录的顺序，不影响并发，总耗时约 6 秒。
    /// 虽然 await 的顺序不影响并发，但 await 的时机会影响“先完成谁、先吃谁”的顺序：
    /// 先完成的任务会先被观察并从列表中移除，因此后续 await 会按新的顺序等待剩余任务。
    /// 例如，如果鸡蛋先完成，则后续的涂抹、橙汁以及对薯饼任务的 await 都会等待该任务完成；
    /// 如果薯饼先完成，则后续的涂抹、橙汁以及对鸡蛋任务的 await 都会等待该任务完成。
    /// 这种非阻塞并发的实现方式，可以显著提高程序的响应性和吞吐量，尤其在处理大量并发任务时效果更明显。
    /// </summary>
    private static async Task MakeBreakfastAsyncV5()
    {
        Coffee cup = PourCoffee();
        Log("咖啡", "完成");

        var eggsTask = FryEggsAsync(2);
        var hashBrownTask = FryHashBrownsAsync(3);
        var toastTask = MakeToastWithButterAndJamAsync(2);

        var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };
        while (breakfastTasks.Count > 0)
        {
            Task finishedTask = await Task.WhenAny(breakfastTasks);
            if (finishedTask == eggsTask)
            {
                Log("鸡蛋", "完成");
            }
            else if (finishedTask == hashBrownTask)
            {
                Log("薯饼", "完成");
            }
            else if (finishedTask == toastTask)
            {
                Log("面包", "完成");
            }
            await finishedTask;
            breakfastTasks.Remove(finishedTask);
        }

        Juice oj = PourOrangeJuice();
        Log("橙汁", "完成");
        Log("早餐", "完成");
    }

    private static Task MakeBreakfastAsyncV6()
    {
        return FryEggsAsync(2)
               .ContinueWith(eggsTask =>
               {
                   var eggs = eggsTask.Result;
                   Log("鸡蛋", "完成");
                   return FryHashBrownsAsync(3);
               })
               .Unwrap()
               .ContinueWith(hashBrownTask =>
               {
                   var hashBrown = hashBrownTask.Result;
                   Log("薯饼", "完成");
                   return MakeToastWithButterAndJamAsync(2);
               })
               .Unwrap()
               .ContinueWith(toastTask =>
               {
                   var toast = toastTask.Result;
                   Log("面包", "涂黄油");
                   return ApplyButterAsync(toast);
               })
               .Unwrap()
               .ContinueWith(butteredToastTask =>
               {
                   var butteredToast = butteredToastTask.Result;
                   Log("面包", "涂黄油");
                   return ApplyJamAsync(butteredToast);
               })
               .Unwrap()
               .ContinueWith(finalToastTask =>
               {
                   Log("早餐", "完成");
               });
    }

    private static async Task MakeBreakfastAsyncV7()
    {
        var eggs = await FryEggsAsync(2);
        Log("鸡蛋", "完成");

        var bacon = await FryHashBrownsAsync(3);
        Log("薯饼", "完成");

        var toast = await StartToastingBreadAsync();
        Log("面包", "完成");

        var butteredToast = await ApplyButterAsync(toast);
        Log("面包", "涂黄油");

        var finalToast = await ApplyJamAsync(butteredToast);
        Log("早餐", "完成");
    }

    private static async Task<Toast> StartToastingBreadAsync()
    {
        return await ToastBreadAsync(2);
    }

    private static async Task<Toast> ApplyButterAsync(Toast toast)
    {
        ApplyButter(toast);
        return toast;
    }

    private static async Task<Toast> ApplyJamAsync(Toast toast)
    {
        ApplyJam(toast);
        return toast;
    }

    /// <summary>
    /// 组合有依赖关系的面包步骤：必须先等面包烤好，才能依次涂黄油、涂果酱。
    /// 涂抹本身是瞬时同步操作，直接调用即可，无需再包装成异步方法。
    /// </summary>
    private static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
    {
        var toast = await ToastBreadAsync(number);
        ApplyButter(toast);
        ApplyJam(toast);
        return toast;
    }

    /// <summary>
    /// 加热锅，然后炒两个鸡蛋。热锅 3000ms，煎制 3000ms。
    /// </summary>
    private static async Task<Egg> FryEggsAsync(int count)
    {
        Log("鸡蛋", "热锅");
        await Task.Delay(3000);
        Log("鸡蛋", $"打入 {count} 个");
        Log("鸡蛋", "煎制");
        await Task.Delay(3000);
        Log("鸡蛋", "装盘");

        return new Egg();
    }

    /// <summary>
    /// 烹制三个薯饼。两面各煎 3000ms。
    /// </summary>
    private static async Task<HashBrown> FryHashBrownsAsync(int patties)
    {
        Log("薯饼", $"放入 {patties} 个，煎第一面");
        await Task.Delay(3000);
        for (int patty = 0; patty < patties; patty++)
        {
            Log("薯饼", $"翻面 {patty + 1}/{patties}");
        }
        Log("薯饼", "煎第二面");
        await Task.Delay(3000);
        Log("薯饼", "装盘");

        return new HashBrown();
    }

    /// <summary>
    /// 故意写错的烤面包版本，用于演示异步异常：烘烤 2 秒后抛出
    /// <see cref="System.InvalidOperationException"/>，模拟烤面包机起火。
    /// 异常会被存入返回的 Task，直到调用方 await 时才重新抛出；throw 之后的代码不可达、不会执行。
    /// </summary>
    private static async Task<Toast> ToastBreadAsyncV2(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Log("面包", $"放入第 {slice + 1}/{slices} 片");
        }
        Log("面包", "开始烘烤");
        await Task.Delay(2000);
        Log("面包", "烤糊了！");
        // 模拟故障点：异常在此被捕获进 Task，方法随即以故障状态结束，以下语句不可达。
        throw new InvalidOperationException("The toaster is on fire");
        await Task.Delay(1000);
        Log("面包", "取出");

        return new Toast();
    }


    /// <summary>
    /// 烤两块面包。烘烤 3000ms。
    /// </summary>
    private static async Task<Toast> ToastBreadAsync(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Log("面包", $"放入第 {slice + 1}/{slices} 片");
        }
        Log("面包", "开始烘烤");
        await Task.Delay(3000);
        Log("面包", "取出");

        return new Toast();
    }

    #endregion

    #region 共用步骤

    /// <summary>
    /// 倒一杯咖啡。瞬时完成。
    /// </summary>
    private static Coffee PourCoffee()
    {
        Log("咖啡", "倒出");
        return new Coffee();
    }

    /// <summary>
    /// 将黄油涂抹在烤面包片上。瞬时完成。
    /// </summary>
    private static void ApplyButter(Toast toast) =>
        Log("面包", "涂黄油");

    /// <summary>
    /// 将果酱涂抹在烤面包片上。瞬时完成。
    /// </summary>
    private static void ApplyJam(Toast toast) =>
        Log("面包", "涂果酱");

    /// <summary>
    /// 倒一杯橙汁。瞬时完成。
    /// </summary>
    private static Juice PourOrangeJuice()
    {
        Log("橙汁", "倒出");
        return new Juice();
    }

    /// <summary>打印阶段标题，便于在控制台对比不同版本的执行顺序。</summary>
    private static void LogPhase(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"== {title} ==");
    }

    /// <summary>统一日志格式“[步骤] 消息”，只用于观察顺序与相对耗时，不改变早餐流程本身。</summary>
    private static void Log(string step, string message) =>
        Console.WriteLine($"[{step}] {message}");

    #endregion
}
