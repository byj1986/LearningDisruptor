// 阅读：使用 async 和 await 进行异步编程
// https://learn.microsoft.com/zh-cn/dotnet/csharp/asynchronous-programming/
//
// 早餐流程（MakeBreakfast 系列三个方法共用）：
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
        var sw = System.Diagnostics.Stopwatch.StartNew();
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
        Log("早餐", $"耗时 {sw.ElapsedMilliseconds}ms");
        sw.Stop();
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
        var sw = System.Diagnostics.Stopwatch.StartNew();
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
        Log("早餐", $"耗时 {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }

    /// <summary>
    /// 先启动三个任务再依次 await，三个步骤并发执行。
    /// 大约需要6秒，即 Max(FryEggs 6000, FryHashBrowns 6000, ToastBread 3000)。
    /// </summary>
    private static async Task MakeBreakfastAsyncV2()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        LogPhase("异步并发（先启动，再按需要等待）");

        Coffee cup = PourCoffee();
        Log("咖啡", "完成");
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

        // Task<Egg> eggsTask = FryEggsAsync(2);
        // Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
        // Task<Toast> toastTask = ToastBreadAsync(2);

        // var eggsTask = FryEggsAsync(2);
        // var hashBrownTask = FryHashBrownsAsync(3);
        // var toastTask = ToastBreadAsync(2);

        // var eggs = await eggsTask;
        // Console.WriteLine("eggs are ready");

        // var hashBrown = await hashBrownTask;
        // Console.WriteLine("hash browns are ready");

        // var toast = await toastTask;
        // Console.WriteLine("toast is ready");
        // await Task.WhenAll(eggsTask, hashBrownTask, toastTask);
        // Juice oj = PourOrangeJuice();
        // Console.WriteLine("oj is ready");
        // Console.WriteLine("Breakfast is ready!");
        Log("早餐", $"耗时 {sw.ElapsedMilliseconds}ms");
        sw.Stop();
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

    private static void LogPhase(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"== {title} ==");
    }

    private static void Log(string step, string message) =>
        Console.WriteLine($"[{step}] {message}");

    #endregion
}
