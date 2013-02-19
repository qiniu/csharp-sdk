## CHANGE LOG

### v2.4.0

2013-02-19

Issue [#10](https://github.com/qiniu/csharp-sdk/pull/10):

- QBox.Auth.AuthPolicy 增加成员：CallbackBodyType, Escape, AsyncOps, ReturnBody
- DownloadToken支持：增加 QBox.Auth.DownloadPolicy 类
- 增加 PutAuth 支持：增加 QBox.Auth.PutAuthClient 类
- 非兼容调整：QBox.RS.Client 改名为 QBox.RPC.Client
- 简易断点续上传支持: 增加 QBox.RS.ResumablePut 类
- hotfix: 修复了 Base64 编码不支持中文的情况（QBox/Util）

