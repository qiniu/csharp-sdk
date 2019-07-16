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
            string isTravisTest = System.Environment.GetEnvironmentVariable("isTravisTest");
            if (!string.IsNullOrEmpty(isTravisTest) && isTravisTest.Equals("true"))
            {
                this.AccessKey = System.Environment.GetEnvironmentVariable("QINIU_ACCESS_KEY");
                this.SecretKey = System.Environment.GetEnvironmentVariable("QINIU_SECRET_KEY");
                this.Bucket = System.Environment.GetEnvironmentVariable("QINIU_TEST_BUCKET");
                this.Domain = System.Environment.GetEnvironmentVariable("QINIU_TEST_DOMAIN");
                this.LocalFile = System.Environment.GetEnvironmentVariable("QINIU_LOCAL_FILE");
            }
            else
            {
                this.AccessKey = "iax_DvZNDvFProDHoARa1btrffzIyd9FN42Sec3L";
                this.SecretKey = "fieg30cLC1BwB0oOBHbC1mRMfSovp4KA_8vTjKMM";
                this.Bucket = "999";
                this.Domain = "pupoe3orm.bkt.clouddn.com";
                this.LocalFile = "E:\\VSProjects\\csharp-sdk\\tools\\files\\test.jpg";
            }
        }


    }
}
