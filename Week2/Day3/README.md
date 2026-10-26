# Week2/Day3（全程第8天）：序号、回绕与对象复用

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

入门、LMAX指南。

[入门](https://github.com/disruptor-net/Disruptor-net/wiki/Getting-Started)；[LMAX指南](https://lmax-exchange.github.io/disruptor/user-guide/index.html)

## 知识点

- 2的幂容量、序号到槽位的映射。
- 预分配槽位对象反复承载不同事件。

## 编码与分析任务

- [ ] 把容量改为8，发布100条并观察槽位对象复用。
- [ ] 先保存事件引用，再改为保存数据副本，观察后续回绕后的差异。
- [ ] 检查每次发布是否覆盖所有相关字段，避免上一轮状态遗留。

## 验收

- [ ] 能解释为何消费结束后长期持有槽位引用可能读到后来的数据。
- [ ] 序号、下标和业务编号各自含义明确。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
