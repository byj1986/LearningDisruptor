# Week4/Day2（全程第17天）：单生产者序号协调

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

源码中的单生产者协调器；LMAX指南。

[源码](https://github.com/disruptor-net/Disruptor-net)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 申请位置、回绕检查、末端消费者进度约束。
- 消费者未释放的槽位不能被覆盖。

## 编码与分析任务

- [ ] 阅读申请、容量检查、发布相关分支，写简短注释。
- [ ] 推演容量8、末端消费者已处理到3、生产者已申请到11的状态。
- [ ] 解释为何此时不能申请12，以及消费者推进后何时恢复。

## 验收

- [ ] 能用序号与容量判断覆盖风险。
- [ ] 能在源码中找到相应检查，而不是只背结论。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
