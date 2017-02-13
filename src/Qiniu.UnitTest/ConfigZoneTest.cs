using NUnit.Framework;
using Qiniu.Common;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class ConfigZoneTest:QiniuTestEnvars
    {
        [Test]
        public void ZoneTest()
        {
            bool useHTTPS = false;

            Zone v1 = new Zone();
            Zone v2 = Zone.ZONE_CN_East(useHTTPS);
            Zone v3 = Zone.ZONE_CN_North(useHTTPS);
            Zone v4 = Zone.ZONE_CN_South(useHTTPS);
            Zone v5 = Zone.ZONE_US_North(useHTTPS);
        }

        [Test]
        public void QueryZoneTest()
        {
            ZoneID zid = ZoneHelper.QueryZone(AccessKey, Bucket1);
        }

        [Test]
        public void SetZoneTest()
        {
            Config.SetZone(ZoneID.CN_East, false);
        }

        [Test]
        public void AutoZoneTest()
        {
            Config.AutoZone(AccessKey, Bucket1, false);
        }

    }
}
