# Week2/Day1（全程第6天）：Disruptor 最小示例

预计90～120分钟：阅读20～30分钟，编码55～75分钟，复盘15分钟。

## 项目

今天有新建空项目。

- [Disruptor.OrderPipeline](Disruptor.OrderPipeline.csproj)

在本日目录运行：

```powershell
dotnet run --project "Disruptor.OrderPipeline.csproj"
```

入口目前为空，运行不会产生练习结果；由你手写当天代码。

## 阅读

入门、源码 README。

[入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[源码](https://github.com/disruptor-net/Disruptor-net)

## 知识点

- Disruptor<T>、RingBuffer、事件工厂、IEventHandler<T>。
- 配置、启动、发布、等待处理、关闭的生命周期。

## 编码与分析任务

- [ ] 手写 OrderEvent（订单编号、金额）与一个处理器，配置容量1024的 Disruptor。
- [ ] 先发布10条观察流程与线程，再发布10万条并计数。
- [ ] 确认全部处理完成后关闭；不使用任意固定休眠猜测完成。

## 验收

- [ ] 发布数与消费数一致。
- [ ] 能指出对象创建、填写和消费位置，以及它们对应的线程。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
