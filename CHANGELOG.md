## CHANGE LOG


### v6.1.1
2014-02-25 issue [#41](https://github.com/qiniu/csharp-sdk/pull/41)

- 简化断点续上传，删除bput逻辑
- 增加断点续上传中块上传结果本地缓存特性
- 修复bug：>2.5GB文件上传失败

### v6.1.0

2014-02-18 issue [#37](https://github.com/qiniu/csharp-sdk/pull/37)

- 添加pfop请求(DO)及状态查询接口(QueryPfopStatus)
- 添加NET2.0、NET3.5、NET4.0支持
- 向PutPolicy类中添加pfop等有关的字段,添加callbody格式验证，fixed returnUrl，callbackUrl不能同时指定的特性，fixed returnUrl,returnBody需要同时指定或不指定特性。添加putPolicy 的JSON字符串的单元测试
- 修复Qiniu.RS.Entry实例化时的逻辑判断错误
- 修复断点续传时会自动对key做urlEncode导致key不一致的错误

### v6.0.0

2013-07-30 issue [#17](https://github.com/qiniu/csharp-sdk/pull/17)

- 开发环境支持Xamarin,Mono
- 增加tools,包含Json库，nunit.framework.dll
- 移植单元测试,由原VisulStudio支持变换为Nunit Framework支持
- QBox命名空间更新为Qiniu
- 遵循 [Qiniu API SDKSpec 6.0](https://github.com/qiniu/sdkspec/tree/v6.0.0)
- 增加 RSF 支持：增加 Qiniu.RSF.RSFClient 类
- 增加 RS批量操作
- 引用第三方JSON库,[JSON.NET](json.codeplex.com)
- 大文件断点并行上传支持: 增加 Qiniu.IO.Resumable.ResumablePut 类
