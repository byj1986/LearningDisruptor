# Week4/Day3（全程第18天）：多生产者序号协调

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

源码中的多生产者协调器；Interlocked。

[源码](https://github.com/disruptor-net/Disruptor-net)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)

## 知识点

- CAS争用、发布可用性与连续可消费范围。
- 已申请、已发布、可连续读取三个状态不同。

## 编码与分析任务

- [ ] 让A申请较早序号后暂缓发布，B先发布后续序号，再恢复A。
- [ ] 用信号控制实验时序，设置超时，避免无控制的随机休眠。
- [ ] 记录消费者进度并找到源码中处理发布缺口的位置。

## 验收

- [ ] 能解释消费者为何不能越过未发布缺口。
- [ ] 能说明多生产者比单生产者额外协调了什么。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
