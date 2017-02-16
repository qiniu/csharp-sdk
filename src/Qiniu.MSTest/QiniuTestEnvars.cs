namespace Qiniu.UnitTest
{
    /// <summary>
    /// 测试环境变量
    /// </summary>
    public class QiniuTestEnvars
    {
        // AK,SK,bucket,key
        protected static string AccessKey;
        protected static string SecretKey;
        protected static string Bucket1;
        protected static string Bucket2;
        protected static string FileKey1;
        protected static string FileKey2;        
        protected static string LocalFile1;  // 本地文件，小文件       
        protected static string LocalFile2;  // 本地文件，大文件
        protected static string TestDomain;
        protected static string TestURL1;
        protected static string TestURL2;

        private static bool loaded = false;

        /// <summary>
        /// 请自行设置
        /// </summary>
        private void ManualSettings()
        {
            if (!loaded)
            {
                AccessKey = "QINIU_ACCESS_KEY";
                SecretKey = "QINIU_SECRET_KEY";
                Bucket1 = "QINIU_BUCKET_1";
                Bucket1 = "QINIU_BUCKET_2";
                FileKey1 = "FILE_KEY_1";
                FileKey2 = "FILE_KEY_2";
                LocalFile1 = "LOCAL_FILE_1";
                LocalFile2 = "LOCAL_FILE_2";
                TestDomain = "TEST_DOMAIN";
                TestURL1 = "TEST_URL_1";
                TestURL2 = "TEST_URL_2";

                loaded = true;
            }
        }

        public QiniuTestEnvars()
        {
            ManualSettings();
        }
    }
}
