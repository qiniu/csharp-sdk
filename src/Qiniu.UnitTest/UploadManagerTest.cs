using NUnit.Framework;
using Qiniu.Storage;
using System.IO;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;


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
            string key = FileKey1;
            string filePath = LocalFile1;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = putPolicy.Scope = Bucket1 + ":" + key;
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
            string key = FileKey2;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket1 + ":" + key;
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
            string key = FileKey2;

            string filePath = LocalFile1;
            Stream fs = File.OpenRead(filePath);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket1 + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager(this.config);
            HttpResult result = target.UploadStream(fs, key, token, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }
    }
}
