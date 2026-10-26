# Week3/Day1（全程第11天）：广播消费

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

事件处理器；LMAX指南的 Multicast Events。

[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 多个普通事件处理器分别接收同一个事件。
- 广播与队列竞争消费的区别；共享事件的写入所有权。

## 编码与分析任务

- [ ] 配置两个并行处理器，一个统计订单数量，一个统计金额。
- [ ] 处理器只读事件，并各自维护自己的统计状态。
- [ ] 发布10万条并单独检查两个处理器的计数。

## 验收

- [ ] 两个处理器都收到10万条，而不是各5万条。
- [ ] 能说明并行写同一字段为什么需要额外协调。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
