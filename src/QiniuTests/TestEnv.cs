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
         
             this.AccessKey = "6POdpY8EdqZo84Wk3TELzK9k4aG4cdlbSjE_Hj0O";
             this.SecretKey = "onZ3xebsjm_YWfFpyeRf2pc_foxJpHisYgnLrCVX";
             this.Bucket = "7qiniu";
             this.Domain = "ol4y0og1e.bkt.clouddn.com";
             this.LocalFile = "E:\\VSProjects\\csharp-sdk\\tools\\files\\test.jpg";
            
        }


    }
}
