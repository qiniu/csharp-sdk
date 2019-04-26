namespace Qiniu.Tests
{
    /// <summary>
    /// TestEnv
    /// </summary>
    public class TestEnv
    {
        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey;
        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey;
        /// <summary>
        /// Bucket
        /// </summary>
        public string Bucket;
        /// <summary>
        /// Domain
        /// </summary>
        public string Domain;
        /// <summary>
        /// LocalFile
        /// </summary>
        public string LocalFile;
        /// <summary>
        /// Region
        /// </summary>
        public string Region;

        /// <summary>
        /// TestEnv
        /// </summary>
        /// <returns>void</returns>
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
                this.AccessKey = "";
                this.SecretKey = "";
                this.Bucket = "";
                this.Domain = "csharpsdk.qiniudn.com";
                this.LocalFile = "E:\\VSProjects\\csharp-sdk\\tools\\files\\test.jpg";
                this.Region = "";
            }
        }
    }
}
