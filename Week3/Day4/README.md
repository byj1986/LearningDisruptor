# Week3/Day4（全程第14天）：多生产者

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

入门、源码中生产者配置；LMAX指南。

[入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[源码](https://github.com/disruptor-net/Disruptor-net)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 单生产者与多生产者配置。
- 业务编号、生产者局部编号、RingBuffer序号之间的区别。

## 编码与分析任务

- [ ] 使用两个生产者，各发布5万条订单，并选择多生产者配置。
- [ ] 事件增加 ProducerId 与生产者内部编号。
- [ ] 检查唯一性、遗漏、重复及每个生产者自身的发布顺序。

## 验收

- [ ] 总计10万条，无遗漏或重复。
- [ ] 不会使用线程启动时间或业务编号推断跨生产者全局顺序。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
