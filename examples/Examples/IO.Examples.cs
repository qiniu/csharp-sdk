using System;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 文件上传示例
    /// </summary>
    public class UploadDemo
    {
        /// <summary>
        /// 使用表单上传方式上传小文件
        /// </summary>
        public static void uploadFile()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独提供了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "x-1-2-测试.txt";
            string localFile = "D:/QFL/测试.txt";

            // 上传策略，参见 
            // http://developer.qiniu.com/article/developer/security/put-policy.html
            PutPolicy putPolicy = new PutPolicy();

            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);

            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 将PutPolicy转换为JSON字符串
            string jstr = putPolicy.ToJsonString();

            // 生成上传凭证，参见
            // http://developer.qiniu.com/article/developer/security/upload-token.html            
            string token = Auth.CreateUploadToken(mac, jstr);

            FormUploader fu = new FormUploader();

            // 支持自定义参数
            var extra = new System.Collections.Generic.Dictionary<string, string>();
            extra.Add("FileType", "UploadFromLocal");
            extra.Add("YourKey", "YourValue");

            var result = fu.UploadFile(localFile, saveKey, token);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        public static void uploadData()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "x-1";
            //byte[] data = System.Text.Encoding.UTF8.GetBytes("这是一个简单的测试");
            byte[] data = System.IO.File.ReadAllBytes("D:/QFL/1.mp4");

            // 上传策略，参见 
            // http://developer.qiniu.com/article/developer/security/put-policy.html
            PutPolicy putPolicy = new PutPolicy();

            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            //putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);

            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 将PutPolicy转换为JSON字符串
            string jstr = putPolicy.ToJsonString();

            // 生成上传凭证，参见
            // http://developer.qiniu.com/article/developer/security/upload-token.html            
            string token = Auth.CreateUploadToken(mac, jstr);

            UploadManager um = new UploadManager();

            var result = um.UploadData(data, saveKey, token);

            Console.WriteLine(result);

        }

        /// <summary>
        /// 上传数据流
        /// </summary>
        public static void uploadStream()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "x-2";

            // 上传策略，参见 
            // http://developer.qiniu.com/article/developer/security/put-policy.html
            PutPolicy putPolicy = new PutPolicy();

            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            //putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);

            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 将PutPolicy转换为JSON字符串
            string jstr = putPolicy.ToJsonString();

            // 生成上传凭证，参见
            // http://developer.qiniu.com/article/developer/security/upload-token.html            
            string token = Auth.CreateUploadToken(mac, jstr);

            UploadManager um = new UploadManager();

            string localFile = "D:/QFL/1.mp4";

            System.IO.FileStream fs = new System.IO.FileStream(localFile, System.IO.FileMode.Open);

            var result = um.UploadStream(fs, saveKey, token);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 上传数据流(NetStream)
        /// </summary>
        public static void uploadNetStream()
        {
            try
            {
                string url = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
                var wReq = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                var resp = wReq.GetResponse() as System.Net.HttpWebResponse;
                using (var stream = resp.GetResponseStream())
                {
                    Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
                    PutPolicy putPolicy = new PutPolicy();
                    putPolicy.Scope = "xuejing-001.jpg";
                    putPolicy.SetExpires(3600);
                    string jstr = putPolicy.ToJsonString();
                    string token = Auth.CreateUploadToken(mac, jstr);

                    // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                    // 请使用FormUploader或者ResumableUploader的UploadStream方法
                    FormUploader fu = new FormUploader();
                    var result = fu.UploadStream(stream, "xuejing-001.jpg", token);
                    Console.WriteLine(result);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 上传完毕后触发数据处理
        /// </summary>
        public static void uploadWithFop()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独提供了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "x-1.jpg";
            string localFile = "D:\\QFL\\1.png";

            // 如果想要将处理结果保存到SAVEAS_BUCKET空间下，文件名为SAVEAS_KEY
            // 可以使用savas参数 <FOPS>|saveas/<encodedUri>
            // 根据fop操作不同，上传完毕后云端数据处理可能需要消耗一定的处理时间
            string encodedUri = Base64.UrlSafeBase64Encode(bucket + ":" + "1-r-x.jpg");
            // 进行imageView2操作后将结果另存
            string fops = "imageView2/0/w/200|saveas/" + encodedUri;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.PersistentOps = fops;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            putPolicy.PersistentNotifyUrl = "http://xar.fengyh.cn/qiniu/upload-callback/handler.php";

            // 将PutPolicy转换为JSON字符串
            string jstr = putPolicy.ToJsonString();

            string token = Auth.CreateUploadToken(mac, jstr);

            FormUploader fu = new FormUploader();

            var result = fu.UploadFile(localFile, saveKey, token);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 上传大文件，支持断点续上传
        /// </summary>
        public static void uploadBigFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "video-x1-1.mp4";
            string localFile = "D:/QFL/1.mp4";

            // 断点记录文件，可以不用设置，让SDK自动生成，如果出现续上传的情况，SDK会尝试从该文件载入断点记录
            // SDK自动生成的文件为Path.Combine(UserEnv.GetHomeFolder(), ResumeHelper.GetDefaultRecordKey(localFile, saveKey))
            // 对于不同的上传任务，请使用不同的recordFile
            string recordFile = "D:/QFL/resume";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;

            // 将PutPolicy转换为JSON字符串
            string jstr = putPolicy.ToJsonString();

            string token = Auth.CreateUploadToken(mac, jstr);

            // 包含两个参数，并且都有默认值
            // 参数1(bool): uploadFromCDN是否从CDN加速上传，默认否
            // 参数2(enum): chunkUnit上传分片大小，可选值128KB,256KB,512KB,1024KB,2048KB,4096KB
            ResumableUploader ru = new ResumableUploader(false, ChunkUnit.U1024K);

            // ResumableUploader.uploadFile有多种形式，您可以根据需要来选择
            //
            // 最简模式，使用默认recordFile和默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token)
            // 
            // 基本模式，使用默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token,recordFile)
            //
            // 一般模式，使用自定义进度处理(监视上传进度)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler)
            //
            // 高级模式，包含上传控制(可控制暂停/继续 或者强制终止)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler,uploadController)
            //
            // 高级模式，包含上传控制(可控制暂停 / 继续 或者强制终止)， 可设置最大尝试次数
            // UploadFile(localFile,saveKey,token,recordFile,maxTry,uploadProgressHandler,uploadController)

            // 使用默认进度处理，使用自定义上传控制
            UploadProgressHandler upph = new UploadProgressHandler(ResumableUploader.DefaultUploadProgressHandler);
            UploadController upctl = new UploadController(uploadControl);

            var result = ru.UploadFile(localFile, saveKey, token, recordFile, upph, upctl);

            Console.WriteLine(result);
        }

        // 内部变量，仅作大文件上传时的演示
        private static bool paused = false;

        /// <summary>
        /// 上传控制，仅作大文件上传时的演示
        /// </summary>
        /// <returns></returns>
        private static UPTS uploadControl()
        {
            // 这个函数只是作为一个演示，实际当中请根据需要来设置
            // 这个演示的实际效果是“走走停停”，也就是停一下又继续，如此重复直至上传结束
            //paused = !paused;
            paused = false;

            if (paused)
            {
                return UPTS.Suspended;
            }
            else
            {
                return UPTS.Activated;
            }
        }
    }

    /// <summary>
    /// 文件下载示例
    /// </summary>
    public class DownloadDemo
    { 
        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        public static void downloadFile()
        {
            // 文件URL
            string rawUrl = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
            // 要保存的文件名
            string saveFile = "D:\\QFL\\saved-1.jpg";

            // 可公开访问的url，直接下载
           var result = DownloadManager.Download(rawUrl, saveFile);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 下载私有空间中的文件(不适用于大文件)
        /// </summary>
        public static void downloadPrivateFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string rawUrl = "http://your-bucket.cloud-storage.com/1.jpg";
            string saveFile = "D:\\QFL\\saved-2.jpg";

            string accUrl = DownloadManager.CreateSignedUrl(mac, rawUrl, 3600);

            // 接下来可以使用accUrl来下载文件
            var result = DownloadManager.Download(accUrl, saveFile);

            Console.WriteLine(result);
        }
    }
}