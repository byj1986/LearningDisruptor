internal static class Program
{
    public static void DoWork()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Working thread... {i}");
            Thread.Sleep(100);
        }
    }

    private static void Main()
    {
        // 请按照当天 README.md 手写练习代码。
        Thread thread1 = new Thread(DoWork);
        thread1.Start();
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"In main... {i}");
            Thread.Sleep(100);
        }
    }
}
