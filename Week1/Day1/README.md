# Week1/Day1（全程第1天）：线程、Task 与异步

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

今天有新建空项目。

- [Fundamentals.ConcurrencyLab](Fundamentals.ConcurrencyLab.csproj)

在本日目录运行：

```powershell
dotnet run --project "Fundamentals.ConcurrencyLab.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

书 A 第1章、第2章等待任务与异常处理；书 B 第26章。

[书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)

## 知识点

- 并发、并行、异步的区别。
- Thread、线程池、Task、await 的关系；Join 与等待任务。

## 编码与分析任务

- [ ] 分别用 Thread、Task.Run 执行计算工作，等待全部工作完成。
- [ ] 比较 Thread.Sleep 与 Task.Delay，记录线程编号与等待期间的行为。
- [ ] 观察异步方法在 await 前后的执行位置，不把一次观察当作线程调度保证。

## 验收

- [ ] 能解释 Task 是否等于线程、await 是否必然创建线程。
- [ ] 能区分阻塞与异步等待；程序不提前退出。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
