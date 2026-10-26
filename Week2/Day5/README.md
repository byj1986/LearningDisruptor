# Week2/Day5（全程第10天）：缓冲区满与背压

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

继续已有项目，本目录不重复创建csproj。

- [Disruptor.OrderPipeline](../Day1/Disruptor.OrderPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "../Day1/Disruptor.OrderPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

发布与背压的 Backpressure、Alternatives to Next。

[发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)

## 知识点

- 等待式申请、尝试发布、超时或拒绝。
- 持续处理能力不足时增加容量只会延缓满。

## 编码与分析任务

- [ ] 使用小容量，故意暂停或放慢消费者，观察生产者等待。
- [ ] 实现尝试发布失败后的重试、超时或拒绝，并分类计数。
- [ ] 恢复消费者，确认已接受事件继续处理；为受控实验设置超时。

## 验收

- [ ] 尝试数、成功数与拒绝数能对账。
- [ ] 能说明扩容与提高消费能力分别解决什么问题。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
