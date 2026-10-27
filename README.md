# LearningDisruptor

5周、25个学习日。所有练习代码由你手写，空入口仅用于确保项目可构建。

每日说明采用120分钟实验单元，可拆成两次60分钟。每个README都有6段执行步骤、3组验证场景、排错提示、验收问题及空白实验表；预期结果与实际记录分开填写。先完成核心验收再做选做，源码阅读固定对照Disruptor 6.0.1对应提交。

核心阅读已改用免费官方文章；原书章节对照、文章范围和自写复习笔记见[阅读资料.md](阅读资料.md)，每天README提供直接链接。

## 开始使用

1. 在Visual Studio中打开LearningDisruptor.sln，或在本目录运行以下命令。
2. 每天打开对应WeekM/DayN/README.md，按其中链接进入真正需要修改的项目。
3. 续用日只提供任务说明，不重复生成项目；Day编号在每周重置为1～5，全程日号另行标注。

```powershell
dotnet restore ./LearningDisruptor.sln
dotnet build ./LearningDisruptor.sln -c Release
dotnet run --project ./Week1/Day1/Fundamentals.ConcurrencyLab.csproj
```

目标框架net10.0；global.json使用本机检测到的SDK 10.0.402。Disruptor.OrderPipeline已声明Disruptor 6.0.1，其余项目仅依赖.NET自带库。首次还原需要访问nuget.org。

完整计划见本仓库中的[Disrupt学习计划.md](Disrupt学习计划.md)。本目录中没有预先完成的事件模型、处理器、同步练习或性能实验。

## 每日导航

| 全程日 | 每日目录 | 主题 | 项目安排 |
| --- | --- | --- | --- |
| 1 | [Week1/Day1](Week1/Day1/README.md) | 线程、Task 与异步 | 新建：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) |
| 2 | [Week1/Day2](Week1/Day2/README.md) | 共享状态与原子操作 | 续用：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) |
| 3 | [Week1/Day3](Week1/Day3/README.md) | 内存可见性与线程协作 | 续用：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) |
| 4 | [Week1/Day4](Week1/Day4/README.md) | 有界队列、背压与取消 | 新建：[Fundamentals.ChannelLab](Week1/Day4/Fundamentals.ChannelLab.csproj) |
| 5 | [Week1/Day5](Week1/Day5/README.md) | 环形缓冲区与基础测量 | 新建：[Fundamentals.RingBufferLab](Week1/Day5/Fundamentals.RingBufferLab.csproj); [Fundamentals.ChannelLab](Week1/Day4/Fundamentals.ChannelLab.csproj) |
| 6 | [Week2/Day1](Week2/Day1/README.md) | Disruptor 最小示例 | 新建：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 7 | [Week2/Day2](Week2/Day2/README.md) | 申请、填写、发布 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 8 | [Week2/Day3](Week2/Day3/README.md) | 序号、回绕与对象复用 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 9 | [Week2/Day4](Week2/Day4/README.md) | 批量发布与消费批次 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 10 | [Week2/Day5](Week2/Day5/README.md) | 缓冲区满与背压 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 11 | [Week3/Day1](Week3/Day1/README.md) | 广播消费 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 12 | [Week3/Day2](Week3/Day2/README.md) | 串行处理流水线 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 13 | [Week3/Day3](Week3/Day3/README.md) | 菱形依赖 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 14 | [Week3/Day4](Week3/Day4/README.md) | 多生产者 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 15 | [Week3/Day5](Week3/Day5/README.md) | 异常策略与正常关闭 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 16 | [Week4/Day1](Week4/Day1/README.md) | RingBuffer 与 Sequence 源码 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 17 | [Week4/Day2](Week4/Day2/README.md) | 单生产者序号协调 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 18 | [Week4/Day3](Week4/Day3/README.md) | 多生产者序号协调 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 19 | [Week4/Day4](Week4/Day4/README.md) | 消费者处理循环 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 20 | [Week4/Day5](Week4/Day5/README.md) | 等待策略 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 21 | [Week5/Day1](Week5/Day1/README.md) | 整理完整订单项目 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) |
| 22 | [Week5/Day2](Week5/Day2/README.md) | Channel 对照实现 | 新建：[Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) |
| 23 | [Week5/Day3](Week5/Day3/README.md) | 吞吐量与分配测量 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj); [Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) |
| 24 | [Week5/Day4](Week5/Day4/README.md) | 延迟、容量与突发流量 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj); [Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) |
| 25 | [Week5/Day5](Week5/Day5/README.md) | 复盘与选型报告 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj); [Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) |
