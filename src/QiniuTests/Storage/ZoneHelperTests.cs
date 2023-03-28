using NUnit.Framework;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class ZoneHelperTests : TestEnv
    {
        [Test]
        public void QueryZoneTest()
        {
            Zone zone = ZoneHelper.QueryZone(AccessKey, Bucket);
            
            Assert.NotNull(zone);
        }
    }
}