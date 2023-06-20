using System.Collections.Generic;
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

        [Test]
        public void QueryZoneWithBackupHostsTest()
        {
            Config config = new Config();
            config.SetUcHost("fake-uc.csharp.qiniu.com");
            config.SetBackupUcHost(new List<string>
                {
                    "unavailable-uc.csharp.qiniu.com",
                    "uc.qbox.me"
                }
            );
            config.UseHttps = true;
            
            Zone zone = ZoneHelper.QueryZone(
                AccessKey,
                Bucket,
                config.UcHost(),
                config.BackupUcHost()
            );
            Assert.NotNull(zone);
        }
    }
}