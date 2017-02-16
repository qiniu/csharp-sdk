using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Qiniu.CDN;
using Qiniu.CDN.Model;
using Qiniu.Util;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestClass]
    public class CdnManagerTest:QiniuTestEnvars
    {

#if LOCAL_TEST
        [TestMethod]
        public async Task RefreshTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] urls = new string[] { TestURL1, TestURL2 };
            string[] dirs = new string[] { "" };
            RefreshResult result = await target.RefreshUrlsAndDirsAsync(urls, dirs);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PrefetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] urls = new string[] { TestURL1, TestURL2 };

            PrefetchResult result = await target.PrefetchUrlsAsync(urls);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task GetBandwidthDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] domains = new string[] { TestDomain };
            string start = "2017-01-01";
            string end = "2017-01-01";
            string granu = "day";

            BandwidthResult result = await target.GetBandwidthDataAsync(domains, start, end, granu);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task GetFluxDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string[] domains = new string[] { TestDomain };
            string start = "2017-01-01";
            string end = "2017-01-01";
            string granu = "day";

            FluxResult result = await target.GetFluxDataAsync(domains, start, end, granu);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public void CreateAnitleechUrlTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager target = new CdnManager(mac);

            string qiniuKey = "12345678";
            int expireInSeconds = 600;

            TimestampAntiLeechUrlRequest req = new TimestampAntiLeechUrlRequest(TestURL2, qiniuKey, expireInSeconds);

            string result = target.CreateTimestampAntiLeechUrl(req);
        }
#endif

    }
}
