using System;
using Qiniu.Http;
using Qiniu.Tests;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class FormUploaderTests : TestEnv
    {
        [Fact]
        public void UploadFileTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var rand = new Random();
            var key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            var filePath = LocalFile;

            var putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            var target = new FormUploader(config);
            var result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
