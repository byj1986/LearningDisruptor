# Week3/Day2（全程第12天）：串行处理流水线

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

事件处理器的依赖配置；LMAX指南消费者依赖图。

[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 前置阶段处理完成后，下游才能处理对应事件。
- 校验、计算、汇总各自的字段所有权。

## 编码与分析任务

- [ ] 扩展为校验 → 费用计算 → 汇总。
- [ ] 校验填写有效标志，计算填写费用，汇总区分正常与无效订单。
- [ ] 写一个单线程参考流程核对最终结果。

## 验收

- [ ] 流水线结果与单线程参考一致。
- [ ] 无效订单规则明确，下游不会使用未设置或遗留的字段。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
