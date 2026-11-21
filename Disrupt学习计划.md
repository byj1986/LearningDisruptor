# C# Disruptor 每日学习计划（总纲）

共25个学习日：5天补基础 + 20天Disruptor。每周5个学习日，约5周；每个学习单元约120分钟，可拆成两次60分钟。

默认已掌握C#基本语法、类、接口、泛型和委托。每天按准备预测10分钟、定向阅读20分钟、最小实验20分钟、扩展实现25分钟、边界对照30分钟、验收复盘15分钟推进。以验收结果推进，未通过就用下一学习日补齐。

本文件为**全局总纲与索引**；为避免多处维护导致文案或数值漂移，每日完整120分钟执行步骤、阅读表、3组验证场景、排错提示、验收清单、自测题及空白实验表均统一以对应 `WeekM/DayN/README.md` 为**单一事实源**。`WeekM/DayN` 的 `DayN` 是当周第1～5天，下文同时标注全程第1～25天。

## 环境与阅读资料

- 空项目目标框架均为 `net10.0`，使用本机已安装的 .NET 10+ SDK 构建（当前仓库未配置 `global.json`，只要本机装有对应或更高版本 SDK 与 `net10.0` 运行时即可构建运行）。
- `Disruptor.OrderPipeline` 项目已声明 `Disruptor 6.0.1` 依赖。首次构建需要 NuGet 还原；源码与 API 一律以 6.0.1 为准。
- 所有 `Program.cs` 初始仅有空 `Main` 入口和提示注释；不包含任何练习实现、订单模型或处理器。
- 核心阅读采用免费官方文章：[全部25天阅读索引、章节对照及复习笔记](阅读资料.md)。各日 README 也直接给出当天结构化阅读表。
- 选读：[《Pro .NET Memory Management》第2版](https://link.springer.com/book/10.1007/979-8-8688-0453-3)，用于性能和内存进阶。

## 贯穿项目与统一业务规格

模拟订单生成、校验、费用计算与汇总，先用内存计算和统计。订单字段由你手写，可逐步增加订单编号、金额、有效标志、费用、生产者编号、生产者局部编号及测量时间戳。金额与费用统一使用整数分，两个实现保持一致。

第12、13天以及第21～24天跨实现对照统一使用两套明确规格，避免跨天漂移：

- **正常全有效对照规格**（第22～24天吞吐/延迟主对照）：订单编号 `0 .. N-1`，每条金额 `100` 分、有效费用 `10` 分（例如 `N = 100000` 时编号和 `4999950000`、总金额 `10000000` 分、总费用 `1000000` 分）。
- **含无效订单校验规格**（第12、13天及第21、22天无效样本对照）：订单编号能被 `10` 整除则判定无效（费用清零为 `0`），其余为有效订单（费用 `10` 分；例如 `0..19` 为有效 `18`、无效 `2`、费用 `180` 分；`0..999` 为有效 `900`、无效 `100`、有效金额 `90000` 分、费用 `9000` 分）。

## 项目与复用安排

| 新建位置 | 项目 | 后续使用 |
| --- | --- | --- |
| Week1/Day1 | Fundamentals.ConcurrencyLab | 全程第1～3天 |
| Week1/Day4 | Fundamentals.ChannelLab | 全程第4天；第5、22天回看或补充 |
| Week1/Day5 | Fundamentals.RingBufferLab | 全程第5天 |
| Week2/Day1 | Disruptor.OrderPipeline | 全程第6～21天及第23～25天 |
| Week5/Day2 | Comparison.ChannelPipeline | 全程第22～25天 |

使用根目录 `LearningDisruptor.sln` 打开全部项目。续用日没有 `csproj`，按当天 README 指向已有项目；自行保留各次实验记录或版本。

## 如何使用这份计划

每天先打开对应 `WeekM/DayN/README.md`，核对项目位置与先修要求，再按6个时间段完成实验：

- 数值是用于核对的理论预期，不是已执行的结果；实际输出由你手写代码、运行后填写。
- 同一项目按日续写，为每天的方法或实验模式单独命名；已有练习、统计与记录不要相互覆盖。
- 主控等待要有超时；故意暂停、故障或制造发布缺口时，`finally` 负责释放控制信号并回收工作流程。超时不自动取消仍在运行的操作。
- 每日记录至少保留：实际命令、输入、配置、预期、结果、解释和未解决问题。只有阅读完成而实验未通过，保留“待验收”。
- 核心任务完成再做选做。120分钟不够时拆成两次，按学习单元而非自然日推进。
- 正式性能轮使用 Release，不附加调试器；正确性轮可以保留详细校验。先验证语义一致，再比较吞吐量和延迟。

## 对照源码与 Disruptor 6.0.1 关键备忘

本仓库依赖 `Disruptor 6.0.1`。根据该 NuGet 包的 repository 元数据，对应源码提交为 `912e2334d91e336209dfb34b26b7d0a24d30a6ae`。第4周及日常 API 查验一律以[这份 6.0.1 源码](https://github.com/disruptor-net/Disruptor-net/tree/912e2334d91e336209dfb34b26b7d0a24d30a6ae)为准；Wiki 中涉及 7.x 的较新接口（如 `ISequenceWaiter`）不直接套用，6.0.1 直接使用 `IWaitStrategy.WaitFor`。

以下几个 6.0.1 关键行为务必在编码与源码阅读前牢记：

1. **默认 `ProducerType` 为 `Multi`**：`Disruptor<T>` 的双参/三参构造默认使用 `SequencerFactory.DefaultProducerType`（`ProducerType.Multi`）和 `BlockingWaitStrategy`。第6～13天单生产者实验及第23～24天单写对比必须使用 5 参构造显式指定 `ProducerType.Single`。
2. **发布 API 归属区分**：`PublishEvent()` / `PublishEvents(int)` 在 `Disruptor<T>` 和 `RingBuffer<T>` 上均有提供；而 `Next()` / `Next(int)` / `Publish(...)` / `TryNext(...)` / `TryPublishEvent()` / `TryPublishEvents(int)` **仅在 `RingBuffer<T>` 上提供**。此外 `RingBuffer.Next(n)` 返回该批的上界 `hi`（起始序号 `lo = hi - n + 1`）。
3. **`Start()` 返回值区分**：`Disruptor<T>.Start()` 返回 `RingBuffer<T>`（不是 `Task`）；返回执行 `Task` 的是底层 `IEventProcessor.Start(TaskScheduler, TaskCreationOptions)`。
4. **事件处理器源码类名区分**：主线 `IEventHandler<T>`（带 `endOfBatch`）在 6.0.1 中由 `EventProcessorFactory` 创建 **`EventProcessor<T, ...>`** 执行；源码中名为 **`BatchEventProcessor<T, ...>`** 的类服务于整批回调接口 `IBatchEventHandler<T>`（`OnBatch(EventBatch<T>)`）。

---

## Week1：补齐并发与队列基础

**本周重点**：优先保证等待可结束、共享状态正确、内存发布协议清晰、有界队列消息数量可对账。

| 全程日 | 每日入口 | 主题 | 项目 | 核心交付物与关键注意 |
| --- | --- | --- | --- | --- |
| 1 | [Week1/Day1](Week1/Day1/README.md) | 线程、Task 与异步 | 新建：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) | 同步/Thread/Task/Sleep vs Delay 四组实验与“启动/等待/异常接收”对照表 |
| 2 | [Week1/Day2](Week1/Day2/README.md) | 共享状态与原子操作 | 续用：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) | 无保护、`lock`（专用 `Lock` 或共享 `object`）、`Interlocked` 三版本 10 轮结果表与交错示意 |
| 3 | [Week1/Day3](Week1/Day3/README.md) | 内存可见性与线程协作 | 续用：[Fundamentals.ConcurrencyLab](Week1/Day1/Fundamentals.ConcurrencyLab.csproj) | `Volatile` 单次发布、`ManualResetEventSlim` 通知与超时、`CompareExchange` CAS 递增 |
| 4 | [Week1/Day4](Week1/Day4/README.md) | 有界队列、背压与取消 | 新建：[Fundamentals.ChannelLab](Week1/Day4/Fundamentals.ChannelLab.csproj) | 有界 Channel 正常完成（10000条）、满时等待、协作取消三组记录与计数对账 |
| 5 | [Week1/Day5](Week1/Day5/README.md) | 环形缓冲区与基础测量 | 新建：[Fundamentals.RingBufferLab](Week1/Day5/Fundamentals.RingBufferLab.csproj)；回看：[Fundamentals.ChannelLab](Week1/Day4/Fundamentals.ChannelLab.csproj) | 单线程环形数组（容量8，空/满/回绕100条）与生产结束/消费完成两时刻 `Stopwatch` 对照 |

## Week2：掌握 Disruptor 基本流程

**本周重点**：优先保证发布协议、槽位对象所有权与清理、批量边界与背压计数正确。**本周全部实验均显式配置 `ProducerType.Single`**。

| 全程日 | 每日入口 | 主题 | 项目 | 核心交付物与关键注意 |
| --- | --- | --- | --- | --- |
| 6 | [Week2/Day1](Week2/Day1/README.md) | Disruptor 最小示例 | 新建：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 最小订单流程与 0/10/100000 条校验；显式传 `ProducerType.Single` |
| 7 | [Week2/Day2](Week2/Day2/README.md) | 申请、填写、发布 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 显式 `Next/Publish` 与 `PublishEvent()` 作用域对照；取得前拒绝 vs 取得后填充失败对账 |
| 8 | [Week2/Day3](Week2/Day3/README.md) | 序号、回绕与对象复用 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 容量8序号下标表、槽位引用与独立副本对照、可选字段清理规则 |
| 9 | [Week2/Day4](Week2/Day4/README.md) | 批量发布与消费批次 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | `N=100003`、`batch=1/16/64` 尾批校验与消费批次分布；注意 `Next(n)` 返回上界 `hi` |
| 10 | [Week2/Day5](Week2/Day5/README.md) | 缓冲区满与背压 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 容量8满时等待、`RingBuffer.TryNext` / `TryPublishEvent()` 与截止重试业务分类对账 |

## Week3：消费拓扑、多生产者与异常

**本周重点**：掌握广播、串行、菱形三类依赖拓扑，明确多生产者序号语义及异常/关闭退出协议。

| 全程日 | 每日入口 | 主题 | 项目 | 核心交付物与关键注意 |
| --- | --- | --- | --- | --- |
| 11 | [Week3/Day1](Week3/Day1/README.md) | 广播消费 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 广播拓扑（10000条，A/B 各收到 10000 条）、两完成时刻、慢分支容量约束 |
| 12 | [Week3/Day2](Week3/Day2/README.md) | 串行处理流水线 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | A校验→B费用→C汇总（0～999，有效900/无效100，费用9000分）、字段所有权表与单线程对账 |
| 13 | [Week3/Day3](Week3/Day3/README.md) | 菱形依赖 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 并行分支 A/B 汇合到 C、分别暂停 A/B 验证汇合屏障、与串行版对账 |
| 14 | [Week3/Day4](Week3/Day4/README.md) | 多生产者 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 切为显式 `ProducerType.Multi`、双生产者 2×50000 三轮唯一性与局部顺序校验 |
| 15 | [Week3/Day5](Week3/Day5/README.md) | 异常策略与正常关闭 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 继续策略、停止策略、关闭超时（`TimeoutException`）与四条退出路径终态对账 |

## Week4：阅读核心源码

**本周重点**：将运行现象与 6.0.1 源码符号一一对应，避免只背概念或误读其他版本 API。

| 全程日 | 每日入口 | 主题 | 项目 | 核心交付物与关键注意 |
| --- | --- | --- | --- | --- |
| 16 | [Week4/Day1](Week4/Day1/README.md) | RingBuffer 与 Sequence 源码 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 版本提交记录、4个以上源码定位点、工厂计数与业务分配来源对照 |
| 17 | [Week4/Day2](Week4/Day2/README.md) | 单生产者序号协调 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | `SingleProducerSequencer` 边界推演表、独立 gating 模型验证与 `Next→Publish` 流程图 |
| 18 | [Week4/Day3](Week4/Day3/README.md) | 多生产者序号协调 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | `MultiProducerSequencer` 双生产者发布缺口时间线与 `GetHighestPublishedSequence` 源码对应 |
| 19 | [Week4/Day4](Week4/Day4/README.md) | 消费者处理循环 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 精读 6.0.1 `EventProcessor`（非 `BatchEventProcessor`），输出12行主循环说明与异常/停止分支 |
| 20 | [Week4/Day5](Week4/Day5/README.md) | 等待策略 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | `Blocking` / `Yielding` / `BusySpin` 源码差异表与空闲/低速/突发/依赖等待 CPU 矩阵 |

## Week5：项目整理与性能实验

**本周重点**：统一容量、载荷类型、业务规则、单写配置、等待策略与计时范围，用可复现的原始数据支撑选型结论。

| 全程日 | 每日入口 | 主题 | 项目 | 核心交付物与关键注意 |
| --- | --- | --- | --- | --- |
| 21 | [Week5/Day1](Week5/Day1/README.md) | 整理完整订单项目 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj) | 可配置入口、四命名场景（正常/无效/慢/突发）与固定对照业务规格 |
| 22 | [Week5/Day2](Week5/Day2/README.md) | Channel 对照实现 | 新建：[Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) | 等价单写单读 Channel 实现、正确性对账；明确 Channel 载荷类型（推荐 `readonly record struct` 或与 `class` 分组） |
| 23 | [Week5/Day3](Week5/Day3/README.md) | 吞吐量与分配测量 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj)；[Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) | 2实现×5轮原始数据（预热5秒+测量≥10秒含排空）；锁定 `ProducerType.Single`、具体 `IWaitStrategy` 与载荷类型 |
| 24 | [Week5/Day4](Week5/Day4/README.md) | 延迟、容量与突发流量 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj)；[Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) | 最近秩百分位自检、三负载与容量（1024 vs 16384）对照；Windows 固定速率发压改用 `Stopwatch` 节拍而非 `Sleep`/`Delay` |
| 25 | [Week5/Day5](Week5/Day5/README.md) | 复盘与选型报告 | 续用：[Disruptor.OrderPipeline](Week2/Day1/Disruptor.OrderPipeline.csproj)；[Comparison.ChannelPipeline](Week5/Day2/Comparison.ChannelPipeline.csproj) | 五类关键场景回归、性能复现校验、一页选型结论与可独立复现文档 |

---

## 每日记录与推进规则

每天在对应 `WeekM/DayN/README.md` 底部记录：完成的程序或实验、实际结果、能解释的一个为什么、尚未解决的问题。每周预留两天休息或补练。

- **第1周**：优先保证等待可结束、共享状态正确、消息数量可对账。
- **第2～3周**：优先保证发布协议、事件所有权、依赖与关闭流程正确。
- **第4周**：把运行现象与所用 6.0.1 版本源码对应起来，避免只背术语。
- **第5周**：统一容量、载荷类型、业务逻辑、投递语义、生产者模式、等待策略与计时范围，保留原始数据。

性能测量使用 Release、关闭调试器、先预热、重复运行，移除逐条输出。跨线程实验使用进程级分配统计，避免把当前线程数据误当全程数据。P99 等百分位数说明采样数量、测量边界与负载（含发送滞后），不仅记录单次最好成绩。

## 参考资料

- [Disruptor-net 入门（Getting Started）](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)
- [Disruptor-net 发布与背压（Publication Options）](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)
- [Disruptor-net 事件处理器（Event Handlers）](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)
- [Disruptor-net 等待策略（Wait Strategies）](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)
- [Disruptor-net 6.0.1 固定源码提交](https://github.com/disruptor-net/Disruptor-net/tree/912e2334d91e336209dfb34b26b7d0a24d30a6ae)
- [LMAX Disruptor 官方指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)
- [微软文档：同步基元概述](https://learn.microsoft.com/zh-cn/dotnet/standard/threading/overview-of-synchronization-primitives)
- [微软文档：Interlocked 类](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.interlocked?view=net-10.0)
- [微软文档：Volatile 类](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.volatile?view=net-10.0)
- [微软文档：System.Threading.Channels 库](https://learn.microsoft.com/zh-cn/dotnet/core/extensions/channels)
- [可选进阶书：Pro .NET Memory Management（第2版）](https://link.springer.com/book/10.1007/979-8-8688-0453-3)
