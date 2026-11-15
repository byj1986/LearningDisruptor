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
    /// 同步顺序执行早餐流程中的所有步骤。
    /// 大约需要15秒(FryEggs 6000 + FryHashBrowns 6000 + ToastBread 3000)
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
    /// 先启动三个任务再依次await，三个步骤并发执行。
    /// 大约需要6秒Max(FryEggs 6000, FryHashBrowns 6000, ToastBread 3000)
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
}
