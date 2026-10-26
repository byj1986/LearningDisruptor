# Week1/Day3（全程第3天）：内存可见性与线程协作

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

继续已有项目，本目录不重复创建csproj。

- [Fundamentals.ConcurrencyLab](../Day1/Fundamentals.ConcurrencyLab.csproj)

在本日目录运行：

```powershell
dotnet run --project "../Day1/Fundamentals.ConcurrencyLab.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

书 B 第29章；Volatile、Interlocked、同步原语。

[书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[同步原语](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)；[Volatile](https://learn.microsoft.com/en-us/dotnet/api/system.threading.volatile?view=net-10.0)

## 知识点

- 原子性、可见性、操作顺序的区别。
- Volatile.Read/Write、CompareExchange；信号通知与超时。

## 编码与分析任务

- [ ] 实现一条线程写数据后发布就绪标志，另一条线程观察标志后读取数据，使用 Volatile.Read/Write。
- [ ] 用 ManualResetEventSlim 实现同样的完成通知，并设置超时和退出条件。
- [ ] 尝试用 CompareExchange 循环实现共享计数器，观察失败重试。

## 验收

- [ ] 能解释先写数据、后发布标志的意义。
- [ ] 知道 volatile 不能让 count++ 成为原子操作；所有等待均可结束。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
