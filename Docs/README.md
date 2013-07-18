---
title: CSharp SDK
---

此 SDK 适用于 .NET4 及以上版本。

SDK 在这里：[https://github.com/qiniu/csharp-sdk/tags](https://github.com/qiniu/csharp-sdk/tags)

**目录**

- [1 接入](#turn-on)
    - [配置密钥（AccessKey / SecretKey）](#establish_connection!)
- [2 授权](#auth)
    - [2.1 上传文件授权](#auth-up)
    - [2.2 下载私有文件授权](#auth-dn)
    - [2.3 文件管理授权](#auth-mgr)
- [3 存储接口](#store)
    - [3.1 上传文件](#up)
    - [3.2 下载文件](#dn)
    - [3.3 删除文件](#del)
    - [3.4 获取文件信息](#stat)
- [4 文件处理接口](#fop)
    - [4.1 图片处理](#imgfop)
        - [4.1.1 获取图片基础信息](#imageinfo)
        - [4.1.2 获取图片EXIF信息](#imageexif)
        - [4.1.3 图片缩略图](#imageview)
        - [4.1.4 高级图片处理](#imagemogrify)


<a name="turn-on"></a>

## 1 接入

<a name="establish_connection!"></a>

### 配置密钥（AccessKey / SecretKey）

要接入七牛云存储，您需要拥有一对有效的 Access Key 和 Secret Key 用来进行签名认证。可以通过如下步骤获得：

1. [开通七牛开发者帐号](https://dev.qiniutek.com/signup)
2. [登录七牛开发者自助平台，查看 Access Key 和 Secret Key](https://dev.qiniutek.com/account/keys) 。

在获取到 Access Key 和 Secret Key 之后，您可以在您的程序中调用如下两行代码进行初始化对接：

    Config.ACCESS_KEY = "<Please apply your access key>";
    Config.SECRET_KEY = "<Dont send your secret key to anyone>";

完整的代码示例参考 SDK 内置的 [Demo/Demo.cs](https://github.com/qiniu/csharp-sdk/blob/develop/Demo/Demo.cs) 文件。

<a name="auth"></a>

## 2 授权

<a name="auth-up"></a>

### 2.1 上传文件授权

上传文件需要 UpToken 来取得服务端授权。
UpToken 是由 AuthPolicy 以及 AccessKey 和 SecretKey 生成的。

    public class AuthPolicy
    {
        public string Scope { get; set; }
        public long Deadline { get; set; }
        public string CallbackUrl { get; set; }
        public string CallbackBodyType { get; set; }
        public bool Escape { get; set; }
        public string AsyncOps { get; set; }
        public string ReturnBody { get; set; }
    }

各字段的含义见[这里](http://docs.qiniutek.com/v3/api/io/#upload-token-algorithm)。

生成 UpToken 例子：

    using QBox.Auth;

    var authPolicy = new AuthPolicy(bucketName, 3600);
    authPolicy.CallbackUrl = "www.example.com/qiniu/callback";
    authPolicy.CallbackBodyType = "application/json"
    string upToken = authPolicy.MakeAuthTokenString();

<a name="auth-dn"></a>

### 2.2 下载私有文件授权

下载私有文件需要 DownloadToken 来取得服务端授权。
DownloadToken 是由 DownloadPolicy 以及 AccessKey 和 SecretKey 生成的。

    public class DownloadPolicy
    {
        public string Pattern { get; set; }
        public long Deadline { get; set; }
    }

各参数的含义见[这里](http://docs.qiniutek.com/v3/api/io/#private-download)。

生成 DownloadToken 例子：

    using QBox.Auth;

    string pattern = "dl.example.com/*";
    var downloadPolicy = new DownloadPolicy(pattern, 3600);
    string downloadToken = downloadPolicy.MakeAuthTokenString();

<a name="auth-mgr"></a>

### 2.3 文件管理授权

文件管理，比如删除文件，获取文件元信息等操作需要提供 AccessToken（放在 HTTP Header 里面） 来取得服务端授权。
AccessToken 是由 HTTP 请求的 URL，BodyType，Body 以及 AccessKey 和 SecretKey 生成的。

获取自动为请求添加 AccessToken 的客户端：

    using QBox.Auth;
    using QBox.RS;

    conn = new DigestAuthClient();
    rs = new RSService(conn, bucketName);

然后就可以用 rs 来进行文件管理操作。

<a name="store"></a>

## 3 存储接口

<a name="up"></a>

### 3.1 上传文件

上传文件需要 upToken，上传 API 为：

    public static PutFileRet PutFileWithUpToken(
                string upToken, string tableName, string key, string mimeType,
                string localFile, string customMeta, string callbackParam)

例子：

    using QBox.Auth;
    using QBox.RS;

    var authPolicy = new AuthPolicy(bucketName, 3600);
    authPolicy.CallbackUrl = "www.example.com/qiniu/callback";
    authPolicy.CallbackBodyType = "application/json"
    string upToken = authPolicy.MakeAuthTokenString();

    string callbackParam = "bucket=<bucket>&key=<key>"
    PutFileRet ret = RSClient.PutFileWithUpToken(upToken, tableName, key, null, localFile, null, callbackParam);
    if (ret.OK) Console.Writeline("upload and callback ok");

此例子是上传一个文件然后将上传的 bucket 和 key 信息回调给 www.example.com/qiniu/callback。

如果上传的文件比较大（大于4M），可以使用断点续传，其将文件在内部切割成多个 4M 的块，
一块块上传，以免直接上传出现超时或用户体验差的问题，断点续传 API 为：

    public static PutFileRet PutFile(
                Client client, string tableName, string key, string mimeType,
                string localFile, string customMeta, string callbackParam)

client 参数是能自动为请求在 HTTP Header 中添加 UpToken 的 Client。

例子：

    using QBox.Auth;
    using QBox.RS;

    var authPolicy = new AuthPolicy(bucketName, 3600);
    string upToken = authPolicy.MakeAuthTokenString();
    PutAuthClient client = new PutAuthClient(upToken);

    PutFileRet ret = ResumablePut.PutFile(client, tableName, key, null, bigFile, null, null);
    if (ret.OK) Console.Writeline("resumable put ok");

<a name="dn"></a>

### 3.2 下载文件

对于公有资源，访问方式为：

    http://<绑定域名>/key

对于[私有资源](http://docs.qiniutek.com/v3/api/io/#private-download)，需要 downloadToken，访问方式为：

    http://<绑定域名>/key?token=<downloadToken>

<a name="del"></a>

### 3.3 删除文件

需要 AccessToken 授权，删除 API 为：

    public CallRet Delete(string key)

例子：

    using QBox.Auth;
    using QBox.RS;

    conn = new DigestAuthClient();
    rs = new RSService(conn, bucketName);
    CallRet ret = rs.Delete(key);
    if (ret.OK) Console.Write("delete ok");

<a name="stat"></a>

### 3.4 获取文件元信息

需要 AccessToken 授权，获取元信息 API 为：

    public class StatRet : CallRet
    {
        public string Hash { get; private set; }
        public long FileSize { get; private set; }
        public long PutTime { get; private set; }
        public string MimeType { get; private set; }
    }
    public StatRet Stat(string key);

例子：

    using QBox.Auth;
    using QBox.RS;

    conn = new DigestAuthClient();
    rs = new RSService(conn, bucketName);
    StatRet ret = rs.Stat(key);
    if (ret.OK)
    {
        Console.WriteLine("Hash: " + ret.Hash);
        Console.WriteLine("FileSize: " + ret.FileSize);
        Console.WriteLine("PutTime: " + ret.PutTime);
        Console.WriteLine("MimeType: " + ret.MimeType);
    }

<a name="fop"></a>

## 4 文件处理接口

<a name="imgfop"></a>

### 4.1 图像处理

<a name="imageinfo"></a>

#### 4.1.1 获取图片信息

获取图片基本信息，API 为：

    public class ImageInfoRet : CallRet
    {
        public string Format { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string ColorModel { get; private set; }
    }
    public static ImageInfoRet ImageInfo(string url);

例子：

    using QBox.FileOp;

    ImageInfoRet ret = ImageOp.ImageInfo("http://yourbucket.qiniudn.com/" + key);
    if (ret.OK)
    {
        Console.WriteLine("Format: " + ret.Format);
        Console.WriteLine("Width: " + ret.Width);
        Console.WriteLine("Heigth: " + ret.Height);
        Console.WriteLine("ColorModel: " + ret.ColorModel);
    }

<a name="imageexif"></a>

#### 4.1.2 获取图片EXIF信息

获取图片 EXIF 信息，API 为：

    public static CallRet ImageExif(string url);

例子：

    using QBox.FileOp;
    using QBox.RPC;

    CallRet ret = ImageOp.ImageExif("http://yourbucket.qiniudn.com/" + key);
    if (ret.OK) Console.Writeline("Exif:\n" + ret.Response);

<a name="imageview"></a>

#### 4.1.3 图片缩略图

获取缩略图URL，API 为：

    public class ImageViewSpec
    {
        public int Mode { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Quality { get; set; }
        public string Format { get; set; }
        public int Sharpen { get; set; }

        public string MakeSpecString()
    }

具体字段含义见[这里](http://docs.qiniutek.com/v3/api/foimg/#imageView)

例子：

    using QBox.FileOp;

    ImageViewSpec viewSpec = new ImageViewSpec{Mode = 1, Width = 200, Height= 200};
    string viewUrl = ImageOp.ImageViewUrl("http://yourbucket.qiniudn.com/" + key, viewSpec);
    Console.WriteLine("ImageViewUrl:" + viewUrl);

<a name="imagemogrify"></a>

#### 4.1.4 高级图片处理

可以对存储中的图片做缩略、裁剪、旋转和格式转化处理，API 为：

    public class ImageMogrifySpec
    {
        public string Thumbnail { get; set; }
        public string Gravity { get; set; }
        public string Crop { get; set; }
        public int Quality { get; set; }
        public int Rotate { get; set; }
        public string Format { get; set; }
        public bool AutoOrient { get; set; }

        public string MakeSpecString()
    }

具体字段含义见[这里](http://docs.qiniutek.com/v3/api/foimg/#imageMogr)。

例子：

    using QBox.FileOp;

    ImageMogrifySpec mogrSpec = new ImageMogrifySpec {
                    Thumbnail = "!50x50r", Gravity = "center", Rotate = 90,
                    Crop = "!50x50", Quality = 80, AutoOrient = true};
    string mogrUrl = ImageOp.ImageMogrifyUrl("http://yourbucket.qiniudn.com/" + key, mogrSpec);
    Console.WriteLine("ImageMogrifyUrl:" + mogrUrl);

可以将处理后的图片持久化到云存储，这里需要一个结过授权的图片 URL， 可以用 Get 接口获取的，所需 API 为：

    public class GetRet : CallRet
    {
        public string Hash { get; private set; }
        public long FileSize { get; private set; }
        public string MimeType { get; private set; }
        public string Url { get; private set; }
    }
    public GetRet Get(string key, string attName);
    public PutFileRet ImageMogrifySaveAs(string url, ImageMogrifySpec spec, string key)

例子：

    using QBox.Auth;
    using QBox.RS;
    using QBox.FileOp;

    conn = new DigestAuthClient();
    rs = new RSService(conn, bucketName);
    GetRet getRet = rs.Get(key, "save-as");

    if (getRet.OK)
    {
        PutFileRet saveAsRet = rs.ImageMogrifySaveAs(getRet.Url, mogrSpec, key + ".save-as.jpg");
        if (saveAsRet.OK) Console.Writeline("mogrify ok and save to <bucketName>:<key>.save-as.jpg");
    }
