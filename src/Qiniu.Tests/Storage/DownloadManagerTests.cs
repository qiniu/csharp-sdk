using System;
using Qiniu.Util;
using Qiniu.Tests;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class DownloadManagerTests : TestEnv
    {
        [Fact]
        public void CreatePrivateUrlTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            string domain = "http://if-pri.qiniudn.com";
            string key = "hello/world/七牛/test.png";
            string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);
            Console.WriteLine(privateUrl);
        }

        [Fact]
        public void CreatePublishUrlTest()
        {
            string domain = "http://if-pbl.qiniudn.com";
            string key = "hello/world/七牛/test.png";
            string publicUrl = DownloadManager.CreatePublishUrl(domain, key);
            Console.WriteLine(publicUrl);
        }
    }
}
