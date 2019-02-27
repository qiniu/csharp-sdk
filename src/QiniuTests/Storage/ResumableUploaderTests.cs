using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using System;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    /// <summary>
    /// ResumableUploaderTests
    /// </summary>
    [TestFixture]
    public class ResumableUploaderTests:TestEnv
    {
        /// <summary>
        /// UploadFileTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void UploadFileTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Random rand = new Random();
            string key = string.Format("UploadFileTest_{0}.dat", rand.Next());

            string filePath = LocalFile;

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
        }
    }
}