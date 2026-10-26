# Week4/Day5（全程第20天）：等待策略

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

等待策略文档与对应版本实现；书 B 线程调度相关内容。

[书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)

## 知识点

- 阻塞、让出线程、自旋；等待新事件与等待依赖阶段。
- CPU、核心数量、系统负载与延迟的权衡。

## 编码与分析任务

- [ ] 在同一程序中切换 BlockingWaitStrategy、YieldingWaitStrategy、BusySpinWaitStrategy。
- [ ] 观察空闲、持续负载、突发负载时的CPU和响应。
- [ ] 记录硬件、运行配置和三种策略的初步对照表。

## 验收

- [ ] 能同时解释CPU成本与延迟表现。
- [ ] 不把某次测量更快等同于所有场景都更合适。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
