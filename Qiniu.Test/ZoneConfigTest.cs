using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Common;

namespace QiniuTest
{
    /// <summary>
    /// Zone&Config
    /// </summary>
    [TestClass()]
    public class ZoneConfigTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// A test for Zone
        /// </summary>
        [TestMethod()]
        public void zoneTest()
        {
            Zone v1 = new Zone();
            Zone v2 = Zone.ZONE_CN_East();
            Zone v3 = Zone.ZONE_CN_North();
            Zone v4 = Zone.ZONE_CN_South();
            Zone v5 = Zone.ZONE_US_North();
        }

        /// <summary>
        /// A test for AutoZone
        /// 设置AK和Bucket后通过测试
        /// </summary>
        [TestMethod()]
        public void autoZoneTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            ZoneID zid = AutoZone.Query(Settings.AccessKey, Settings.Bucket);
        }

        /// <summary>
        /// A test for Config
        /// </summary>
        [TestMethod()]
        public void configTest()
        {
            Config.ConfigZone(ZoneID.CN_East);
        }
    }
}
