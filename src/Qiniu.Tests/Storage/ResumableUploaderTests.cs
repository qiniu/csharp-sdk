using Qiniu.Util;
using Qiniu.Http;
using System;
using Qiniu.Tests;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class ResumableUploaderTests : TestEnv
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
            ResumableUploader target = new ResumableUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("chunk upload result: " + result.ToString());
            Assert.Equal((int)HttpCode.OK, result.Code);
        }

        [Fact]
        public void ResumeUploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            string filePath = LocalFile;
            System.IO.Stream fs = System.IO.File.OpenRead(filePath);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Config config = new Config();
            config.UseHttps = true;
            config.Zone = Zone.ZONE_CN_East;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            ResumableUploader target = new ResumableUploader(config);
            PutExtra extra = new PutExtra();
            //设置断点续传进度记录文件
            extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(filePath, key);
            Console.WriteLine("record file:" + extra.ResumeRecordFile);
            extra.ResumeRecordFile = "test.progress";
            HttpResult result = target.UploadStream(fs, key, token, extra);
            Console.WriteLine("resume upload: " + result.ToString());
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
