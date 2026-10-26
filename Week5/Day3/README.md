# Week5/Day3（全程第23天）：吞吐量与分配测量

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

内存管理书中的测量与分配章节；复习Stopwatch与GC统计。

[内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

## 知识点

- 预热、Release、重复测量、进程级统计。
- 计时覆盖最后一条事件处理完成，验证与性能测量分开。

## 编码与分析任务

- [ ] 分别扩展已有Disruptor与Channel项目，不新建重复项目。
- [ ] 关闭调试器、移除逐条输出；预热后每组测量至少10秒，重复5轮。
- [ ] 记录已完成事件数、耗时、吞吐量、进程CPU、分配量和GC次数。跨线程实验不要只读当前线程分配量。

## 验收

- [ ] 保留5轮原始数据，并明确计时起止和预热方式。
- [ ] 不是仅测生产者发布速度；正确性场景事先通过。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
