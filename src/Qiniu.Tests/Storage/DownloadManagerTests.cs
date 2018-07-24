using System;
using Qiniu.Tests;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class DownloadManagerTests : TestEnv
    {
        [Fact]
        public void CreatePrivateUrlTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var domain = "http://if-pri.qiniudn.com";
            var key = "hello/world/七牛/test.png";
            var privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);
            Console.WriteLine(privateUrl);
        }

        [Fact]
        public void CreatePublishUrlTest()
        {
            var domain = "http://if-pbl.qiniudn.com";
            var key = "hello/world/七牛/test.png";
            var publicUrl = DownloadManager.CreatePublishUrl(domain, key);
            Console.WriteLine(publicUrl);
        }
    }
}
