# Week2/Day4（全程第9天）：批量发布与消费批次

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

继续已有项目，本目录不重复创建csproj。

- [Disruptor.OrderPipeline](../Day1/Disruptor.OrderPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "../Day1/Disruptor.OrderPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

发布与背压的 Batches；事件处理器。

[发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)；[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)

## 知识点

- 批量申请、填充、发布。
- endOfBatch 是当前可消费批次的结束，不保证与发布批次一一对应。

## 编码与分析任务

- [ ] 用单条、16条、64条批量发布相同数量订单。
- [ ] 记录消费批次大小，在消费批次结束时更新汇总。
- [ ] 比较各种方式的数量与金额总和，并覆盖最后不足一批的情况。

## 验收

- [ ] 各种发布方式业务结果一致。
- [ ] 能区分生产者批次和消费者批次。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
