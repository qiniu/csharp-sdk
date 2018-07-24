using System;

namespace Qiniu.Tests
{
    public class TestEnv
    {
        public string AccessKey;
        public string Bucket;
        public string Domain;
        public string LocalFile;
        public string SecretKey;

        public TestEnv()
        {
            var isTravisTest = Environment.GetEnvironmentVariable("isTravisTest");
            if (!string.IsNullOrEmpty(isTravisTest) && isTravisTest.Equals("true"))
            {
                AccessKey = Environment.GetEnvironmentVariable("QINIU_ACCESS_KEY");
                SecretKey = Environment.GetEnvironmentVariable("QINIU_SECRET_KEY");
                Bucket = Environment.GetEnvironmentVariable("QINIU_TEST_BUCKET");
                Domain = Environment.GetEnvironmentVariable("QINIU_TEST_DOMAIN");
                LocalFile = Environment.GetEnvironmentVariable("QINIU_LOCAL_FILE");
            }
            else
            {
                AccessKey = "";
                SecretKey = "";
                Bucket = "csharpsdk";
                Domain = "csharpsdk.qiniudn.com";
                LocalFile = "E:\\VSProjects\\csharp-sdk\\tools\\files\\test.jpg";
            }
        }
    }
}
