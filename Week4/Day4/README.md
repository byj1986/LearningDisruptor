# Week4/Day4（全程第19天）：消费者处理循环

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

源码中批量事件处理器；事件处理器文档。

[事件处理器](https://github.com/disruptor-net/Disruptor-net/wiki/Event-Handlers)；[源码](https://github.com/disruptor-net/Disruptor-net)

## 知识点

- 等待、批量遍历、回调、进度更新、异常与退出。
- 批次处理与消费者进度推进的时机。

## 编码与分析任务

- [ ] 给消费主循环写注释，并画出一次批量消费的步骤。
- [ ] 对照第9、12、15天实验定位批次、依赖和异常处理位置。
- [ ] 记录正常停止与故障停止经过的路径。

## 验收

- [ ] 能解释消费者何时推进进度。
- [ ] 能说明慢处理器为何限制槽位复用，停止时如何结束等待。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
