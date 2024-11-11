using NUnit.Framework;
using Qiniu.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
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

        [TestCaseSource(typeof(OperationManagerTests), nameof(OperationManagerTests.PfopOptionsTestCases))]
        public void UploadFileWithPersistOptionsTest(int type, string workflowId)
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string bucketName = Bucket;
            string key = "test-pfop/upload-file";

            // generate file to upload
            string tempPath = System.IO.Path.GetTempPath();
            int rnd = new Random().Next(1, 100000);
            string filePath = tempPath + "resumeFile" + rnd.ToString();
            char[] testBody = new char[4 * 1024 * 1024];
            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.Default);
            sw.Write(testBody);
            sw.Close();
            stream.Close();

            // generate put policy
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = string.Join(":", bucketName, key);
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;

            StringBuilder persistentKeyBuilder = new StringBuilder("test-pfop/test-pfop-by-upload");
            if (type > 0)
            {
                persistentKeyBuilder.Append("type_" + type);
                putPolicy.PersistentType = type;
            }

            if (!string.IsNullOrEmpty(workflowId))
            {
                putPolicy.PersistentWorkflowTemplateId = workflowId;
            }
            else
            {
                string saveEntry = Base64.UrlSafeBase64Encode(String.Join(
                    ":",
                    bucketName,
                    persistentKeyBuilder.ToString()
                ));
                putPolicy.PersistentOps = "avinfo|saveas/" + saveEntry;
            }

            // upload
            Config config = new Config();
            config.UseHttps = true;
            config.UseCdnDomains = true;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            FormUploader uploader = new FormUploader(config);
            HttpResult result = uploader.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result.ToString());
            Assert.AreEqual((int)HttpCode.OK, result.Code);
            System.IO.File.Delete(filePath);

            // get persist task info
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Text.ToString());
            Assert.IsTrue(dict.ContainsKey("persistentId"));
            OperationManager manager = new OperationManager(mac, config);
            PrefopResult prefopRet = manager.Prefop(dict["persistentId"].ToString());

            // assert the result
            if (prefopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("prefop error: " + prefopRet.ToString());
            }

            Assert.IsNotNull(prefopRet.Result.CreationDate);
            Assert.IsNotEmpty(prefopRet.Result.CreationDate);

            if (type == 1)
            {
                Assert.AreEqual(1, prefopRet.Result.Type);
            }

            if (!string.IsNullOrEmpty(workflowId))
            {
                Assert.IsNotNull(prefopRet.Result.TaskFrom);
                Assert.IsNotEmpty(prefopRet.Result.TaskFrom);
                Assert.IsTrue(prefopRet.Result.TaskFrom.Contains(workflowId));
            }
        }
    }
}
