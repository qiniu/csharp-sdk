using System;
using Qiniu.Common;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RSF;
using Qiniu.Util;

namespace Qiniu.Examples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string AK; // Accesskey
            string SK;  // SecretKey
            Settings.Load("./keys", out AK, out SK);

            string bucket = "test";
            string localFile = "./big.jpg";
            string saveKey = "big.jpg";

            //var zoneId = ZoneHelper.QueryZone(AK, bucket);
            //Config.SetZone(zoneId);

            Mac mac = new Mac(AK, SK);

            // Upload file (automatically, using Simple/Resumable)
            UploadManager um = new UploadManager(mac);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SaveKey = saveKey;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;                
            var result1 = um.UploadFile(localFile, saveKey, putPolicy);
            Console.WriteLine(result1.StatusCode);
            Console.WriteLine(result1.Message);

            // FileOps
            FileOpManager fm = new FileOpManager(mac);
            string fops = "imageView2/0/w/200|saveas/" + StringHelper.UrlSafeBase64Encode(bucket + ":" + "saved.jpg");
            string key = "1.jpg";
            var result2 = fm.pfop(bucket, key, fops, null, null, false);
            Console.WriteLine(result2.StatusCode);
            Console.WriteLine(result2.Message);

            // Query fop result
            string persistentId = result2.Message;
            var result3 = fm.prefop(persistentId);
            Console.WriteLine(result3.StatusCode);
            Console.WriteLine(result3.Message);

            Console.ReadKey();
        }
    }
}
