using NUnit.Framework;
using Qiniu.Storage;
using System.IO;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;
using System;

namespace Qiniu.UnitTest
{
    public class UploadManagerTest : QiniuTestEnvars
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
            string key = string.Format("UploadFileTest_{0}.", rand.Next()); ;
            string filePath = LocalFile;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager(this.config);
            HttpResult result = target.UploadFile(filePath, key, token, null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void UploadDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            byte[] data = Encoding.UTF8.GetBytes("hello world");
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.", rand.Next());

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager(this.config);
            HttpResult result = target.UploadData(data, key, token, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }

        [Test]
        public void UploadStreamTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.", rand.Next());

            string filePath = LocalFile;
            Stream fs = File.OpenRead(filePath);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager(this.config);
            HttpResult result = target.UploadStream(fs, key, token, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }
    }
}
