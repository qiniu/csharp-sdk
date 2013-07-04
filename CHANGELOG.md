## CHANGE LOG

### v6.0.0

2013-07-3

Issue [#10](https://github.com/qiniu/csharp-sdk/pull/10):
- QBox命名空间更新为Qiniu
- 遵循 [Qiniu API SDKSpec 6.0](https://github.com/qiniu/sdkspec/tree/v6.0.0)
- 增加 RSF 支持：增加 Qiniu.RSF.RSFClient 类
- 增加 RS批量操作
- 引用方面JSON库,[JSON.NET](json.codeplex.com)
- 大文件断点并行上传支持: 增加 Qiniu.IO.Resumable.ResumablePut 类
- hotfix: 修复了 Base64 编码不支持中文的情况（QBox/Util）

