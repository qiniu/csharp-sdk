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
        protected static string Bucket;
        protected static string Domain;
        protected static string LocalFile;

        private static bool loaded = false;

        /// <summary>
        /// 本地测试，从文件载入设置
        /// </summary>
        private void LoadSettings(string cfgFile)
        {
            if (!loaded)
            {
                string[] lines = System.IO.File.ReadAllLines(cfgFile);
                var dict = new System.Collections.Generic.Dictionary<string, string>();
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("#")) continue;

                    var d = line.Split('=');
                    dict.Add(d[0].Trim(), d[1].Trim());
                }

                AccessKey = dict["QINIU_ACCESS_KEY"];
                SecretKey = dict["QINIU_SECRET_KEY"];
                Bucket = dict["QINIU_TEST_BUCKET"];
                Domain = dict["QINIU_TEST_DOMAIN"];
                LocalFile = dict["QINIU_LOCAL_FILE"];

                loaded = true;
            }
        }

        /// <summary>
        /// 自行设置
        /// </summary>
        private void ManualSettings()
        {
            if (!loaded)
            {
                AccessKey = "QINIU_ACCESS_KEY";
                SecretKey = "QINIU_SECRET_KEY";
                Bucket = "QINIU_TEST_BUCKET";
                Domain = "QINIU_TEST_DOMAIN";
                LocalFile = "QINIU_LOCAL_FILE";
                loaded = true;

                AccessKey = "QWYn5TFQsLLU1pL5MFEmX3s5DmHdUThav9WyOWOm";
                SecretKey = "Bxckh6FA-Fbs9Yt3i3cbKVK22UPBmAOHJcL95pGz";
                Bucket = "csharpsdk";
                Domain = "csharpsdk.qiniudn.com";
                LocalFile = "E:\\Bin\\qiniu.png";
            }
        }

        /// <summary>
        /// 线上测试，读取EXPORT的设置
        /// </summary>
        private void InitSettings()
        {
            if (!loaded)
            {
                AccessKey = System.Environment.GetEnvironmentVariable("QINIU_ACCESS_KEY");
                SecretKey = System.Environment.GetEnvironmentVariable("QINIU_SECRET_KEY");
                Bucket = System.Environment.GetEnvironmentVariable("QINIU_TEST_BUCKET");
                Domain = System.Environment.GetEnvironmentVariable("QINIU_TEST_DOMAIN");
                LocalFile = System.Environment.GetEnvironmentVariable("QINIU_LOCAL_FILE");
                loaded = true;
            }
        }

        public QiniuTestEnvars()
        {
            //InitSettings();
            ManualSettings();
        }
    }
}
