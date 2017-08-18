using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.Storage;
using System;
namespace Qiniu.UnitTest
{
    [TestFixture]
    public class ResumeUploaderTest:QiniuTestEnvars
    {
        private Mac mac;
        private Config config;

        [SetUp]
        public void Init()
        {
            this.mac = new Mac(AccessKey, SecretKey);
            this.config = new Config();
        }
        [Test]
        public void UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.", rand.Next());
            string filePath = LocalFile;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            ResumableUploader target = new ResumableUploader(this.config);
            HttpResult result = target.UploadFile(filePath, key, token,null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }

        [Test]
        public void UploadStreamTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.", rand.Next());

            string filePath = LocalFile;
            System.IO.Stream fs = System.IO.File.OpenRead(filePath);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            ResumableUploader target = new ResumableUploader(this.config);
            HttpResult result = target.UploadStream(fs, key, token, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }
    }
}
