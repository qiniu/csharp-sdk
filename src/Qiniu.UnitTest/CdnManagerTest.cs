using NUnit.Framework;
using Qiniu.CDN;
using Qiniu.Util;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class CdnManagerTest:QiniuTestEnvars
    {
        private string testUrl1;
        private string testUrl2;
        [SetUp]
        public void Init()
        {
            this.testUrl1 = "http://csharpsdk.qiniudn.com/qiniu.png";
            this.testUrl2 = "http://csharpsdk.qiniudn.com/qiniu-x.png";
        }

        [Test]
        public void RefreshTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] urls = new string[] { testUrl1, testUrl2 };
            //string[] dirs = new string[] { "" };
            string[] dirs = null;
            RefreshResult result = target.RefreshUrlsAndDirs(urls, dirs);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PrefetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] urls = new string[] { testUrl1, testUrl2 };

            PrefetchResult result = target.PrefetchUrls(urls);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.RefCode);
        }

        [Test]
        public void GetBandwidthDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] domains = new string[] { "if-pbl.qiniudn.com","qdisk.qiniudn.com" };
            string start = "2017-01-01";
            string end = "2017-01-01";
            string granu = "day";

            BandwidthResult result = target.GetBandwidthData(domains, start, end, granu);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.RefCode);
        }

        [Test]
        public void GetFluxDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] domains = new string[] { Domain };
            string start = "2017-01-01";
            string end = "2017-01-01";
            string granu = "day";

            FluxResult result = target.GetFluxData(domains, start, end, granu);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.RefCode);
        }

    }
}
