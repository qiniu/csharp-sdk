using System;
using System.IO;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Tests.Storage
{
    public class ResumableUploaderTests : TestEnv
    {
        [Fact]
        public async Task ResumeUploadFileTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var rand = new Random();
            var key = $"UploadFileTest_{rand.Next()}.dat";

            var filePath = LocalFile;
            Stream fs = File.OpenRead(filePath);

            var putPolicy = new PutPolicy
            {
                Scope = Bucket + ":" + key,
                DeleteAfterDays = 1
            };
            putPolicy.SetExpires(3600);
            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            var config = new Config
            {
                UseHttps = true,
                Zone = Zone.ZoneCnEast,
                UseCdnDomains = true,
                ChunkSize = ChunkUnit.U512K
            };
            var target = new ResumableUploader(config);
            //设置断点续传进度记录文件
            var extra = new PutExtra { ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(filePath, key) };
            Console.WriteLine("record file:" + extra.ResumeRecordFile);
            extra.ResumeRecordFile = "test.progress";
            var result = await target.UploadStream(fs, key, token, extra);
            Console.WriteLine("resume upload: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }

        [Fact]
        public async Task UploadFileTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var rand = new Random();
            var key = $"UploadFileTest_{rand.Next()}.dat";

            var filePath = LocalFile;

            var putPolicy = new PutPolicy
            {
                Scope = Bucket + ":" + key,
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
            var target = new ResumableUploader(config);
            var result = await target.UploadFile(filePath, key, token, null);
            Console.WriteLine("chunk upload result: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
