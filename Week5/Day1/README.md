# Week5/Day1（全程第21天）：整理完整订单项目

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

复习入门、发布与背压、事件处理器、等待策略。

[入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)；[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)

## 知识点

- 事件所有权、拓扑、背压、关闭与配置组合。
- 用一致输入与单线程参考核对复杂流水线。

## 编码与分析任务

- [ ] 继续已有订单项目，将数量、容量、生产者数、拓扑、等待策略改为可配置。
- [ ] 整理正常、无效订单、慢消费者、突发输入四种场景。
- [ ] 确保从同一个程序入口可以重复运行各场景。

## 验收

- [ ] 正常场景结果可与单线程流程核对。
- [ ] 异常和过载有明确统计；不额外复制一套相同项目。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
