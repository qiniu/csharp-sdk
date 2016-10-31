# Qiniu Resource (Cloud) Storage SDK for C# #

[![Build Status](https://api.travis-ci.org/qiniu/csharp-sdk.png?branch=v7)](https://travis-ci.org/qiniu/csharp-sdk)

##关于

此 C# SDK 适用于.NET Framework 2.0以上版本，基于七牛云API参考手册构建。使用此 SDK 构建您的网络应用程序，能让您以非常便捷地方式将数据安全地存储到七牛云存储上。无论您的网络应用是一个网站程序，还是包括从云端（服务端程序）到终端（手持设备应用）的架构的服务或应用，通过七牛云存储及其 SDK，都能让您应用程序的终端用户高速上传和下载，同时也让您的服务端更加轻盈。

###使用准备

####下载安装

######源码下载

    git clone https://github.com/qiniu/csharp-sdk

**注意** 

当前最新版本为v7（master与v7同步），另请参考 [v7.0.0 release](https://github.com/qiniu/csharp-sdk/releases/tag/v7.0.0)

######添加引用

获取编译好的qiniu.dll后，在项目中添加Qiniu.dll引用

######附加依赖项

C# SDK引用了第三方的开源项目[Json.NET](http://www.newtonsoft.com/json)因此，您需要在项目中引用它，或者使用NuGet安装

    Install-Package Newtonsoft.Json


####秘钥配置

要接入七牛云存储，您需要拥有一对有效的 Access Key (以下简称AK) 和 Secret Key (以下简称SK) 用来进行签名认证。步骤如下：

1. 登录七牛开发者平台，查看 [AK 和 SK](https://portal.qiniu.com/user/key)，如果没有账号，可以开通一个[七牛开发者帐号](https://portal.qiniu.com/signup)

2. 在适当位置(比如程序中的settings模块)配置 AK & SK

3. 接下来就可以调用七牛API服务，如文件上传、数据处理等

###使用示例

在QSunSync和qiniulab这两个工具中，都用到了此SDK，研究它们的源代码也能帮助您更好地了解此SDK：

* [QSunSync](https://github.com/qiniu/QSunSync)
* [QiniulaLab](https://github.com/qiniu/qiniulab)

您也可以参考官方文档 [C# SDK 使用指南](http://developer.qiniu.com/code/v7/sdk/csharp.html)

####上传文件

#####上传流程

为了尽可能地改善终端用户的上传体验，七牛云存储首创了客户端直传功能。更多信息请参阅[业务流程](http://developer.qiniu.com/article/developer/programming-model.html#workflow)。

#####普通上传

推荐使用UploadManager，以下代码示意了如何使用UploadManager来上传一个本地文件

	using System;
	using Qiniu.Util;
	using Qiniu.Storage;
	using System.IO;

	namespace ConsoleDemo
	{
    	class SimpleUploadDemo
    	{
        	public static void Main(string[] args)
        	{
            	string AK = "ACCESS_KEY";
            	string SK = "SECRET_KEY";
            	// 目标空间名
            	string bucket = "TARGET_BUCKET";
            	// 目标文件名
            	string saveKey = "SAVE_KEY";
            	// 本地文件
            	string localFile = "LOCAL_FILE";

            	// 上传策略
            	PutPolicy putPolicy = new PutPolicy();
            	// 设置要上传的目标空间
            	putPolicy.Scope = bucket;
            	// 上传策略的过期时间(单位:秒)
            	putPolicy.SetExpires(3600);
            	// 文件上传完毕后，在多少天后自动被删除
            	putPolicy.DeleteAfterDays = 1;

            	Mac mac = new Mac(AK,SK); // Use AK & SK here
            	// 生成上传凭证
            	string uploadToken = Auth.createUploadToken(putPolicy, mac);

            	UploadOptions uploadOptions = null;

            	// 上传完毕事件处理
            	UpCompletionHandler uploadCompleted = new UpCompletionHandler(OnUploadCompleted);

				// 方式1：使用UploadManager
            	//默认设置 Qiniu.Common.Config.PUT_THRESHOLD = 512*1024;
            	//可以适当修改,UploadManager会根据这个阈值自动选择是否使用分片(Resumable)上传	
            	UploadManager um = new UploadManager();
            	um.uploadFile(localFile, saveKey, token, uploadOptions, uploadCompleted);

				// 方式2：使用FormManager
            	//FormUploader fm = new FormUploader();
            	//fm.uploadFile(localFile, saveKey, token, uploadOptions, uploadCompleted);

            	Console.ReadKey();
        	}
		
			private static void OnUploadCompleted(string key, ResponseInfo respInfo, string respJson)
        	{
            	// respInfo.StatusCode
            	// respJson是返回的json消息，示例: { "key":"FILE","hash":"HASH","fsize":FILE_SIZE }
        	}
    	}
	}


#####断点续上传

实际上也是分片上传，使用ResumeUploader，参考如下示例：

	using System;
	using Qiniu.Util;
	using Qiniu.Storage;
	using System.IO;

	namespace ConsoleDemo
	{
    	class ResumableUploadDemo
    	{
        	public static void Main(string[] args)
        	{
            	string AK = "ACCESS_KEY";
            	string SK = "SECRET_KEY";
            	string bucket = "TARGET_BUCKET";
            	string saveKey = "SAVE_KEY";
            	string localFile = "LOCAL_FILE";
            	// 上传进度记录保存的目录
            	string recordPath = "RECORD_PATH";
            	// 上传进度保存为文件
            	string recordFile = "RECORD_FILE";
				
				// 设置上传时的分片大小(单位为字节,已默认设置为2MB,不得大于4MB,一般保留默认即可)
				// Qiniu.Common.CHUNK_SIZE = N_CHUNK_SIZE;
			
				UploadOptions uploadOptions = new UploadOptions(
                	null, // ExtraParams
                	null, // MimeType
                	false,  // CheckCrc32
                	new UpProgressHandler(OnUploadProgressChanged), // 上传进度
                	null // CancelSignal
                	);

            	UpCompletionHandler uploadCompleted = new UpCompletionHandler(OnUploadCompleted); // 上传完毕
			
            	// 上传时会将当前进度记录写到文件，下次可以“断点续传”
            	ResumeRecorder rr = new ResumeRecorder(recordPath);
            	// 开始上传
            	ResumeUploader ru = new ResumeUploader(
                	rr,               // 续传记录
                	recordFile,       // 续传记录文件
                	localFile,        // 待上传的本地文件
                	saveKey,          // 要保存的文件名
                	token,            // 上传凭证
                	uploadOptions,    // 上传选项(其中包含进度处理)，可为null
                	uploadCompleted   // 上传完毕事件处理
                	); 
				
            	ru.uploadFile();
            	Console.ReadKey();
        	}
		
			private static void OnUploadProgressChanged(string key,double percent)
        	{
            	// percent = [0(开始)~1.0(完成)]
        	}

        	private static void OnUploadCompleted(string key,ResponseInfo respInfo,string respJson)
        	{
            	// respInfo.StatusCode
            	// respJson是返回的json消息，示例: { "key":"FILE","hash":"HASH","fsize":FILE_SIZE }
        	}
    	}
	}


**说明**

可以使用UploadManager配合一个阈值(Qiniu.Common.PUT_THRESHOLD)让程序(SDK)自动选择使用简单上传(传适合于小文件)或者分片上传。分片上传/断点续上传使用ResumbaleUploader。

使用此SDK上传文件需要注意：

*关于UpCompletionHandler参数*

1.上传方法(如uploadFile)中包含UploadOptions,UpCompletionHandler等参数，可以保持默认(设置null即可)，也可以自行设置

2.UpCompletionHandler是**在上传完成后或者遇到错误(比如网络错误等)触发**，其中会包含一些返回信息，可以在此进行处理

*关于分片上传*

1.分片上传时，片大小(Qiniu.Common.CHUNK_SIZE)可以在上传之前预先设置

2.**CHUNK_SIZE不得大于4MB**，默认已设置为2MB(可自行设置为512KB,1MB等)

3.如果分片(chunk)较小，上传一个chunk的耗时更短，但整个文件需要被切分成更多的chunk

4.如果chunk设置得太大，上传单个chunk可能会出现连接超时的问题

5.使用ResumbaleUploader时，**上传不同的文件，请务必使用不同的recordPath/recordFile**，因为断点记录和上传文件是对应的


####文件下载

#####生成下载链接

生成授权下载链接 `ACC_URL = RAW_URL+Expire+Token`

比如原始链接 `RAW_URL = http://example.com/file/1.jpg`

加上过期时间戳(Expire)后 `http://example.com/file/1.jpg?e=1476783956`

然后Auth生成Token拼接上去 `http://example.com/file/1.jpg?e=1476783956&token=<TOKEN>`

**说明** 

如果原始链接中已包含·?·字符（如`http://xxx/1.jpg?query_what`），后面就不应该再次出现该字符，因此时间戳后缀应该是类似`&e=1476783956`这样的形式，最后的链接可能会是这样子`http://xxx/1.jpg?query_what&e=1476783956&token=<TOKEN>`

示例代码

	using Qiniu.Util;
	// AK = "ACCESS_KEY"
	// SK = "SECRET_KEY"
	// 加上过期参数，使用 ?e=<UnixTimestamp>
	// 如果rawUrl中已包含?，则改用&e=<UnixTimestamp>
	// rawURL = "RAW_URL" + "?e=1482207600"; 
	Mac mac = new Mac(AK,SK);
	string token = Auth.createDownloadToken(rawUrl, mac);
	string signedURL = rawURL + "&token=" + token;


####空间资源管理

#####简单处理

支持stat、copy、move、delete,listFiles等，具体可参阅[examples/BucketFileManagemt.cs](https://github.com/qiniu/csharp-sdk/blob/master/examples/BucketFileManagement.cs)代码和BucketManager模块说明。

**说明**

在BucketManager类中提供了listFiles方法`listFiles(bucket, prefix, marker, limit, delimiter)`.

该方法返回值中，有以下3个字段需要关注：

| 字段 | 含义 |
|-----|-----|
| Marker | 请参阅下述的`marker`参数 |
| Items  | 返回的结果，其中包括文件名、hash、大小等信息 |
| CommonPrefixes | 请参阅下述的`delimiter`和`prefix`参数 |

函数参数说明如下：

| 参数 | 含义 |
|--------|--------|
|bucket|目标空间名称|
|prefix|返回指定文件名前缀的文件列表(prefix可设为null)|
|marker|考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作) 执行listFiles操作时使用marker标记来追加新的结果。**特别注意**首次执行listFiles操作时marker为null|
|limit|每次返回结果所包含的文件总数限制(limit<=1000，*建议值100*)|
|delimiter|分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)。假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt 现设置delimiter=/ 得到结果items =[]，commonPrefixes = [fakepath/]  调整prefix=fakepath/ delimiter=null 得到所需结果items=[1.txt,2.txt] 于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件|


以下是一个简单的示例：

	// AK = "ACCESS_KEY"
	// SK = "SECRET_KEY"
	Mac mac = new Mac(AK, SK);
	BucketManager bm = new BucketManager(mac);

	string bucket = "BUCKET"; // 目标空间
    string marker = ""; // 首次请求时marker必须为空
    string prefix = null; // 按文件名前缀保留搜索结果
    string delimiter = null; // 目录分割字符(比如"/")
    int limit = 100; // 最大值1000

	// 返回结果存储在items中
	List<FileDesc> items = new List<FileDesc>();

	// 由于limit限制，可能需要执行多次操作
	// 返回值中Marker字段若非空，则表示文件数超过了limit
	do
	{
		var result = bm.listFiles(bucket, prefix, marker, limit, delimiter);
		marker = result.Marker;
		if (result.Items != null)
		{
			items.AddRange(result.Items);
		}
	} while (!string.IsNullOrEmpty(marker));

	// 在这里处理文件列表items

完整示例及其它更多代码可以参考SDK的[examples目录](https://github.com/qiniu/csharp-sdk/tree/master/examples)

#####批量处理

batch批处理:

/batch

op=`OP1`&op=`OP2`...

如下示例：

    // AK = ACCESS_KEY
	// SK = SECRET_KEY
    Mac mac = new Mac(AK, SK);
    // 批量操作类似于
    // op=<op1>&op=<op2>&op=<op3>...
    string batchOps = "BATCH_OPS";
    BucketManager bm = new BucketManager(mac);
    HttpResult result = bm.batch(batchOps);
    // 或者
    //string[] batch_ops={"<op1>","<op2>","<op3>",...};
    //bm.batch(batch_ops);
    //返回结果在这里result.Response
			
可参考[examples/BucketManagement.batch()](https://github.com/qiniu/csharp-sdk/blob/master/examples/BucketFileManagement.cs#L129)

#####新特性:force参数

move/copy支持force参数，另请参阅[资源复制的force参数](http://developer.qiniu.com/code/v6/api/kodo-api/rs/copy.html)

####持久化操作

如：fops = vframe/jpg/offset/1/w/480/h/360/rotate/90 表示视频截图。

###SDK结构

#####总体框架

C# SDK的总体框架如下图

![SDK diagram](Docs/imgs/SDKdg.png)

主要包含Auth、PutPolicy、Storage、Http、Processing等模块

######Auth

![Auth](Docs/imgs/Auth.png)

得到上传/管理/下载凭证。

另请参阅：[上传凭证](http://developer.qiniu.com/article/developer/security/upload-token.html)

######PutPolicy

![PutPolicy](Docs/imgs/PutPolicy.png)

上传策略，主要是一些参数设置。

另请参阅：[上传策略](http://developer.qiniu.com/article/developer/security/put-policy.html) 

######HttpManager

![HttpManager](Docs/imgs/HttpManager.png)

HttpManager模块负责HTTP请求与响应。

######BucketManager

![BucketManager](Docs/imgs/BucketManager.png)

BucketManager模块支持对空间文件的各种操作，如stat,copy,move等，支持批量操作(batch)。

######UploadManager

![UploadManager](Docs/imgs/UploadManager.png)

上传管理器：可以通过该上传管理器自动判断上传的内容是采用表单上传还是分片上传。

* 对于二进制数据和文件流，目前仅支持表单上传

* 对于沙盒文件，目前支持以表单方式和分片方式上传

######FormUploader

![FormUploader](Docs/imgs/FormUploader.png)

以表单的方式上传文件/字节数据/数据流

######ResumeUploader

![ResumeUploader](Docs/imgs/ResumeUploader.png)

文件分片上传，支持文件/数据流

######Pfop

![pfop](Docs/imgs/pfop.png)

持久化，可以配合上传使用，比如上传complete事件触发数据处理。

###API参考手册

* [对象存储API参考手册](http://developer.qiniu.com/article/index.html#kodo-api-handbook)

* [数据处理API参考手册](http://developer.qiniu.com/article/index.html#dora-api-handbook)

###相关资源

如果您有任何关于我们文档或产品的建议和想法，欢迎到我们的技术论坛参与讨论。

* [技术论坛](http://segmentfault.com/qiniu) - 在这里您可以和其他开发者愉快的讨论如何更好的使用七牛云服务

* [提交工单](http://support.qiniu.com/hc/request/guest/) - 如果您的问题不适合在论坛讨论或得不到回答，您也可以提交一个工单，技术支持人员会尽快回复您

* [博客](http://blog.qiniu.com/) - 这里会持续发布市场活动和技术分享文章

* [微博](http://weibo.com/qiniutek)

* [常见问题](http://developer.qiniu.com/article/faqs/)

####贡献代码

1. Fork

2. 创建您的特性分支 git checkout -b my-new-feature

3. 提交您的改动 git commit -am 'Added some feature'

4. 将您的修改记录提交到远程 git 仓库 git push origin my-new-feature

5. 然后到 github 网站的该 git 远程仓库的 my-new-feature 分支下发起 Pull Request


###许可证

Copyright (c) 2016 [qiniu.com](www.qiniu.com/)

基于 MIT 协议发布:

www.opensource.org/licenses/MIT

