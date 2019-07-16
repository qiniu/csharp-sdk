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
                this.AccessKey = "6POdpY8EdqZo84Wk3TELzK9k4aG4cdlbSjE_Hj0O";
                this.SecretKey = "onZ3xebsjm_YWfFpyeRf2pc_foxJpHisYgnLrCVX";
                this.Bucket = "7qiniu";
                this.Domain = "ol4y0og1e.bkt.clouddn.com";
                this.LocalFile = "E:\\VSProjects\\csharp-sdk\\tools\\files\\test.jpg";
            }
        }


    }
}
