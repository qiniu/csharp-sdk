## CHANGE LOG

### v6.0.0

2013-07-30

- 开发环境支持Xamarin,Mono
- 移植单元测试,由原VisulStudio支持变换为Nunit Framework支持


2013-07-3

- QBox命名空间更新为Qiniu
- 遵循 [Qiniu API SDKSpec 6.0](https://github.com/qiniu/sdkspec/tree/v6.0.0)
- 增加 RSF 支持：增加 Qiniu.RSF.RSFClient 类
- 增加 RS批量操作
- 引用第三方JSON库,[JSON.NET](json.codeplex.com)
- 大文件断点并行上传支持: 增加 Qiniu.IO.Resumable.ResumablePut 类
