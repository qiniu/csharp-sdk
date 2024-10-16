**2024-09-05**

v8.7.0

新增：持久化处理，支持工作流模版

v8.6.0

新增：验证 Qbox, Qiniu 签名的辅助方法

新增：持久化处理，支持闲时任务

更改：对象存储，默认空间管理域名，查询区域主备域名

**2024-08-23**

v8.5.1

修复：对象存储，分片并发上传实现非预期

**2023-12-11**

v8.5.0

新增：对象存储，支持归档直读存储

修复：对象存储，批量操作支持自动查询 rs 服务域名

修复：对象存储，上传策略中文件大小、过期时间的属性类型与服务端不一致

**2023-03-28**

v8.4.0

修复：请求消息体可能不正确

新增：查询区域域名过期时间

更改：查询区域域名不再硬编码

更改：查询区域域名后端接口更新

移除：首尔区域移除

更改：部分服务域名由字面量提取为常量

新增：对象存储，更改文件禁用状态

新增：对象存储，获取文件信息时将返回更多信息

新增：对象存储，uc 服务地址可配置

更改：对象存储，查询 bucket 域名更改使用 uc 服务

新增：对象存储，请求中间件逻辑，方便拓展请求逻辑

新增：对象存储，备用 UC 域名用于查询区域域名

修复：对象存储，上传策略缺失 forceSaveKey 字段


**2023-03-06**

v8.3.1

修复：表单直传、分片上传配置 MaxRetryTimes 无效（v7.4.0 引入）

**2022-10-26**

v8.3.0

移除：不推荐域名

增加：亚太-首尔 和 华东-浙江2 固定区域

增加：设置存储对象的生命周期 API

优化：分片上传 ctx 超时检测

修复：部分运行环境 Content-Type 请求头不兼容问题（v8.2.0 引入）

修复：分片上传 v2 不允许没有 key 的问题

**2022-06-9**

v8.2.0

新增：对象存储，管理类 API 发送请求时增加 [X-Qiniu-Date](https://developer.qiniu.com/kodo/3924/common-request-headers) （生成请求的时间） header

* * * 



**2022-04-18**

v8.1.0

新增： 对象存储，新增支持 [深度归档存储类型](https://developer.qiniu.com/kodo/3956/kodo-category#deep_archive)



* * * 

**2021-10-14**

v8.0.0

新增：支持 [分片上传 V2](https://developer.qiniu.com/kodo/6364/multipartupload-interface)，可在上传时指定分片上传版本，参考 [使用手册](https://developer.qiniu.com/kodo/1237/csharp)

优化：更改了底层框架，改用 .Net Standard 2.0，适用于 .NET Core 2.0 和 .NET Framework v4.6.1，详情 [查看官网](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

* * *

**2017-09-15**

最新版本v7.3.0

优化：重构C# SDK的核心代码，扩充详细的使用案例

新增：增加上传策略的新参数fileType等


**2017-03-08**

最新版本v7.2.15，适用于.NET Framework 2.0+

* * *

**2017-03-08**

最新版本v7.2.15，适用于.NET Framework 2.0+ , .NET Core 和 UWP

修复：时间戳防盗链(位于`Qiniu.CDN.Model`)函数中的一个已知问题

优化：JSON序列化/反序列化，弱化Json.NET依赖，可由用户自行定义

* * *

**2017-03-02**

最新版本v7.2.14，适用于.NET Framework 2.0+ , .NET Core 和 UWP

修复：上传策略`PutPolicy`(位于`Qiniu.IO.Model`)补上fsizeMin字段

优化：JSON解析优化，开放JSON解析模块，可由用户自行定义

优化：`Qiniu.CDN`模块中一些优化，如urlSplit等优化

* * *

**2017-02-24**

最新版本v7.2.13，适用于.NET Framework 2.0+ , .NET Core 和 UWP

修复：`Qiniu.CDN.Model`模块下`XXRequest.ctor(IList_string)`函数传入null参数导致的错误

* * *

**2017-02-24**

最新版本v7.2.12，适用于.NET Framework 2.0+ , .NET Core 和 UWP

新增：MD5哈希函数的3rdParty实现(避免FIPS问题)

修复：日期时间戳转换的bug(时间基准设置不对)

* * *

**2017-02-16**

最新版本v7.2.11，适用于.NET Framework 2.0+ , .NET Core 和 UWP

新增：适用于UWP的MSTest单元测试

修复：FormUploader.UploadDataAsync()的已知bug

修复：Qiniu.Util.UrlHelper中URL拆分函数的bug

* * *

**2017-02-15**

最新版本v7.2.10，适用于.NET Framework 2.0+ , .NET Core 和 UWP

移除：移除`Qiniu.IO.SimpleUploader`

优化：部分细节优化

更新：User-Agent格式为`SDK_ALIAS/SDK_VERSION (RUNTIME; OS_DECRIPTION)`

更新：新增DLL文件签名

* * *

**2017-02-13**

最新版本v7.2.9，适用于.NET Framework 2.0+ , .NET Core 和 UWP

新增：单元测试(NUnit)

修复：部分细节问题修复

* * *

**2017-02-08**

最新版本v7.2.8，适用于.NET Framework 2.0+ , .NET Core 和 UWP

修复：FormUploader上传文件出现"file is not specified in multipart"的错误

* * *

**2017-02-06**

最新版本v7.2.7，适用于.NET Framework 2.0+ , .NET Core 和 UWP

新增：对于UWP(Windows10)的支持

新增：对于.NET Framework 4.5及以上, .NET Core和UWP的“异步(async)”支持

更新：部分类、属性、方法名称等细节修改，请参见SDK文档或者使用指南

* * *

**2017-01-20**

最新版本v7.2.6，适用于.NET Framework 2.0+ 和.NET Core

修复：从断点记录恢复时出现错误(Context保存错误)

新增：HTTP状态码，HTTP方法(postPlain等)

* * *

**2017-01-17**

最新版本v7.2.5，适用于.NET Framework 2.0+ 和.NET Core

修复：因不同系统对于HTTP换行处理(CRLF)导致部署到Linux下的程序使用表单上传失败的问题

更新：部分类、属性、方法等名称修改，请参见文档或者指南

* * *

**2017-01-12**

最新版本v7.2.4，适用于.NET Framework 2.0+ 和.NET Core

新增：分片上传(ResumableUploader)的uploadFile拥有更多的重载版本。

新增：上传文件支持用户自定义参数(extraParams)

* * *

**2017-01-10**

最新版本v7.2.3，适用于.NET Framework 2.0+ 和.NET Core

更新：HTTP模块现在可以加入请求Header内容。

* * *

**2017-01-09**

最新版本v7.2.0.0，适用于.NET Framework 2.0+ 和.NET Core

更新：函数命名风格统一，部分细节修改。

* * *

**2017-01-06**

最新版本v7.1.3.0，适用于.NET Framework 2.0+ 和.NET Core

新增：`Qiniu.RS`模块增加`Bucket`功能，用于获取某bucket的属性信息。

修改：.NET Framework 4.0版本Http请求管理器改回HttpWebRequest/HttpWebResponse方式。

* * *

**2017-01-05**

最新版本v7.1.2.0，适用于.NET Framework 2.0+ 和.NET Core

优化项目结构。

* * *

**2017-01-03**

最新版本v7.1.1.0，适用于.NET Framework 2.0+ 和.NET Core

修复部分细节BUG

更新CR.Date至2017

* * *

**2016-12-30**

最新版本v7.1.0.0，适用于.NET Framework 2.0+ 和.NET Core

已发布至NuGet，请搜索`Qiniu.Shared`

* * *

**2016-12-30**

最新版本v7.1.0.0，适用于.NET Framework 2.0+ 和.NET Core

优化：HTTP返回消息(`HttpResult`)的对象复制操作(`Shadow`)中，参考信息(`RefText`)被正确叠加。

* * *

**2016-12-29**

最新版本v7.1.0.0，适用于.NET Framework 2.0+ 和.NET Core

新增：HTTP请求的返回消息(`HttpResult`)中加入参考代码(`RefCode`)和参考信息(`RefInfo`)

新增：断点/分片上传(`ResumableUploader`)加入CRC32校验及重试逻辑(参见`UploadFileAdvanced`的说明)

* * *

**2016-12-27**

最新版本v7.1.0.0，适用于.NET Framework 2.0+ 和.NET Core

* * *

**2016-12-22**

最新版本v7.1.0.0，适用于.NET Framework 4.0+ 和.NET Core

新增：HTTPS支持(CDN/Fusion模块除外)

* * *

**2016-12-14**

最新版本v7.1.0.0，适用于.NET Framework 4.0+ 和.NET Core

新增：Fusion模块中增加HotLink（"时间戳防盗链"功能）

新增：UrlHelper增加URL分析（Host,Path,File,Query分拆）

* * *

**2016-12-12**

最新版本v7.1.0.0，适用于.NET Framework 4.0+ 和.NET Core

新增：增加代码文档

* * *

**2016-12-08**

最新版本v7.1.0.0，适用于.NET Framework 4.0+ 和.NET Core

新增：优化代码易读性

* * *

**2016-12-07**

最新版本v7.1.0.0，适用于.NET Framework 4.0+ 和.NET Core

新特性：.NET Framework 4.0+和.NET Core共用一套代码

新增：更新文件生命周期(BucketManager.UpdateLifecycle)

* * *

**2016-12-01**

最新版本v7.0.0.5

增加：[Fusion](http://developer.qiniu.com/article/index.html#fusion)相关功能

增加： dfop

修复：上传(分片上传)文件发生重试时，上传到空间的文件内容错误

修改：默认不使用CDN（可自行设置）；重试域名和原始上传域名保持一致

* * *


**2016-11-22**

最新版本v7.0.0.3，适用于.NET xx

（xx: 2.0/3.0/3.5/4.0/4.5/4.5.1/4.5.2/4.6/4.6.1/4.6.2）

增加：上传域名默认使用CDN（`Qiniu.Common.Config.UploadFromCDN:Boolean`）

优化：应对更多类型的网络错误（`WebException`）

优化：上传失败重试域名（`upHost`）保持不变

优化：上传分块数量（`blockCount`）计算优化


* * *


**2016-10-28**

优化：上传示例中增加`UpCompleteHandler`的说明


* * *


**2016-10-24**

增加：增加`listFiles`（获取空间文件列表）功能，更新示例及文档


* * *


**2016-10-08**

推出新版本v7


* * *


**2016-08 ~ 2016-09**

增加：`Zone`模块

增加：多机房支持（华东/华北/华南/北美）

增加：示例及说明文档
