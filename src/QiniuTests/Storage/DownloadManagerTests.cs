using NUnit.Framework;
using Qiniu.Storage;
using System;
using Qiniu.Util;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    /// <summary>
    ///  DownloadManagerTests
    /// </summary>
    [TestFixture]
    public class DownloadManagerTests:TestEnv
    {
        /// <summary>
        ///  CreatePrivateUrlTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public  void CreatePrivateUrlTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string domain = "http://if-pbl.qiniudn.com";
            string key = "hello/world/七牛/test.png";
            string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);
            Console.WriteLine(privateUrl);
        }

        /// <summary>
        /// CreatePublishUrlTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void CreatePublishUrlTest()
        {
            string domain = "http://if-pbl.qiniudn.com";
            string key = "hello/world/七牛/test.png";
            string publicUrl = DownloadManager.CreatePublishUrl(domain, key);
            Console.WriteLine(publicUrl);
        }
    }
}