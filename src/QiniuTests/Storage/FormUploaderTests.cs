using NUnit.Framework;
using Qiniu.Http;
using System;
using Qiniu.Util;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class FormUploaderTests : TestEnv
    {
        [Test]
        public void UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            string tempPath = System.IO.Path.GetTempPath();
            int rnd = new Random().Next(1, 100000); 
            string filePath = tempPath + "resumeFile" + rnd.ToString();
            char[] testBody = new char[4 * 1024 * 1024]; 
            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.Default);
            sw.Write(testBody);
            sw.Close();
            stream.Close();

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
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result.ToString());
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            System.IO.File.Delete(filePath);
        }

        [Test]
        public void UploadFileV2Test()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            string tempPath = System.IO.Path.GetTempPath();
            int rnd = new Random().Next(1, 100000); 
            string filePath = tempPath + "resumeFile" + rnd.ToString();
            char[] testBody = new char[4 * 1024 * 1024]; 
            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.Default);
            sw.Write(testBody);
            sw.Close();
            stream.Close();

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
            FormUploader target = new FormUploader(config);
            PutExtra extra = new PutExtra();
            extra.Version = "v2";
            extra.PartSize = 4 * 1024 * 1024;
            HttpResult result = target.UploadFile(filePath, key, token, extra);
            Console.WriteLine("form upload result: " + result.ToString());
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            System.IO.File.Delete(filePath);
        }

    }
}