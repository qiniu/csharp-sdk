using NUnit.Framework;

namespace Qiniu.Tests
{
    public class TestEnv
    {
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Bucket { get; }
        public string Domain { get; }

        public TestEnv()
        {
            this.AccessKey = System.Environment.GetEnvironmentVariable("QINIU_ACCESS_KEY");
            this.SecretKey = System.Environment.GetEnvironmentVariable("QINIU_SECRET_KEY");
            this.Bucket = System.Environment.GetEnvironmentVariable("QINIU_TEST_BUCKET");
            this.Domain = System.Environment.GetEnvironmentVariable("QINIU_TEST_DOMAIN");

            Assert.IsFalse(string.IsNullOrEmpty(AccessKey), "单元测试必须先配置好环境变量");
            Assert.IsFalse(string.IsNullOrEmpty(SecretKey), "单元测试必须先配置好环境变量");
        }
    }
}