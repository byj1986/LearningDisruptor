# Week5/Day2（全程第22天）：Channel 对照实现

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

今天有新建空项目。

- [Comparison.ChannelPipeline](Comparison.ChannelPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "Comparison.ChannelPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

复习第4天项目与 Channels 文档。

[Channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)

## 知识点

- 比较对象必须具有相同容量、载荷、业务逻辑和投递语义。
- 满时等待及相同完成条件。

## 编码与分析任务

- [ ] 建立Channel订单对照项目，借鉴第4天手写代码。
- [ ] 先实现与Disruptor相同的单生产者/单消费者、相同容量、满时等待。
- [ ] 统一输入和业务计算；若后续比较广播，再补齐相同投递行为。

## 验收

- [ ] 两个实现业务结果一致。
- [ ] 明确配置差异，不能把广播处理与竞争分摊直接比较。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
