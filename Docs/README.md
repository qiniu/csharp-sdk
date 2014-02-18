---
layout: docs
title: C# SDK 使用指南
---

# C# SDK 使用指南

此 C# SDK 适用于.net framework>2.0版本，基于 [七牛云存储官方API](http://docs.qiniu.com/) 构建。使用此 SDK 构建您的网络应用程序，能让您以非常便捷地方式将数据安全地存储到七牛云存储上。无论您的网络应用是一个网站程序，还是包括从云端（服务端程序）到终端（手持设备应用）的架构的服务或应用，通过七牛云存储及其 SDK，都能让您应用程序的终端用户高速上传和下载，同时也让您的服务端更加轻盈。

- [安装](#install)
- [初始化](#setup)
	- [配置密钥](#setup-key)
- [资源管理接口](#rs-api)
	- [查看单个文件属性信息](#rs-stat)
	- [复制单个文件](#rs-copy)
	- [移动单个文件](#rs-move)
	- [删除单个文件](#rs-delete)
	- [批量操作](#batch)
		- [批量获取文件属性信息](#batch-stat)
		- [批量复制文件](#batch-copy)
		- [批量移动文件](#batch-move)
		- [批量删除文件](#batch-delete)
- [资源列表](#rsf-api)
- [上传下载接口](#get-and-put-api)
	- [上传策略](#putpolicy)
	- [ 文件上传](#upload)
		- [普通上传](#io-upload)
		- [断点续上传](#resumable-io-upload)
	- [文件下载](#io-download)
		- [公有资源下载](#public-download)
		- [私有资源下载](#private-download)
- [数据处理接口](#fop-api)
	- [图像](#fop-image)
		- [查看图像属性](#fop-image-info)
		- [查看图片EXIF信息](#fop-exif)
		- [生成图片预览](#fop-image-view)
		- [图片高级处理(缩略、裁剪、旋转、转化)](#fop-image-mogr)
		- [图像水印接口](#fop-image-watermark)
- [贡献代码](#contribution)
- [许可证](#license)

<a name=install></a>
##  安装
下载:

	git clone http://github.com/qiniu/csharp-sdk

DLL引用方式:
	
	下载DLL文件，右键<项目>-<引用>文件夹，在弹出的菜单中点击"添加引用"选项后弹出"添加引用"对话框，选择”浏览"Qiniu.DLL文件,点击确定	

项目引用方式：

	下载项目文件，右键解决方案，在弹出的菜单中点击"添加"->"现有项目"，然后在弹出的对话框中选择 Qiniu.csproj"文件，点击确定。接下来与DLL引用方式类似，在"添加引用”对话框选择"项目"选项卡后选中Qiniu项目即可。

其它:

	C# SDK引用了第三方的开源项目 Json.NET,因此，您需要在项目中引用它
项目地址：[http://json.codeplex.com](http://json.codeplex.com)。

<a name=setup-key></a>
### 配置密钥

服务端使用。

要接入七牛云存储，您需要拥有一对有效的 Access Key 和 Secret Key 用来进行签名认证。可以通过如下步骤获得：

1. [开通七牛开发者帐号](https://portal.qiniu.com/signup)
2. [登录七牛开发者自助平台，查看 Access Key 和 Secret Key](https://portal.qiniu.com/setting/key) 。

在获取到 Access Key 和 Secret Key 之后，您可以在您的程序中调用如下两行代码进行初始化对接, 要确保`ACCESS_KEY` 和 `SECRET_KEY` 在<u>调用所有七牛API服务之前均已赋值</u>：

```c#
using Qiniu.Conf;
qiniu.conf.ACCESS_KEY = "<YOUR_APP_ACCESS_KEY>"
qiniu.conf.SECRET_KEY = "<YOUR_APP_SECRET_KEY>"
```

或者，编译配置文件app.conf或者web.conf等文件，添加以下配置项：

``` xml
<appSettings>
    <add key="USER_AGENT" value="" />
    <add key="ACCESS_KEY" value="" />
    <add key="SECRET_KEY" value="" />
    <add key="RS_HOST" value="" />
    <add key="UP_HOST" value="" />
    <add key="RSF_HOST" value="" />
    <add key="PREFETCH_HOST" value="" />
  </appSettings>
 ```

添加完成后，在程序启动的时候调用`Qiniu.Conf.Config.Init()`进行初始化。

<a name=rs-api></a>
## 资源管理接口

服务端使用。

基本的数据结构定义：

```c#
// 摘要:
//    对bucket、key进行封装
public class EntryPath
{
	public EntryPath(string bucket, string key);

	public string Base64EncodedURI { get; }
	//
	// 摘要:
	//     七牛云存储空间名
	public string Bucket { get; }
	//
	// 摘要:
	//     文件key
	public string Key { get; }
	//
	// 摘要:
	//     bucket+ ":"+ key
	public string URI { get; }
}
// 摘要:
//     二元操作路径
public class EntryPathPair
{
	// 摘要:
	//     二元操作路径构造函数
	// 参数:
	//   bucket:
	//     源空间名称，目标空间名称
	//   keySrc:
	//     源文件key
	//   keyDest:
	//     目标文件key
	public EntryPathPair(string bucket, string keySrc, string keyDest);
	//
	// 摘要:
	//     二元操作路径构造函数
	// 参数:
	//   bucketSrc:
	//     源空间名称
	//   keySrc:
	//     源文件key
	//   bucketDest:
	//     目标空间名称
	//   keyDest:
	//     目标文件key
	public EntryPathPair(string bucketSrc, string keySrc, string bucketDest, string keyDest);

	// 摘要:
	//     bucketDest+":"+keyDest
	public string URIDest { get; }
	//
	// 摘要:
	//     bucketSrc+":"+keySrc
	public string URISrc { get; }
}
```

<a name=rs-stat></a>
### 查看单个文件属性信息

```c#
//example
using Qiniu.RS

/// <summary>
/// 查看单个文件属性信息
/// </summary>
/// <param name="bucket">七牛云存储空间名</param>
/// <param name="key">文件key</param>
public static void Stat(string bucket, string key)
{
    RSClient client = new RSClient();
    Entry entry = client.Stat(new EntryPath(bucket, key));
    if (entry.OK)
    {
		Console.WriteLine("Hash: " + entry.Hash);
		Console.WriteLine("Fsize: " + entry.Fsize);
		Console.WriteLine("PutTime: " + entry.PutTime);
		Console.WriteLine("MimeType: " + entry.MimeType);
		Console.WriteLine("Customer: " + entry.Customer);
	}
	else
	{
		Console.WriteLine("Failed to Stat");
	}
}
```

<a name=rs-copy></a>
### 复制单个文件

```c#
//example

/// <summary>
/// 复制单个文件
/// </summary>
/// <param name="bucketSrc">需要复制的文件所在的空间名</param>
/// <param name="keySrc">需要复制的文件key</param>
/// <param name="bucketDest">目标文件所在的空间名</param>
/// <param name="keyDest">标文件key</param>
public static void Copy(string bucketSrc, string keySrc, string bucketDest, string keyDest)
{
	RSClient client = new RSClient();
	CallRet ret = client.Copy(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
	if (ret.OK)
	{
		Console.WriteLine("Copy OK");
	}
	else
	{
		Console.WriteLine("Failed to Copy");
	}
}
```

<a name=rs-move></a>
### 移动单个文件

```c#
//example

/// <summary>
/// 移动单个文件
/// </summary>
/// <param name="bucketSrc">需要移动的文件所在的空间名</param>
/// <param name="keySrc">需要移动的文件</param>
/// <param name="bucketDest">目标文件所在的空间名</param>
/// <param name="keyDest">目标文件key</param>
public static void Move(string bucketSrc, string keySrc, string bucketDest, string keyDest)
{
	Console.WriteLine("\n===> Move {0}:{1} To {2}:{3}", 
	bucketSrc, keySrc, bucketDest, keyDest);
	RSClient client = new RSClient();
	new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
	CallRet ret = client.Move(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
	if (ret.OK)
	{
		Console.WriteLine("Move OK");
	}
	else
	{
		Console.WriteLine("Failed to Move");
	}
}
```

<a name=rs-delete></a>
### 删除单个文件

```C#
//example

/// <summary>
/// 删除单个文件
/// </summary>
/// <param name="bucket">文件所在的空间名</param>
/// <param name="key">文件key</param>
public static void Delete(string bucket, string key)
{
    Console.WriteLine("\n===> Delete {0}:{1}", bucket, key);
    RSClient client = new RSClient();
    CallRet ret = client.Delete(new EntryPath(bucket, key));
    if (ret.OK)
    {
		Console.WriteLine("Delete OK");
    }
	else
	{
	Console.WriteLine("Failed to delete");
	}
}
```

<a name=batch></a>
### 批量操作
当您需要一次性进行多个操作时, 可以使用批量操作.
<a name=batch-stat></a>
#### 批量获取文件属性信息

```C#
//example

public static void BatchStat(string bucket, string[] keys)
{
	RSClient client = new RSClient();
	List<EntryPath> EntryPaths= new List<EntryPath>();
	foreach(string key in keys)
	{
		Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
		EntryPaths.Add(new EntryPath(bucket,key));
	}
	client.BatchStat(EntryPaths.ToArray()); 
}
```

<a name=batch-copy></a>
#### 批量复制文件

```C#
//example

public static void BatchCopy(string bucket, string[] keys)
{
	List<EntryPathPair> pairs = new List<EntryPathPair>();
	foreach (string key in keys)
	{
		EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
		pairs.Add(entry);
	}
	RSClient client = new RSClient();
	client.BatchCopy(pairs.ToArray());
}
```

<a name=batch-move></a>
#### 批量移动文件

```c#
//example

public static void BatchMove(string bucket, string[] keys)
{
	List<EntryPathPair> pairs = new List<EntryPathPair>();
	foreach (string key in keys)
	{
		EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
		pairs.Add(entry);
	}
	RSClient client = new RSClient();
	client.BatchMove(pairs.ToArray());
}
```

<a name=batch-delete></a>
#### 批量删除文件

```c#
//example

public static void BatchDelete(string bucket, string[] keys)
{
	RSClient client = new RSClient();
	List<EntryPath> EntryPaths = new List<EntryPath>();
	foreach (string key in keys)
	{
		Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
		EntryPaths.Add(new EntryPath(bucket, key));
	}
	client.BatchDelete(EntryPaths.ToArray());
}
```

<a name=rsf-api></a>
## 资源列表

服务端使用。

资源列表接口允许用户列出空间下的所有文件信息。使用资源列表接口如果引入Qiniu.RSF命名空间。

```c#
	using Qiniu.RSF;
	RSFClient client = new RSFClient();
	client.listPrifix("<bucketName>", "<prefix>", "<marker>", 500);
```

或者,

```c#
public static void List (string bucket)
{
	RSF.RSFClient rsf = new Qiniu.RSF.RSFClient(bucket);
	rsf.Prefix = "test";
	rsf.Limit = 100;
	List<DumpItem> items;
	while ((items=rsf.Next())!=null)
	{
		//todo
	}
}
```

<a name=get-and-put-api></a>
## 上传下载接口

服务端或客户端使用。

<a name=putpolicy></a>
### 上传策略

PutPolicy类用于定制上传策略，关于上传策略完整的说明，请参考[上传策略（PutPolicy）](http://developer.qiniu.com/docs/v6/api/reference/security/put-policy.html)。

uptoken 实际上是用 AccessKey/SecretKey对上传策略进行数字签名的字符串,用于上传接口。

```c#
string bucketName = "test";
PutPolicy put = new PutPolicy(bucketName);
string uptoken = put.Token();
```

    
<a name=upload></a>
### 文件上传
**注意**：如果您只是想要上传已存在您电脑本地或者是服务器上的文件到七牛云存储，可以直接使用七牛提供的 [qrsync](/tools/qrsync.html/) 上传工具。
文件上传有两种方式，一种是以普通方式直传文件，简称普通上传，另一种方式是断点续上传，断点续上传在网络条件很一般的情况下也能有出色的上传速度，而且对大文件的传输非常友好。

<a name=io-upload></a>
### 普通上传
普通上传的接口在 `qiniu.io` 里，如下：

上传本地文件

```c#
/// <summary>
/// 上传文件测试
/// </summary>
/// <param name="bucket"></param>
/// <param name="key"></param>
/// <param name="fname"></param>
public static void PutFile(string bucket, string key, string fname)
{
	var policy = new PutPolicy(bucket, 3600);
	string upToken = policy.Token();
	PutExtra extra = new PutExtra { Bucket = bucket };
	IOClient client = new IOClient();
	client.PutFinished += new EventHandler<PutRet>((o, ret) => {
		if (ret.OK)
		{
			Console.WriteLine("Hash: " + ret.Hash);
		}
		else
		{
			Console.WriteLine("Failed to PutFile");
		}
	});
	client.PutFile(upToken, key, fname, extra);
}
```

**注意： key必须采用utf8编码，如使用非utf8编码访问七牛云存储将反馈错误**

<a name=resumable-io-upload></a>
### 断点续上传

上传本地文件,示例如下:

```c#
public static void ResumablePutFile(string bucket, string key, string fname)
{
	Console.WriteLine("\n===> ResumablePutFile {0}:{1} fname:{2}", bucket, key, fname);
	PutPolicy policy = new PutPolicy(bucket, 3600);
	string upToken = policy.Token();
	Settings setting = new Settings();
	ResumablePutExtra extra = new ResumablePutExtra();
	extra.Bucket = bucket;
	ResumablePut client = new ResumablePut(setting, extra);
	client.Progress += new Action<float>((p) => {
	    Console.WriteLine("当前进度:{0}%", p * 100);

	});
	client.PutFinished += new EventHandler<CallRet>((o, ret) => {
	    if (ret.OK)
	    {
			Console.WriteLine("上传成功:{0}",ret.Response);
	    }
	    else
	    {
			Console.WriteLine("上传失败:{0}", ret.Response);
	    }
	});
	client.PutFile(upToken, fname, Guid.NewGuid().ToString());
}
```

ResumablePut采用分快上传，各快之间采用并行上传,通过注册事件Progress可以获取当前文件上传进度，同时您也可以通过注册ResumablePutExtra以下两个事件监听当前上传进度以及成功情况：

```c#
public event EventHandler<PutNotifyEvent> Notify;
public event EventHandler<PutNotifyErrorEvent> NotifyErr;
```

<a name=io-download></a>
### 文件下载
七牛云存储上的资源下载分为 公有资源下载 和 私有资源下载 。

私有（private）是 Bucket（空间）的一个属性，一个私有 Bucket 中的资源为私有资源，私有资源不可匿名下载。

新创建的空间（Bucket）缺省为私有，也可以将某个 Bucket 设为公有，公有 Bucket 中的资源为公有资源，公有资源可以匿名下载。

<a name=public-download></a>
#### 公有资源下载
如果在给bucket绑定了域名的话，可以通过以下地址访问。

	[GET] http://<domain>/<key>

其中<domain>是bucket所对应的域名。七牛云存储为每一个bucket提供一个默认域名。默认域名可以到[七牛云存储开发者平台](https://portal.qiniu.com/)中，空间设置的域名设置一节查询。用户也可以将自有的域名绑定到bucket上，用户可以通过自有域名访问七牛云存储。

**注意： key必须采用utf8编码，如使用非utf8编码访问七牛云存储将反馈错误**

<a name=private-download></a>
#### 私有资源下载
私有资源必须通过临时下载授权凭证(downloadToken)下载，如下：

	[GET] http://<domain>/<key>?e=<deadline>&token=<downloadToken>

注意，尖括号不是必需，代表替换项。  
私有下载链接可以使用 SDK 提供的如下方法生成：

```c#
using Qiniu.RS;
public static void MakeGetToken(string domain, string key)
{
	string baseUrl = GetPolicy.MakeBaseUrl(domain, key);
	string private_url = GetPolicy.MakeRequest(baseUrl);
}
```

<a name=fop-api></a>
## 数据处理接口

服务端或客户端使用。

七牛支持在云端对图像, 视频, 音频等富媒体进行个性化处理。使用数据处理接口需要引入Qiniu.FileOp命名空间。

```c#
using Qiniu.FileOp;
```

<a name=fop-image></a>
### 图像
<a name=fop-image-info></a>
### 查看图像属性

```c#
	string domain = "domain";
	string key = key;
	Console.WriteLine("\n===> FileOp.ImageInfo");
	//生成base_url
	string url = Qiniu.RS.GetPolicy.MakeBaseUrl(domian, key);
	//生成fop_url
	string imageInfoURL = ImageInfo.MakeRequest(url);
	//对其签名，生成private_url。如果是公有bucket此步可以省略
	imageInfoURL = GetPolicy.MakeRequest(imageInfoURL);
	ImageInfoRet infoRet = ImageInfo.Call(imageInfoURL);
	if (infoRet.OK)
	{
		Console.WriteLine("Format: " + infoRet.Format);
		Console.WriteLine("Width: " + infoRet.Width);
		Console.WriteLine("Heigth: " + infoRet.Height);
		Console.WriteLine("ColorModel: " + infoRet.ColorModel);
	}
	else
	{
		Console.WriteLine("Failed to ImageInfo");
	}
```

<a name=fop-exif></a>
### 查看图片EXIF信息

```C#
	string exifURL = Exif.MakeRequest(url);
	ExifRet exifRet = Exif.Call(exifURL);
	if (exifRet.OK)
	{
		Console.WriteLine("ApertureValue.val: " + exifRet["ApertureValue"].val);
		Console.WriteLine("ApertureValue.type: " + exifRet["ApertureValue"].type.ToString());
		Console.WriteLine("ExifInfo: " + exifRet.ToString());
	}
	else
	{
	    Console.WriteLine("Failed to ImageExif");
	}
```


<a name=fop-image-view></a>
### 生成图片预览

```c#
	ImageView imageView = new ImageView { Mode = 0, Width = 200, Height = 200, Quality = 90, Format = "gif" };
	string viewUrl = imageView.MakeRequest(url);
	viewUrl = GetPolicy.MakeRequest(viewUrl);
	Console.WriteLine("ImageViewURL:" + viewUrl);
```

<a name=fop-image-mogr></a>
### 图片高级处理(缩略、裁剪、旋转、转化)

```c#
	ImageMogrify imageMogr = new ImageMogrify
	{
		Thumbnail = "!50x50r",
		Gravity = "center",
		Rotate = 90,
		Crop = "!50x50",
		Quality = 80,
		AutoOrient = true
	};
	string mogrUrl = imageMogr.MakeRequest(url);
	mogrUrl = GetPolicy.MakeRequest(mogrUrl);
	Console.WriteLine("ImageMogrifyURL:" + mogrUrl);
```

<a name=fop-image-watermark></a>
### 图像水印接口

```c#
	//文字水印
	WaterMarker marker = new TextWaterMarker("hello,qiniu cloud!","","red");
	string MarkerUrl = marker.MakeRequest(url);
	//图片水印
	marker = new ImageWaterMarker("http://www.b1.qiniudn.com/images/logo-2.png");
	MarkerUrl = marker.MakeRequest(url);
```

<a name=contribution></a>
## 贡献代码

1. Fork
2. 创建您的特性分支 (`git checkout -b my-new-feature`)
3. 提交您的改动 (`git commit -am 'Added some feature'`)
4. 将您的修改记录提交到远程 `git` 仓库 (`git push origin my-new-feature`)
5. 然后到 github 网站的该 `git` 远程仓库的 `my-new-feature` 分支下发起 Pull Request

<a name=license></a>
## 许可证

Copyright (c) 2013 qiniu.com

基于 MIT 协议发布:

* [www.opensource.org/licenses/MIT](http://www.opensource.org/licenses/MIT)
