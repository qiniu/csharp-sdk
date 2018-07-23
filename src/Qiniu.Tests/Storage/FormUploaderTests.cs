using Qiniu.Http;
using System;
using Qiniu.Util;
using Qiniu.Tests;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class FormUploaderTests : TestEnv
    {
        [Fact]
        public void UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            string filePath = LocalFile;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result.ToString());
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
