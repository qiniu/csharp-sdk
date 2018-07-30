using System;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Tests.Storage
{
    public class FormUploaderTests : TestEnv
    {
        [Fact]
        public async Task UploadFileTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var rand = new Random();
            var key = $"UploadFileTest_{rand.Next()}.dat";

            var filePath = LocalFile;

            var putPolicy = new PutPolicy
            {
                Scope = $"{Bucket}:{key}",
                DeleteAfterDays = 1
            };
            putPolicy.SetExpires(3600);
            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            var config = new Config
            {
                Zone = Zone.ZoneCnEast,
                UseHttps = true,
                UseCdnDomains = true,
                ChunkSize = ChunkUnit.U512K
            };
            var target = new FormUploader(config);
            var result = await target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
