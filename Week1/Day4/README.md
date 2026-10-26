# Week1/Day4（全程第4天）：有界队列、背压与取消

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

今天有新建空项目。

- [Fundamentals.ChannelLab](Fundamentals.ChannelLab.csproj)

在本日目录运行：

```powershell
dotnet run --project "Fundamentals.ChannelLab.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

书 A 第9章队列、第10章取消；Channels 文档。

[书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)；[Channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)

## 知识点

- 生产速度、消费速度、容量与背压。
- 正常完成写入与主动取消处理的区别。

## 编码与分析任务

- [ ] 建立容量64、BoundedChannelFullMode.Wait 的 Channel<T>，发布1万条编号消息。
- [ ] 让消费者偶尔延迟，观察 WriteAsync 等待；异步等待不等于阻塞线程。
- [ ] 分别实现正常完成和主动取消，记录已接受、已处理、未完成数量。

## 验收

- [ ] 正常完成时1万条全部消费。
- [ ] 能说明满时行为、完成与取消的区别，以及取消后未处理数据如何统计。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
