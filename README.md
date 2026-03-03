# Qiniu (Cloud) C# SDK

## 使用

* 参考文档：[七牛云存储 C# SDK 使用指南](https://developer.qiniu.com/kodo/sdk/1237/csharp)
* 可以参考我们为大家精心准备的使用 [实例](https://github.com/qiniu/csharp-sdk/tree/master/src/QiniuTests)


## 贡献代码

1. Fork

2. 创建您的特性分支 git checkout -b my-new-feature

3. 提交您的改动 git commit -am 'Added some feature'

4. 将您的修改记录提交到远程 git 仓库 git push origin my-new-feature

5. 然后到 github 网站的该 git 远程仓库的 my-new-feature 分支下发起 Pull Request

## 现代化改造经验总结

本项目在现代化改造中，建议持续遵循以下实践：

1. 网络层统一使用 `HttpClient` + `HttpRequestMessage`，避免 `HttpWebRequest`。
2. JSON 处理优先使用 `System.Text.Json`，并尽量配合源生成上下文（`JsonSerializerContext`）提升 AOT 兼容性。
3. 对关键入参增加早期校验（如 `ArgumentNullException.ThrowIfNull` / `ArgumentException.ThrowIfNullOrWhiteSpace`），减少隐式空引用风险。
4. 常用集合初始化时尽量预估容量（如 `new List<T>(count)`、`new Dictionary<TKey, TValue>(capacity)`），减少扩容开销。
5. 字符串拼接优先使用内插字符串（`$"..."`），替代 `string.Format`，可读性更高。
6. 共享序列化配置与工具方法，避免各处重复创建配置和重复实现。

这组实践可作为后续重构的默认基线，在新增功能和修复中持续落地。


## 许可证

Copyright (c) 2017 [qiniu.com](www.qiniu.com)
