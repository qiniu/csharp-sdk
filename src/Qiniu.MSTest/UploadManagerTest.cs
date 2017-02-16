using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using System.IO;
using System.Text;
using Windows.Storage;

namespace Qiniu.UnitTest
{
    [TestClass]
    public class UploadManagerTest:QiniuTestEnvars
    {
        [TestMethod]
        public async Task UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string key = FileKey1;
            StorageFile localFile = await StorageFile.GetFileFromPathAsync(LocalFile2);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = putPolicy.Scope = Bucket1 + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager();
            HttpResult result = await target.UploadFileAsync(localFile, key, token);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task UploadDataTest()
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
            HttpResult result = await target.UploadDataAsync(data, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }

        [TestMethod]
        public async Task UploadStreamTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string key = FileKey2;
            StorageFile localFile = await StorageFile.GetFileFromPathAsync(LocalFile2);
            Stream fs = await localFile.OpenStreamForReadAsync();

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket1 + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            UploadManager target = new UploadManager();
            HttpResult result = await target.UploadStreamAsync(fs, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);

        }
    }
}
