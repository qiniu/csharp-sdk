---
title: C# SDK | 七牛云存储
---

# C# SDK 使用指南


此 SDK 适用于 .NET4 及以上版本。

SDK下载地址：[https://github.com/qiniu/csharp-sdk/tags](https://github.com/qiniu/csharp-sdk/tags)


**应用接入**

- [获取 Access Key 和 Secret Key](#acc-appkey)
- [签名认证](#acc-auth)

**云存储接口**

- [新建资源表](#rs-NewService)
- [获得上传授权](#rs-PutAuth)
- [上传文件](#rs-PutFile)
- [获取已上传文件信息](#rs-Stat)
- [下载文件](#rs-Get)
- [发布公开资源](#rs-Publish)
- [取消资源发布](#rs-Unpublish)
- [删除已上传的文件](#rs-Delete)
- [删除整张资源表](#rs-Drop)

## 应用接入

<a name="acc-appkey"></a>

### 1. 获取Access Key 和 Secret Key

要接入七牛云存储，您需要拥有一对有效的 Access Key 和 Secret Key 用来进行签名认证。可以通过如下步骤获得：

1. [开通七牛开发者帐号](https://dev.qiniutek.com/signup)
2. [登录七牛开发者自助平台，查看 Access Key 和 Secret Key](https://dev.qiniutek.com/account/keys) 。

<a name="acc-auth"></a>

### 2. 签名认证

首先，到 [https://github.com/qiniu/csharp-sdk](https://github.com/qiniu/csharp-sdk) 下载SDK源码。

然后，将 SDK 导入到您的 Visual C# 项目中，并编辑 QBox 目录下的 Config.cs 文件，确保其包含您从七牛开发者平台所获取的 [Access Key 和 Secret Key](#acc-appkey)：

    public static string ACCESS_KEY = "<Please apply your access key>";
	public static string SECRET_KEY = "<Dont send your secret key to anyone>";

在完成 Access Key 和 Secret Key 配置后，为了正常使用该 SDK 提供的功能，您还需要使用你获得的 Access Key 和 Secret Key 向七牛云存储服务器发出认证请求：

	DigestAuthClient conn = new DigestAuthClient();

请求成功后得到的 conn 即可用于您正常使用七牛云存储的一系列功能，接下来将一一介绍。

## 云存储接口

<a name="rs-NewService"></a>

### 1. 新建资源表

新建资源表的意义在于，您可以将所有上传的资源分布式加密存储在七牛云存储服务端后还能保持相应的完整映射索引。

    // 首先定义资源表名
    string tableName = "tableName";

    // 然后获得签名认证
    DigestAuthClient conn = new DigestAuthClient();

    // 签名认证完成后，即可使用该认证来新建资源表
    RSService rs = new RSService(conn, tableName);


<a name="rs-PutAuth"></a>

### 2. 获得上传授权

客户端上传文件之前需要取得上传授权。所谓上传授权，就是获得一个可匿名直传的临时有效URL。实例代码如下：

    // 调用资源表对象的 putAuth() 方法来获得上传授权
    PutAuthRet putAuthRet = rs.PutAuth();


如果请求成功，putAuthRet 会包含 Url 和 Expires 两个字段。Url 字段对应的值为匿名上传的临时URL，Expires 对应的值则是该临时URL的有效期。

<a name="rs-PutFile"></a>

### 3. 上传文件

七牛云存储上传文件的方式分为服务器端上传和客户端上传两种。

##### 1. 服务器端上传

上传某个本地文件，示例代码如下：

	// 调用资源表对象的 PutFile() 方法进行文件上传
	PutFileRet putFileRet = rs.PutFile(key, mimeType, filePath, customMeta);

##### 2. 客户端上传

因为服务器端已经在之前进行过签名认证持有相应的安全凭证，因此可以直接进行上传。而由于客户端并不持有对RS的安全凭证，所以需要获取上传授权，从而得到一个有上传权限的URL，参见[获得上传授权](#rs-PutAuth)。在获得上传授权URL后，就可以开始上传文件了，示例代码如下：

	// 在客户端上传文件之前，需要获得上传授权，得到经过授权的临时URL
    PutAuthRet putAuthRet = rs.PutAuth();
    uploadUrl = putAuthRet.Url;

    // 通过该临时 URL 进行文件上传
	PutFileRet putFileRet = RSClient.PutFile(uploadUrl, tableName, key, mimeType,
	                                         filePath, customMeta, callbackParam);

<a name="rs-Stat"></a>

### 4. 获取已上传文件信息

您可以调用资源表对象的 Stat() 方法并传入一个 Key 来获取指定文件的相关信息。

    // 获取资源表中特定文件信息
    StatRet statRet = rs.Stat(key);

如果请求成功，得到的 statRet 将会包含如下几个字段：

    Hash: <FileETag>
    FileSize: <FileSize>
    PutTime: <PutTime>
    MimeType: <MimeType>

<a name="rs-Get"></a>

### 5. 下载文件

要下载一个文件，首先需要取得下载授权，所谓下载授权，就是取得一个临时合法有效的下载链接，只需传入相应的文件 Key 和下载要保存的文件名作为参数即可，示例代码如下：

    // 下载资源表中的特定文件
    GetRet getRet = rs.Get(key, filename);
    GetRet getRet = rs.GetIfNotModified(key, filename, baseHash);

返回的 getRet 包含如下字段：

    Url: <DownloadURL> # 获取文件内容的实际下载地址
    Hash: <FileETag>
    FileSize: <FileSize>
    MimeType: <MimeType>

方法 GetIfNotModified 与 Get 的不同之处在于多了一个 baseHash 参数。这个参数是一个 SHA-1 值，用于判断所指向的文件内容是否被改动。只有在内容没有被变动时才会返回该文件的下载URL。

<a name="rs-Publish"></a>

### 6. 发布公开资源

使用七牛云存储提供的资源发布功能，您可以将一个资源表里边的所有文件以静态链接可访问的方式公开发布到您自己的域名下。

要公开发布一个资源表里边的所有文件，只需调用该资源表对象的 Publish() 方法并传入域名作为参数即可。如下示例：

    // 公开发布某个资源表
    PublishRet publishRet = rs.Publish(DomainName);

<a name="rs-Unpublish"></a>

### 7. 取消资源发布

调用资源表对象的 Unpublish() 方法可取消该资源表内所有文件的静态外链。

    // 取消公开发布某个资源表
    PublishRet unpublishRet = rs.Unpublish(DomainName);

<a name="rs-Delete"></a>

### 8. 删除已上传的文件

要删除指定的文件，只需调用资源表对象的 Delete() 方法并传入文件 key 作为参数即可。如下示例代码：

    // 删除资源表中的某个文件
    DeleteRet deleteRet = rs.Delete(key);

<a name="rs-Drop"></a>

### 9. 删除整张资源表

要删除整个资源表及该表里边的所有文件，可以调用资源表对象的 Drop() 方法。
需慎重，这会删除整个表及其所有文件。如下示例代码：

    // 删除整个资源表
    DropRet dropRet = rs.Drop();
