using NUnit.Framework;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class ConfigTests : TestEnv
    {
        [Test]
        public void UcHostTest()
        {
            Config config = new Config();
            string ucHost = config.UcHost();
            Assert.AreEqual("http://uc.qbox.me", ucHost);
            config.SetUcHost("uc.example.com");
            ucHost = config.UcHost();
            Assert.AreEqual("http://uc.example.com", ucHost);

            config = new Config();
            config.UseHttps = true;
            ucHost = config.UcHost();
            Assert.AreEqual("https://uc.qbox.me", ucHost);
            config.SetUcHost("uc.example.com");
            ucHost = config.UcHost();
            Assert.AreEqual("https://uc.example.com", ucHost);
        }
    }
}