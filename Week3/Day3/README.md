# Week3/Day3（全程第13天）：菱形依赖

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

继续已有项目，本目录不重复创建csproj。

- [Disruptor.OrderPipeline](../../Week2/Day1/Disruptor.OrderPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "../../Week2/Day1/Disruptor.OrderPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

事件处理器；LMAX指南消费者依赖图。

[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 并行分支与汇合；下游等待所有前置阶段。
- 最慢前置阶段如何限制汇合阶段的进度。

## 编码与分析任务

- [ ] 实现 A校验、B独立计算费用 → C汇总，A/B分别写自己的字段。
- [ ] 故意放慢A，再放慢B，观察C的处理进度。
- [ ] 画出依赖图并标注各处理器持有的进度。

## 验收

- [ ] C处理某订单时A/B均已完成该订单。
- [ ] 能说明哪些工作可以并行，以及共享写入的边界。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
