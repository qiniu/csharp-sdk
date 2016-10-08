using System;
using Qiniu.Util;
using Qiniu.Storage;
using System.IO;

namespace Qiniu
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.LoadFromFile();
            mgrUploadFile();
            mgrUploadStream();
            frmUploadData();
            frmUploadFile();
            Console.ReadKey();
        }

		// 使用UploadManager上传文件
        public static void mgrUploadFile()
        {
            UploadManager target = new UploadManager();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_UploadManagerUploadFile.png";
            string filePath = "F:\\test.png";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);

            Console.WriteLine(key);
            Console.WriteLine(putPolicy);
            Console.WriteLine(token);

            target.uploadFile(filePath, key, token, null, null);

            Console.WriteLine();
        }

		// 使用UploadManager上传文件流
        public static void mgrUploadStream()
        {
            UploadManager target = new UploadManager();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_UploadManagerUploadStream.png";
            string filePath = "F:\\test.png";

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            string token = Auth.createUploadToken(putPolicy, mac);

            Console.WriteLine(key);
            Console.WriteLine(putPolicy);
            Console.WriteLine(token);

            target.uploadStream(fs, key, token, null, null);

            Console.WriteLine();
        }

		// 使用FormUploader上传数据
        public static void frmUploadData()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FormUploader target = new FormUploader();
            byte[] data = Encoding.UTF8.GetBytes("hello world");
            string key = "test_FormUploaderUploadData.txt";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);

            Console.WriteLine(key);
            Console.WriteLine(putPolicy);
            Console.WriteLine(token);

            target.uploadData(data, key, token, null, null);

            Console.WriteLine();
        }

		// 使用FormUploader上传文件
        public static void frmUploadFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_FormUploaderUploadFile.png";

            FormUploader target = new FormUploader();
            string filePath = "F:\\test.png";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 10;
            string token = Auth.createUploadToken(putPolicy, mac);

            Console.WriteLine(key);
            Console.WriteLine(putPolicy);
            Console.WriteLine(token);

            target.uploadFile(filePath, key, token, null, null);

            Console.WriteLine();
        }
    }

	// 账号AK&SK，目标Bucket设置
    public class Settings
    {
        //秘钥查看 https://portal.qiniu.com/user/key
        public static string AccessKey;
        public static string SecretKey;
        public static string Bucket;
        private static bool loaded = false;

        public static void load()
        {
            if (!loaded)
            {
                AccessKey = "<Your Access Key>";
                SecretKey = "<Your Secret Key>";
                Bucket = "<Your Bucket>";

                loaded = true;
            }
        }

        // 仅在测试时使用，文本文件(cFile)中逐行存放：AK,SK,Bucket
        public static void LoadFromFile(string cFile="F:\\test.cfg")
        {
            if (!loaded)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(cFile))
                {
                    AccessKey = sr.ReadLine();
                    SecretKey = sr.ReadLine();
                    Bucket = sr.ReadLine();
                    sr.Close();
                }

                loaded = true;
            }
        }
    }	
	
}
