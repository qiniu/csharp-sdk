using NUnit.Framework;
using Qiniu.Storage;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class ConfigZoneTest:QiniuTestEnvars
    {
        [Test]
        public void ZoneTest()
        {

            Zone v1 = new Zone();
            Zone v2 = Zone.ZONE_CN_East;
            Zone v3 = Zone.ZONE_CN_North;
            Zone v4 = Zone.ZONE_CN_South;
            Zone v5 = Zone.ZONE_US_North;
        }

        [Test]
        public void QueryZoneTest()
        {
            Zone zone = ZoneHelper.QueryZone(AccessKey, Bucket);
            Assert.NotNull(zone);
        }

    }
}
