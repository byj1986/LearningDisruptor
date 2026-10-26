# Week5/Day5（全程第25天）：复盘与选型报告

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

继续已有项目，本目录不重复创建csproj。

- [Disruptor.OrderPipeline](../../Week2/Day1/Disruptor.OrderPipeline.csproj)
- [Comparison.ChannelPipeline](../Day2/Comparison.ChannelPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "../../Week2/Day1/Disruptor.OrderPipeline.csproj"
dotnet run --project "../Day2/Comparison.ChannelPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

回顾全计划与实验记录；按问题回查源码和文档。

[源码](https://github.com/disruptor-net/Disruptor-net)

## 知识点

- 区分实验事实、原因推断和适用边界。
- 记录环境、版本、拓扑、配置和可复现步骤。

## 编码与分析任务

- [ ] 在当天README中完成报告：环境、正确性、吞吐量、延迟、CPU、分配与GC。
- [ ] 说明何时选Disruptor、何时选Channel、哪些结论还需验证。
- [ ] 从解决方案重新构建，按记录复现关键实验。

## 验收

- [ ] 另一位开发者能按说明复现实验。
- [ ] 能用自己的数据说明选型理由，列明限制与未解决问题。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
