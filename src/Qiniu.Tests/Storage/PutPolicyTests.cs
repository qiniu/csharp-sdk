using NUnit.Framework;
using Qiniu.Tests;
using Qiniu.Util;
using System;
namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class PutPolicyTests : TestEnv
    {
        [Test]
        public void CreateUptokenTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            PutPolicy putPolicy = null;
            // 简单上传凭证
            putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            string upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 自定义凭证有效期（示例2小时）
            putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.SetExpires(7200);
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 覆盖上传凭证
            putPolicy = new PutPolicy();
            string keyToOverwrite = "qiniu.png";
            putPolicy.Scope = string.Format("{0}:{1}", Bucket, keyToOverwrite);
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 自定义上传回复（非callback模式）凭证
            putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.ReturnBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 带回调业务服务器的凭证（application/json）
            putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.CallbackUrl = "http://api.example.com/qiniu/upload/callback";
            putPolicy.CallbackBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";
            putPolicy.CallbackBodyType = "application/json";
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 带回调业务服务器的凭证（application/x-www-form-urlencoded）
            putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.CallbackUrl = "http://api.example.com/qiniu/upload/callback";
            putPolicy.CallbackBody = "key=$(key)&hash=$(etag)&bucket=$(bucket)&fsize=$(fsize)&name=$(x:name)";
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);

            // 带数据处理的凭证
            putPolicy = new PutPolicy();
            string saveMp4Entry = Base64.UrlSafeBase64Encode(Bucket + ":avthumb_test_target.mp4");
            string saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            string avthumbMp4Fop = "avthumb/mp4|saveas/" + saveMp4Entry;
            string vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;
            string fops = string.Join(";", new string[] { avthumbMp4Fop, vframeJpgFop });
            putPolicy.Scope = Bucket;
            putPolicy.PersistentOps = fops;
            putPolicy.PersistentPipeline = "video-pipe";
            putPolicy.PersistentNotifyUrl = "http://api.example.com/qiniu/pfop/notify";
            upToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Console.WriteLine(upToken);
        }

    }
}