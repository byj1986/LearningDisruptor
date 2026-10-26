# Week1/Day5（全程第5天）：环形缓冲区与基础测量

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

今天有新建空项目。

- [Fundamentals.RingBufferLab](Fundamentals.RingBufferLab.csproj)
- [Fundamentals.ChannelLab](../Day4/Fundamentals.ChannelLab.csproj)

在本日目录运行：

```powershell
dotnet run --project "Fundamentals.RingBufferLab.csproj"
dotnet run --project "../Day4/Fundamentals.ChannelLab.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

LMAX指南的核心概念；复习数组、序号、Stopwatch。

[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 环形数组、读写序号、取模、满与空。
- 逻辑序号与数组下标不同；明确计时起止位置。

## 编码与分析任务

- [ ] 手写容量8的单线程环形缓冲区，实现 TryWrite、TryRead。
- [ ] 交替写入与读取至少100条数据，覆盖多次回绕，验证满和空。
- [ ] 回到第4天的 Channel 项目增加总耗时与已消费数量统计。

## 验收

- [ ] 满时写入失败，空时读取失败，多次回绕后数据正确。
- [ ] 能画出读写位置；不会把生产者结束误当作全部消费完成。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
