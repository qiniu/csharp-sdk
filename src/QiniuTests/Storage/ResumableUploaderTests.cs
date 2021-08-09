using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using System;
using Qiniu.Tests;
using System.Collections.Generic;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class ResumableUploaderTests : TestEnv
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
            char[] testBody = new char[6 * 1024 * 1024]; 
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
            ResumableUploader target = new ResumableUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("chunk upload result: " + result.ToString());
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
            char[] testBody = new char[6 * 1024 * 1024]; 
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
            PutExtra extra = new PutExtra();
            extra.Version = "v2";
            extra.PartSize = 4 * 1024 * 1024;
            ResumableUploader target = new ResumableUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, extra);
            Console.WriteLine("chunk upload result: " + result.ToString());
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            System.IO.File.Delete(filePath);
        }

        [Test]
        public void ResumeUploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());
            
            string tempPath = System.IO.Path.GetTempPath();
            int rnd = new Random().Next(1, 100000); 
            string filePath = tempPath + "resumeFile" + rnd.ToString();
            char[] testBody = new char[5 * 1024 * 1024]; 
            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.Default);
            sw.Write(testBody);
            sw.Close();
            stream.Close();
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
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            System.IO.File.Delete(filePath);
        }

        [Test]
        public void ResumeUploadFileV2Test()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Config config = new Config();
            config.UseHttps = true;
            config.Zone = Zone.ZONE_CN_East;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            ResumableUploader target = new ResumableUploader(config);
            PutExtra extra = new PutExtra();
            extra.PartSize = 4 * 1024 * 1024;
            extra.Version = "v2";

            int[] sizes = new int[5]{extra.PartSize/2, extra.PartSize, extra.PartSize+1, extra.PartSize*2, 10*1024*1024};
            foreach(int i in sizes)
            {
                char[] testBody = new char[i]; 
                Random rand = new Random();
                string key = string.Format("UploadFileTest_{0}.dat", rand.Next());
                
                string tempPath = System.IO.Path.GetTempPath();
                int rnd = new Random().Next(1, 100000); 
                string filePath = tempPath + "resumeFile" + rnd.ToString();
                System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.Default);
                sw.Write(testBody);
                sw.Close();
                stream.Close();
                System.IO.Stream fs = System.IO.File.OpenRead(filePath);

                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = Bucket + ":" + key;
                putPolicy.SetExpires(3600);
                putPolicy.DeleteAfterDays = 1;
                string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

                //设置断点续传进度记录文件
                extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(filePath, key);
                Console.WriteLine("record file:" + extra.ResumeRecordFile);
                extra.ResumeRecordFile = "test.progress";
                HttpResult result = target.UploadStream(fs, key, token, extra);
                Console.WriteLine("resume upload: " + result.ToString());
                Assert.AreEqual((int)HttpCode.OK, result.Code);
                System.IO.File.Delete(filePath);
            }
        }

    }
}