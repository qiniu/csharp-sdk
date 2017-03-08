using NUnit.Framework;
using Qiniu.CDN;
using Qiniu.CDN.Model;
using Qiniu.Util;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class CdnManagerTest:QiniuTestEnvars
    {

        [Test]
        public void RefreshTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] urls = new string[] { TestURL1, TestURL2 };
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

            string[] urls = new string[] { TestURL1, TestURL2 };

            PrefetchResult result = target.PrefetchUrls(urls);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.RefCode);
        }

        [Test]
        public void GetBandwidthDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] domains = new string[] { TestDomain };
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

            string[] domains = new string[] { TestDomain };
            string start = "2017-01-01";
            string end = "2017-01-01";
            string granu = "day";

            FluxResult result = target.GetFluxData(domains, start, end, granu);

            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.RefCode);
        }

        [Test]
        public void CreateAnitleechUrlTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string qiniuKey = "12345678";
            int expireInSeconds = 600;

            TimestampAntiLeechUrlRequest req = new TimestampAntiLeechUrlRequest(TestURL2, qiniuKey, expireInSeconds);

            string result = target.CreateTimestampAntiLeechUrl(req);
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreateAnitleechUrlTest2()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string qiniuKey = "12345678";
            int expireInSeconds = 600;

            string host, path, file, query;
            UrlHelper.UrlSplit(TestURL2, out host, out path, out file, out query);

            string result = target.CreateTimestampAntiLeechUrl(host,path,file,query,qiniuKey,expireInSeconds);
            Assert.IsNotNull(result);
        }

    }
}
