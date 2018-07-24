using System;
using System.IO;
using Qiniu.Http;
using Qiniu.Tests;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class ResumableUploaderTests : TestEnv
    {
        [Fact]
        public void ResumeUploadFileTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var rand = new Random();
            var key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            var filePath = LocalFile;
            Stream fs = File.OpenRead(filePath);

            var putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            var config = new Config();
            config.UseHttps = true;
            config.Zone = Zone.ZONE_CN_East;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            var target = new ResumableUploader(config);
            var extra = new PutExtra();
            //设置断点续传进度记录文件
            extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(filePath, key);
            Console.WriteLine("record file:" + extra.ResumeRecordFile);
            extra.ResumeRecordFile = "test.progress";
            var result = target.UploadStream(fs, key, token, extra);
            Console.WriteLine("resume upload: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }

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
            var target = new ResumableUploader(config);
            var result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("chunk upload result: " + result);
            Assert.Equal((int)HttpCode.OK, result.Code);
        }
    }
}
