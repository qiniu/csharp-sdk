namespace Qiniu.Tests
{
    public class TestEnv
    {
        public string AccessKey;
        public string SecretKey;
        public string Bucket;
        public string Domain;
        public string LocalFile;

        public TestEnv()
        {
            this.AccessKey = System.Environment.GetEnvironmentVariable("QINIU_ACCESS_KEY");
            this.SecretKey = System.Environment.GetEnvironmentVariable("QINIU_SECRET_KEY");
            this.Bucket = System.Environment.GetEnvironmentVariable("QINIU_TEST_BUCKET");
            this.Domain = System.Environment.GetEnvironmentVariable("QINIU_TEST_DOMAIN");
            this.LocalFile = System.Environment.GetEnvironmentVariable("QINIU_LOCAL_FILE");
        }
    }
}