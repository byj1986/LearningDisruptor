# Week1/Day2（全程第2天）：共享状态与原子操作

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

书 A 第12章阻塞锁；书 B 第29、30章相关部分；同步原语、Interlocked。

[书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)；[同步原语](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)

## 知识点

- 竞争条件、临界区、互斥与原子操作。
- ++ 包含读取、计算、写回多个步骤。

## 编码与分析任务

- [ ] 创建4个任务，每个任务对共享计数器递增10万次。
- [ ] 分别实现普通 ++、lock、Interlocked.Increment 三个版本并重复运行。
- [ ] 记录预期值40万、实际值与粗略耗时，先验证正确性。

## 验收

- [ ] 两个正确同步版本均得到40万。
- [ ] 能解释普通版本偶尔正确为何不能证明线程安全。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
