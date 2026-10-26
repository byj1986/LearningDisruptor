# Week2/Day2（全程第7天）：申请、填写、发布

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

发布与背压。

[发布与背压](https://github.com/disruptor-net/Disruptor-net/wiki/Publication-Options)

## 知识点

- 申请序号、填充槽位、发布序号的协议。
- 发布作用域退出会发布，不等于事务回滚。

## 编码与分析任务

- [ ] 分别手写发布作用域和显式 Next/Publish 两种方式。
- [ ] 发布前校验输入；取得槽位后先置无效，完整填写后再置有效。
- [ ] 模拟填写异常，完成发布协议并使消费者识别无效事件。

## 验收

- [ ] 知道申请后为什么必须完成发布协议。
- [ ] 异常输入不会被计入正常订单，不留下阻塞后续消费的发布缺口。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
