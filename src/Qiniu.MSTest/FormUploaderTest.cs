using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.IO;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Windows.Storage;

namespace Qiniu.UnitTest
{    
    [TestClass]
    public class FormUploaderTest:QiniuTestEnvars
    {
        [TestMethod]
        public async Task UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string key = FileKey1;            
            StorageFile localFile = await StorageFile.GetFileFromPathAsync(LocalFile1);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = putPolicy.Scope = Bucket1 + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            FormUploader target = new FormUploader();
            HttpResult result = await target.UploadFileAsync(localFile, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task UploadDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            byte[] data = Encoding.UTF8.GetBytes("hello world");
            string key = FileKey1;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket1 + ":" + key;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1; 
            string token = Auth.CreateUploadToken(mac,putPolicy.ToJsonString());

            FormUploader target = new FormUploader();
            HttpResult result = await target.UploadDataAsync(data, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task UploadStreamTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string key = FileKey1;
            StorageFile localFile = await StorageFile.GetFileFromPathAsync(LocalFile1);
            Stream fs = await localFile.OpenStreamForReadAsync();

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket1;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            FormUploader target = new FormUploader();
            HttpResult result = await target.UploadStreamAsync(fs, key, token);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }
    }
}
