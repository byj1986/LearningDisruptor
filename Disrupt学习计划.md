# C# Disruptor 每日学习计划

共25个学习日：5天补基础 + 20天Disruptor。每周5个学习日，约5周；每天90～120分钟。

默认已掌握C#基本语法、类、接口、泛型和委托。每日安排为20～30分钟阅读、55～75分钟编码、15分钟复盘。以验收结果推进，未通过就用下一学习日补齐。

本计划与当前 LearningDisruptor 仓库配套。WeekM/DayN 的 DayN 是当周第1～5天，正文同时标注全程第1～25天。25个每日目录均有README；可以继续前一天项目时不再新建。

## 环境与书籍

- 本机检测到.NET SDK 10.0.402，空项目目标为net10.0；global.json按已安装SDK固定版本。
- Disruptor订单项目已声明Disruptor 6.0.1依赖。首次构建需要NuGet还原；以对应版本源码和API为准。
- 所有Program.cs仅有空Main入口和提示注释；不包含任何练习实现、订单模型或处理器。
- 书 A：[《C#并发编程经典实例》第2版](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)，Stephen Cleary。
- 书 B：[《CLR via C#》第4版](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)，Jeffrey Richter。重点读线程和同步；运行时细节结合当前.NET文档。
- 选读：[《Pro .NET Memory Management》第2版](https://link.springer.com/book/10.1007/979-8-8688-0453-3)，用于性能和内存进阶。

## 贯穿项目

模拟订单生成、校验、费用计算与汇总。先用内存计算和统计。订单字段由你手写，可逐步增加订单编号、金额、有效标志、费用、生产者编号、生产者局部编号及测量时间戳。金额可用整数分，两个实现保持一致。

## 项目与复用安排

| 新建位置 | 项目 | 后续使用 |
| --- | --- | --- |
| Week1/Day1 | Fundamentals.ConcurrencyLab | 全程第1～3天 |
| Week1/Day4 | Fundamentals.ChannelLab | 全程第4天；第5、22天回看或补充 |
| Week1/Day5 | Fundamentals.RingBufferLab | 全程第5天 |
| Week2/Day1 | Disruptor.OrderPipeline | 全程第6～21天及第23～25天 |
| Week5/Day2 | Comparison.ChannelPipeline | 全程第22～25天 |

使用根目录LearningDisruptor.sln打开全部项目。续用日没有csproj，按当天README指向已有项目；自行保留各次实验记录或版本。


## Week1：补齐并发与队列基础


### Day1（全程第1天）：线程、Task 与异步

**项目安排：**新建空项目；`Week1/Day1/Fundamentals.ConcurrencyLab.csproj`。

**阅读：**书 A 第1章、第2章等待任务与异常处理；书 B 第26章。 [书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)

**知识点：**

- 并发、并行、异步的区别。
- Thread、线程池、Task、await 的关系；Join 与等待任务。

**编码与分析：**

- [ ] 分别用 Thread、Task.Run 执行计算工作，等待全部工作完成。
- [ ] 比较 Thread.Sleep 与 Task.Delay，记录线程编号与等待期间的行为。
- [ ] 观察异步方法在 await 前后的执行位置，不把一次观察当作线程调度保证。

**验收：**

- [ ] 能解释 Task 是否等于线程、await 是否必然创建线程。
- [ ] 能区分阻塞与异步等待；程序不提前退出。


### Day2（全程第2天）：共享状态与原子操作

**项目安排：**继续已有项目；`Week1/Day1/Fundamentals.ConcurrencyLab.csproj`。

**阅读：**书 A 第12章阻塞锁；书 B 第29、30章相关部分；同步原语、Interlocked。 [书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)；[同步原语](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)

**知识点：**

- 竞争条件、临界区、互斥与原子操作。
- ++ 包含读取、计算、写回多个步骤。

**编码与分析：**

- [ ] 创建4个任务，每个任务对共享计数器递增10万次。
- [ ] 分别实现普通 ++、lock、Interlocked.Increment 三个版本并重复运行。
- [ ] 记录预期值40万、实际值与粗略耗时，先验证正确性。

**验收：**

- [ ] 两个正确同步版本均得到40万。
- [ ] 能解释普通版本偶尔正确为何不能证明线程安全。


### Day3（全程第3天）：内存可见性与线程协作

**项目安排：**继续已有项目；`Week1/Day1/Fundamentals.ConcurrencyLab.csproj`。

**阅读：**书 B 第29章；Volatile、Interlocked、同步原语。 [书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[同步原语](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)；[Volatile](https://learn.microsoft.com/en-us/dotnet/api/system.threading.volatile?view=net-10.0)

**知识点：**

- 原子性、可见性、操作顺序的区别。
- Volatile.Read/Write、CompareExchange；信号通知与超时。

**编码与分析：**

- [ ] 实现一条线程写数据后发布就绪标志，另一条线程观察标志后读取数据，使用 Volatile.Read/Write。
- [ ] 用 ManualResetEventSlim 实现同样的完成通知，并设置超时和退出条件。
- [ ] 尝试用 CompareExchange 循环实现共享计数器，观察失败重试。

**验收：**

- [ ] 能解释先写数据、后发布标志的意义。
- [ ] 知道 volatile 不能让 count++ 成为原子操作；所有等待均可结束。


### Day4（全程第4天）：有界队列、背压与取消

**项目安排：**新建空项目；`Week1/Day4/Fundamentals.ChannelLab.csproj`。

**阅读：**书 A 第9章队列、第10章取消；Channels 文档。 [书 A](https://www.oreilly.com.cn/index.php?func=book&isbn=978-7-115-55060-6)；[Channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)

**知识点：**

- 生产速度、消费速度、容量与背压。
- 正常完成写入与主动取消处理的区别。

**编码与分析：**

- [ ] 建立容量64、BoundedChannelFullMode.Wait 的 Channel<T>，发布1万条编号消息。
- [ ] 让消费者偶尔延迟，观察 WriteAsync 等待；异步等待不等于阻塞线程。
- [ ] 分别实现正常完成和主动取消，记录已接受、已处理、未完成数量。

**验收：**

- [ ] 正常完成时1万条全部消费。
- [ ] 能说明满时行为、完成与取消的区别，以及取消后未处理数据如何统计。


### Day5（全程第5天）：环形缓冲区与基础测量

**项目安排：**新建空项目；`Week1/Day5/Fundamentals.RingBufferLab.csproj`; `Week1/Day4/Fundamentals.ChannelLab.csproj`。

**阅读：**LMAX指南的核心概念；复习数组、序号、Stopwatch。 [LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 环形数组、读写序号、取模、满与空。
- 逻辑序号与数组下标不同；明确计时起止位置。

**编码与分析：**

- [ ] 手写容量8的单线程环形缓冲区，实现 TryWrite、TryRead。
- [ ] 交替写入与读取至少100条数据，覆盖多次回绕，验证满和空。
- [ ] 回到第4天的 Channel 项目增加总耗时与已消费数量统计。

**验收：**

- [ ] 满时写入失败，空时读取失败，多次回绕后数据正确。
- [ ] 能画出读写位置；不会把生产者结束误当作全部消费完成。


## Week2：掌握Disruptor基本流程


### Day1（全程第6天）：Disruptor 最小示例

**项目安排：**新建空项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**入门、源码 README。 [入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[源码](https://github.com/disruptor-net/Disruptor-net)

**知识点：**

- Disruptor<T>、RingBuffer、事件工厂、IEventHandler<T>。
- 配置、启动、发布、等待处理、关闭的生命周期。

**编码与分析：**

- [ ] 手写 OrderEvent（订单编号、金额）与一个处理器，配置容量1024的 Disruptor。
- [ ] 先发布10条观察流程与线程，再发布10万条并计数。
- [ ] 确认全部处理完成后关闭；不使用任意固定休眠猜测完成。

**验收：**

- [ ] 发布数与消费数一致。
- [ ] 能指出对象创建、填写和消费位置，以及它们对应的线程。


### Day2（全程第7天）：申请、填写、发布

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**发布与背压。 [发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)

**知识点：**

- 申请序号、填充槽位、发布序号的协议。
- 发布作用域退出会发布，不等于事务回滚。

**编码与分析：**

- [ ] 分别手写发布作用域和显式 Next/Publish 两种方式。
- [ ] 发布前校验输入；取得槽位后先置无效，完整填写后再置有效。
- [ ] 模拟填写异常，完成发布协议并使消费者识别无效事件。

**验收：**

- [ ] 知道申请后为什么必须完成发布协议。
- [ ] 异常输入不会被计入正常订单，不留下阻塞后续消费的发布缺口。


### Day3（全程第8天）：序号、回绕与对象复用

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**入门、LMAX指南。 [入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 2的幂容量、序号到槽位的映射。
- 预分配槽位对象反复承载不同事件。

**编码与分析：**

- [ ] 把容量改为8，发布100条并观察槽位对象复用。
- [ ] 先保存事件引用，再改为保存数据副本，观察后续回绕后的差异。
- [ ] 检查每次发布是否覆盖所有相关字段，避免上一轮状态遗留。

**验收：**

- [ ] 能解释为何消费结束后长期持有槽位引用可能读到后来的数据。
- [ ] 序号、下标和业务编号各自含义明确。


### Day4（全程第9天）：批量发布与消费批次

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**发布与背压的 Batches；事件处理器。 [发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)；[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)

**知识点：**

- 批量申请、填充、发布。
- endOfBatch 是当前可消费批次的结束，不保证与发布批次一一对应。

**编码与分析：**

- [ ] 用单条、16条、64条批量发布相同数量订单。
- [ ] 记录消费批次大小，在消费批次结束时更新汇总。
- [ ] 比较各种方式的数量与金额总和，并覆盖最后不足一批的情况。

**验收：**

- [ ] 各种发布方式业务结果一致。
- [ ] 能区分生产者批次和消费者批次。


### Day5（全程第10天）：缓冲区满与背压

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**发布与背压的 Backpressure、Alternatives to Next。 [发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)

**知识点：**

- 等待式申请、尝试发布、超时或拒绝。
- 持续处理能力不足时增加容量只会延缓满。

**编码与分析：**

- [ ] 使用小容量，故意暂停或放慢消费者，观察生产者等待。
- [ ] 实现尝试发布失败后的重试、超时或拒绝，并分类计数。
- [ ] 恢复消费者，确认已接受事件继续处理；为受控实验设置超时。

**验收：**

- [ ] 尝试数、成功数与拒绝数能对账。
- [ ] 能说明扩容与提高消费能力分别解决什么问题。


## Week3：消费拓扑、多生产者与异常


### Day1（全程第11天）：广播消费

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**事件处理器；LMAX指南的 Multicast Events。 [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 多个普通事件处理器分别接收同一个事件。
- 广播与队列竞争消费的区别；共享事件的写入所有权。

**编码与分析：**

- [ ] 配置两个并行处理器，一个统计订单数量，一个统计金额。
- [ ] 处理器只读事件，并各自维护自己的统计状态。
- [ ] 发布10万条并单独检查两个处理器的计数。

**验收：**

- [ ] 两个处理器都收到10万条，而不是各5万条。
- [ ] 能说明并行写同一字段为什么需要额外协调。


### Day2（全程第12天）：串行处理流水线

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**事件处理器的依赖配置；LMAX指南消费者依赖图。 [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 前置阶段处理完成后，下游才能处理对应事件。
- 校验、计算、汇总各自的字段所有权。

**编码与分析：**

- [ ] 扩展为校验 → 费用计算 → 汇总。
- [ ] 校验填写有效标志，计算填写费用，汇总区分正常与无效订单。
- [ ] 写一个单线程参考流程核对最终结果。

**验收：**

- [ ] 流水线结果与单线程参考一致。
- [ ] 无效订单规则明确，下游不会使用未设置或遗留的字段。


### Day3（全程第13天）：菱形依赖

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**事件处理器；LMAX指南消费者依赖图。 [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 并行分支与汇合；下游等待所有前置阶段。
- 最慢前置阶段如何限制汇合阶段的进度。

**编码与分析：**

- [ ] 实现 A校验、B独立计算费用 → C汇总，A/B分别写自己的字段。
- [ ] 故意放慢A，再放慢B，观察C的处理进度。
- [ ] 画出依赖图并标注各处理器持有的进度。

**验收：**

- [ ] C处理某订单时A/B均已完成该订单。
- [ ] 能说明哪些工作可以并行，以及共享写入的边界。


### Day4（全程第14天）：多生产者

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**入门、源码中生产者配置；LMAX指南。 [入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[源码](https://github.com/disruptor-net/Disruptor-net)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 单生产者与多生产者配置。
- 业务编号、生产者局部编号、RingBuffer序号之间的区别。

**编码与分析：**

- [ ] 使用两个生产者，各发布5万条订单，并选择多生产者配置。
- [ ] 事件增加 ProducerId 与生产者内部编号。
- [ ] 检查唯一性、遗漏、重复及每个生产者自身的发布顺序。

**验收：**

- [ ] 总计10万条，无遗漏或重复。
- [ ] 不会使用线程启动时间或业务编号推断跨生产者全局顺序。


### Day5（全程第15天）：异常策略与正常关闭

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**事件处理器的异常与生命周期；对应版本关闭API。 [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)

**知识点：**

- 记录后继续与停止处理两种异常策略。
- 停止接收、等待生产者、排空、关闭，以及超时。

**编码与分析：**

- [ ] 先在单消费者场景注入10个业务错误，分别试验继续和停止。
- [ ] 实现停止接收 → 等待生产者完成 → 等待已发布事件处理 → 关闭。
- [ ] 故障时带超时退出，并报告未完成事件与原因。

**验收：**

- [ ] 继续处理时成功数 + 失败数 = 已接受数。
- [ ] 停止处理时能够报告未完成数量，不无限等待已故障消费者。


## Week4：阅读核心源码


### Day1（全程第16天）：RingBuffer 与 Sequence 源码

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**源码中 RingBuffer、Sequence；书 B 第21章；内存管理书选读。 [书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[源码](https://github.com/disruptor-net/Disruptor-net)；[内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

**知识点：**

- 事件预分配、序号读写、缓存行与伪共享的目的。
- 槽位复用不代表业务全过程没有内存分配。

**编码与分析：**

- [ ] 按所用NuGet版本找到源码对应标签，标注工厂调用、槽位访问与序号读写位置。
- [ ] 统计事件工厂调用次数，观察继续发布是否继续创建槽位事件对象。
- [ ] 列出日志、装箱、字符串及业务逻辑中可能产生分配的位置。

**验收：**

- [ ] 能解释框架对象复用与业务零分配的区别。
- [ ] 能把实际现象对应到源码位置，了解Sequence布局意图。


### Day2（全程第17天）：单生产者序号协调

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**源码中的单生产者协调器；LMAX指南。 [源码](https://github.com/disruptor-net/Disruptor-net)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

**知识点：**

- 申请位置、回绕检查、末端消费者进度约束。
- 消费者未释放的槽位不能被覆盖。

**编码与分析：**

- [ ] 阅读申请、容量检查、发布相关分支，写简短注释。
- [ ] 推演容量8、末端消费者已处理到3、生产者已申请到11的状态。
- [ ] 解释为何此时不能申请12，以及消费者推进后何时恢复。

**验收：**

- [ ] 能用序号与容量判断覆盖风险。
- [ ] 能在源码中找到相应检查，而不是只背结论。


### Day3（全程第18天）：多生产者序号协调

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**源码中的多生产者协调器；Interlocked。 [源码](https://github.com/disruptor-net/Disruptor-net)；[Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)

**知识点：**

- CAS争用、发布可用性与连续可消费范围。
- 已申请、已发布、可连续读取三个状态不同。

**编码与分析：**

- [ ] 让A申请较早序号后暂缓发布，B先发布后续序号，再恢复A。
- [ ] 用信号控制实验时序，设置超时，避免无控制的随机休眠。
- [ ] 记录消费者进度并找到源码中处理发布缺口的位置。

**验收：**

- [ ] 能解释消费者为何不能越过未发布缺口。
- [ ] 能说明多生产者比单生产者额外协调了什么。


### Day4（全程第19天）：消费者处理循环

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**源码中批量事件处理器；事件处理器文档。 [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[源码](https://github.com/disruptor-net/Disruptor-net)

**知识点：**

- 等待、批量遍历、回调、进度更新、异常与退出。
- 批次处理与消费者进度推进的时机。

**编码与分析：**

- [ ] 给消费主循环写注释，并画出一次批量消费的步骤。
- [ ] 对照第9、12、15天实验定位批次、依赖和异常处理位置。
- [ ] 记录正常停止与故障停止经过的路径。

**验收：**

- [ ] 能解释消费者何时推进进度。
- [ ] 能说明慢处理器为何限制槽位复用，停止时如何结束等待。


### Day5（全程第20天）：等待策略

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**等待策略文档与对应版本实现；书 B 线程调度相关内容。 [书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)

**知识点：**

- 阻塞、让出线程、自旋；等待新事件与等待依赖阶段。
- CPU、核心数量、系统负载与延迟的权衡。

**编码与分析：**

- [ ] 在同一程序中切换 BlockingWaitStrategy、YieldingWaitStrategy、BusySpinWaitStrategy。
- [ ] 观察空闲、持续负载、突发负载时的CPU和响应。
- [ ] 记录硬件、运行配置和三种策略的初步对照表。

**验收：**

- [ ] 能同时解释CPU成本与延迟表现。
- [ ] 不把某次测量更快等同于所有场景都更合适。


## Week5：项目整理与性能实验


### Day1（全程第21天）：整理完整订单项目

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`。

**阅读：**复习入门、发布与背压、事件处理器、等待策略。 [入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)；[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)

**知识点：**

- 事件所有权、拓扑、背压、关闭与配置组合。
- 用一致输入与单线程参考核对复杂流水线。

**编码与分析：**

- [ ] 继续已有订单项目，将数量、容量、生产者数、拓扑、等待策略改为可配置。
- [ ] 整理正常、无效订单、慢消费者、突发输入四种场景。
- [ ] 确保从同一个程序入口可以重复运行各场景。

**验收：**

- [ ] 正常场景结果可与单线程流程核对。
- [ ] 异常和过载有明确统计；不额外复制一套相同项目。


### Day2（全程第22天）：Channel 对照实现

**项目安排：**新建空项目；`Week5/Day2/Comparison.ChannelPipeline.csproj`。

**阅读：**复习第4天项目与 Channels 文档。 [Channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)

**知识点：**

- 比较对象必须具有相同容量、载荷、业务逻辑和投递语义。
- 满时等待及相同完成条件。

**编码与分析：**

- [ ] 建立Channel订单对照项目，借鉴第4天手写代码。
- [ ] 先实现与Disruptor相同的单生产者/单消费者、相同容量、满时等待。
- [ ] 统一输入和业务计算；若后续比较广播，再补齐相同投递行为。

**验收：**

- [ ] 两个实现业务结果一致。
- [ ] 明确配置差异，不能把广播处理与竞争分摊直接比较。


### Day3（全程第23天）：吞吐量与分配测量

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`; `Week5/Day2/Comparison.ChannelPipeline.csproj`。

**阅读：**内存管理书中的测量与分配章节；复习Stopwatch与GC统计。 [内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

**知识点：**

- 预热、Release、重复测量、进程级统计。
- 计时覆盖最后一条事件处理完成，验证与性能测量分开。

**编码与分析：**

- [ ] 分别扩展已有Disruptor与Channel项目，不新建重复项目。
- [ ] 关闭调试器、移除逐条输出；预热后每组测量至少10秒，重复5轮。
- [ ] 记录已完成事件数、耗时、吞吐量、进程CPU、分配量和GC次数。跨线程实验不要只读当前线程分配量。

**验收：**

- [ ] 保留5轮原始数据，并明确计时起止和预热方式。
- [ ] 不是仅测生产者发布速度；正确性场景事先通过。


### Day4（全程第24天）：延迟、容量与突发流量

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`; `Week5/Day2/Comparison.ChannelPipeline.csproj`。

**阅读：**内存管理书选读；复习等待策略与容量实验。 [等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)；[内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

**知识点：**

- 平均值、P50、P99、最大值；时间戳位置决定延迟含义。
- 排队、背压与发送速率；测量本身也有成本。

**编码与分析：**

- [ ] 用 Stopwatch.GetTimestamp 测量尝试发布前 → 最终处理完成的延迟。
- [ ] 预分配测量存储，单独运行延迟实验，记录时间戳开销与测量边界。
- [ ] 测试低负载、接近饱和、突发输入，记录目标/实际发送速率；改变容量观察排队。

**验收：**

- [ ] 能解释吞吐量相近时P99为何仍有差异。
- [ ] 报告明确是否包含申请槽位等待，说明更大容量对延迟的影响。


### Day5（全程第25天）：复盘与选型报告

**项目安排：**继续已有项目；`Week2/Day1/Disruptor.OrderPipeline.csproj`; `Week5/Day2/Comparison.ChannelPipeline.csproj`。

**阅读：**回顾全计划与实验记录；按问题回查源码和文档。 [源码](https://github.com/disruptor-net/Disruptor-net)

**知识点：**

- 区分实验事实、原因推断和适用边界。
- 记录环境、版本、拓扑、配置和可复现步骤。

**编码与分析：**

- [ ] 在当天README中完成报告：环境、正确性、吞吐量、延迟、CPU、分配与GC。
- [ ] 说明何时选Disruptor、何时选Channel、哪些结论还需验证。
- [ ] 从解决方案重新构建，按记录复现关键实验。

**验收：**

- [ ] 另一位开发者能按说明复现实验。
- [ ] 能用自己的数据说明选型理由，列明限制与未解决问题。


## 每日记录与推进规则

每天记录：完成的程序或实验、实际结果、能解释的一个为什么、尚未解决的问题。每周预留两天休息或补练。

- 第1周：优先保证等待可结束、共享状态正确、消息数量可对账。
- 第2～3周：优先保证发布协议、事件所有权、依赖与关闭流程正确。
- 第4周：把运行现象与所用版本源码对应起来，避免只背术语。
- 第5周：统一容量、载荷、业务逻辑、投递语义与计时范围，保留原始数据。

性能测量使用Release、关闭调试器、先预热、重复运行，移除逐条输出。跨线程实验使用进程级分配统计，避免把当前线程数据误当全程数据。P99等百分位数说明采样数量、测量边界与负载，不仅记录单次最好成绩。

## 参考资料


- [入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)

- [发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)

- [事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)

- [等待策略](https://github.com/disruptor-net/Disruptor-net/wiki/Wait-strategies)

- [源码](https://github.com/disruptor-net/Disruptor-net)

- [LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

- [同步原语](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)

- [Interlocked](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked)

- [Volatile](https://learn.microsoft.com/en-us/dotnet/api/system.threading.volatile?view=net-10.0)

- [Channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)

- [内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)
