using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using System.IO;
using System.Text;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class UploadManagerTest:QiniuTestEnvars
    {
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

            UploadManager target = new UploadManager();
            HttpResult result = target.UploadFile(filePath, key, token);

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

            UploadManager target = new UploadManager();
            HttpResult result = target.UploadData(data, key, token);
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

            UploadManager target = new UploadManager();
            HttpResult result = target.UploadStream(fs, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }
    }
}
