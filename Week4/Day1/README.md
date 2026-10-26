# Week4/Day1（全程第16天）：RingBuffer 与 Sequence 源码

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

源码中 RingBuffer、Sequence；书 B 第21章；内存管理书选读。

[书 B](https://www.microsoftpressstore.com/store/clr-via-c-sharp-9780735667457)；[源码](https://github.com/disruptor-net/Disruptor-net)；[内存管理书](https://link.springer.com/book/10.1007/979-8-8688-0453-3)

## 知识点

- 事件预分配、序号读写、缓存行与伪共享的目的。
- 槽位复用不代表业务全过程没有内存分配。

## 编码与分析任务

- [ ] 按所用NuGet版本找到源码对应标签，标注工厂调用、槽位访问与序号读写位置。
- [ ] 统计事件工厂调用次数，观察继续发布是否继续创建槽位事件对象。
- [ ] 列出日志、装箱、字符串及业务逻辑中可能产生分配的位置。

## 验收

- [ ] 能解释框架对象复用与业务零分配的区别。
- [ ] 能把实际现象对应到源码位置，了解Sequence布局意图。

## 学习记录

- [ ] 完成阅读
- [ ] 完成练习
- [ ] 通过验收
- [ ] 能用自己的话解释当天问题

今天完成的程序或实验：

实际结果：

我能解释的一个“为什么”：

尚未解决的问题：
