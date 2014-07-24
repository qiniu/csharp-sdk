## CHANGE LOG

### v6.1.4
2014-07-24 [#55](https://github.com/qiniu/csharp-sdk/pull/55)
- [#54] 调整上传user agent，更新put policy

### v6.1.3
2014-07-10 [#53](https://github.com/qiniu/csharp-sdk/pull/53)
- [#52] 调整上传host

### v6.1.2
2014-05-30 [#51](https://github.com/qiniu/csharp-sdk/pull/51)
- [#46] nuget 安装文档
- [#48] url key escape for make base url
- [#50] limit args for listprefix

### v6.1.1
2014-04-28 issue [#45](https://github.com/qiniu/csharp-sdk/pull/45)
- [#41] [#42] 简化断点续上传，删除bput逻辑, 修复bug：>2.5GB文件上传失败
- [#38] [#40] pfop 支持

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
