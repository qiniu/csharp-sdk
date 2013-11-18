## CHANGE LOG

### v6.0.2

2013-11-18 #issue[38](https://github.com/qiniu/csharp-sdk/pull/38)

- 向PutPolicy类中添加pfop等有关的字段,添加callbody格式验证，fixed returnUrl，callbackUrl不能同时指定的特性，fixed returnUrl,returnBody需要同时指定或不指定特性。添加putPolicy 的JSON字符串的单元测试

### v6.0.1

2013-09-28 #issue[28](https://github.com/qiniu/csharp-sdk/pull/28)

- 修复Qiniu.RS.Entry实例化时的逻辑判断错误
- 修复断点续传时会自动对key做urlEncode导致key不一致的错误

### v6.0.0

2013-07-30 #issue[17](https://github.com/qiniu/csharp-sdk/pull/17)

- 开发环境支持Xamarin,Mono
- 增加tools,包含Json库，nunit.framework.dll
- 移植单元测试,由原VisulStudio支持变换为Nunit Framework支持
- QBox命名空间更新为Qiniu
- 遵循 [Qiniu API SDKSpec 6.0](https://github.com/qiniu/sdkspec/tree/v6.0.0)
- 增加 RSF 支持：增加 Qiniu.RSF.RSFClient 类
- 增加 RS批量操作
- 引用第三方JSON库,[JSON.NET](json.codeplex.com)
- 大文件断点并行上传支持: 增加 Qiniu.IO.Resumable.ResumablePut 类
