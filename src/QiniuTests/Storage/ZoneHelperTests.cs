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
        public void QueryZoneWithCustomQueryRegionHost()
        {
            Config config = new Config();
            config.SetQueryRegionHost("uc.qbox.me");
            config.UseHttps = true;
            
            Zone zone = ZoneHelper.QueryZone(
                AccessKey,
                Bucket,
                config.UcHost()
            );
            Assert.NotNull(zone);
        }
        
        [Test]
        public void QueryZoneWithBackupHostsTest()
        {
            Config config = new Config();
            config.SetQueryRegionHost("fake-uc.csharp.qiniu.com");
            config.SetBackupQueryRegionHosts(new List<string>
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
                config.BackupQueryRegionHosts()
            );
            Assert.NotNull(zone);
        }
        
        [Test]
        public void QueryZoneWithUcAndBackupHostsTest()
        {
            Config config = new Config();
            config.SetUcHost("fake-uc.csharp.qiniu.com");
            config.SetBackupQueryRegionHosts(new List<string>
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
                config.BackupQueryRegionHosts()
            );
            Assert.NotNull(zone);
        }
    }
}