using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Http;
using System;
using Qiniu.Util;
using Qiniu.Net40.TravisTests;

namespace Qiniu.Storage.Tests
{
    [TestClass()]
    public class FormUploaderTests : TestEnv
    {
        [TestMethod()]
        public void UploadFileTest()
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
            Console.WriteLine(putPolicy.ToJsonString());
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Config config = new Config();
            //config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadStream(fs, key, token, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            Console.WriteLine("form upload result: " + result.ToString());
        }
    }
}