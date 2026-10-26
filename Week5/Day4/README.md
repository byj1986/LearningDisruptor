# Week5/Day4（全程第24天）：延迟、容量与突发流量

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

内存管理书选读；复习等待策略与容量实验。

[等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)；[内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

## 知识点

- 平均值、P50、P99、最大值；时间戳位置决定延迟含义。
- 排队、背压与发送速率；测量本身也有成本。

## 编码与分析任务

- [ ] 用 Stopwatch.GetTimestamp 测量尝试发布前 → 最终处理完成的延迟。
- [ ] 预分配测量存储，单独运行延迟实验，记录时间戳开销与测量边界。
- [ ] 测试低负载、接近饱和、突发输入，记录目标/实际发送速率；改变容量观察排队。

## 验收

- [ ] 能解释吞吐量相近时P99为何仍有差异。
- [ ] 报告明确是否包含申请槽位等待，说明更大容量对延迟的影响。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
