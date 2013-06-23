using System;
using System.Collections.Generic;
using QBox.Conf;
using QBox.Auth;
using QBox.FileOp;
using QBox.RPC;
using QBox.Util;
using QBox.IO;
using QBox.RS;
using QBox.RSF;

namespace QBox.Demo
{
    public class Demo
    {
        public static string localBucket;
        public static string localKey;
        public static string localFile;
        public static string localLargeKey;
        public static string localLargeFile;
        public static string DEMO_DOMAIN;

        public static void Main()
        {
            Config.ACCESS_KEY = "gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
            Config.SECRET_KEY = "OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";

            localBucket = "icattlecoder3";
            DEMO_DOMAIN = localBucket + ".qiniudn.com";
            localKey = "gogopher.jpg";
            localFile = "Resource/gogopher.jpg";
			//List(localBucket);
            string[] keys = new string[3] { "Makefile", "Makefile", "Makefile" };
            Stat("icattlecoder3", "Makefile");
            BatchStat(localBucket, keys);


            PutFile(localBucket, localKey, localFile);
            ResumablePutFile(localBucket, localKey, localFile);
            FileManage();
            MakeGetToken();
            ImageOps();

            Console.ReadLine();
        }
        /// <summary>
        /// Fetch 测试
        /// </summary>
        /// <param name="bucket"></param>
		public static void List (string bucket)
		{
			RSF.RSFClient rsf = new QBox.RSF.RSFClient(bucket);
            rsf.Prefix = string.Empty;
            rsf.Limit = 10;
            List<DumpItem> items;
            while ((items=rsf.Next())!=null)
            {
                //todo
            }
            DumpRet ret = rsf.ListPrefix(localBucket, "test", string.Empty);
		}
        /// <summary>
        /// 上传文件测试
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="fname"></param>
        public static void PutFile(string bucket, string key, string fname)
        {
            Console.WriteLine("\n===>PutFile: Generate UpToken");
            var policy = new PutPolicy(bucket, 3600);
            string upToken = policy.Token();
            Console.WriteLine("upToken: " + upToken);

            Console.WriteLine("\n===> PutFile {0}:{1} fname:{2}", bucket, key, fname);
            PutExtra extra = new PutExtra { Bucket = bucket };
            PutRet ret = IOClient.PutFile(upToken, key, fname, extra);
            PrintRet(ret);
            if (ret.OK)
            {
                Console.WriteLine("Hash: " + ret.Hash);
            }
            else
            {
                Console.WriteLine("Failed to PutFile");
            }
        }

        public static void ResumablePutFile(string bucket, string key, string fname)
        {
            Console.WriteLine("\n===> ResumablePutFile: Generate UpToken");
            var policy = new PutPolicy(localBucket, 3600);
            string upToken = policy.Token();
            Console.WriteLine("upToken: " + upToken);

            Console.WriteLine("\n===> ResumablePutFile {0}:{1} fname:{2}", bucket, key, fname);
            PutExtra extra = new PutExtra { Bucket = bucket };
            PutRet ret = IOClient.ResumablePutFile(upToken, key, fname, extra);
            if (ret.OK)
            {
                Console.WriteLine("Hash: " + ret.Hash);
            }
            else
            {
                Console.WriteLine("Failed to ResumablePutFile");
            }
        }

        public static void FileManage()
        {
            Stat(localBucket, localKey);
            Copy(localBucket, localKey, localBucket, "copy.jpg");
            Stat(localBucket, "copy.jpg");
            Move(localBucket, "copy.jpg", localBucket, "move.jpg");
            Stat(localBucket, "move.jpg");
            Delete(localBucket, "move.jpg");
        }

        public static void Move(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            Console.WriteLine("\n===> Move {0}:{1} To {2}:{3}", 
                bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            CallRet ret = client.Move(bucketSrc, keySrc, bucketDest, keyDest);
            if (ret.OK)
            {
                Console.WriteLine("Move OK");
            }
            else
            {
                Console.WriteLine("Failed to Move");
            }
        }

        public static void Copy(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            Console.WriteLine("\n===> Copy {0}:{1} To {2}:{3}",
                bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            CallRet ret = client.Copy(bucketSrc, keySrc, bucketDest, keyDest);
            if (ret.OK)
            {
                Console.WriteLine("Copy OK");
            }
            else
            {
                Console.WriteLine("Failed to Copy");
            }
        }

        public static void Stat(string bucket, string key)
        {
            Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            Entry entry = client.Stat(bucket, key);
            if (entry.OK)
            {
                Console.WriteLine("Hash: " + entry.Hash);
                Console.WriteLine("Fsize: " + entry.Fsize);
                Console.WriteLine("PutTime: " + entry.PutTime);
                Console.WriteLine("MimeType: " + entry.MimeType);
                Console.WriteLine("Customer: " + entry.Customer);
            }
            else
            {
                Console.WriteLine("Failed to Stat");
            }
        }
        public static void BatchStat(string bucket, string[] keys)
        {
            //Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            client.BatchStat(bucket, keys);
 
        }

        public static void Delete(string bucket, string key)
        {
            Console.WriteLine("\n===> Delete {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            CallRet ret = client.Delete(bucket, key);
            if (ret.OK)
            {
                Console.WriteLine("Delete OK");
            }
            else
            {
                Console.WriteLine("Failed to Delete");
            }
        }

        public static void MakeGetToken()
        {
            Console.WriteLine("\n===> GetPolicy Token");
            string pattern = "*/*";
            var policy = new GetPolicy(pattern, 3600);
            string getToken = policy.Token();
            Console.WriteLine("GetToken: " + getToken);
        }

        public static void ImageOps()
        {
            string host = "http://" + DEMO_DOMAIN + "/" + localKey;

            Console.WriteLine("\n===> FileOp.ImageInfo");
            string imageInfoURL = ImageInfo.MakeRequest(host);
            ImageInfoRet infoRet = ImageInfo.Call(imageInfoURL);
            if (infoRet.OK)
            {
                Console.WriteLine("Format: " + infoRet.Format);
                Console.WriteLine("Width: " + infoRet.Width);
                Console.WriteLine("Heigth: " + infoRet.Height);
                Console.WriteLine("ColorModel: " + infoRet.ColorModel);
            }
            else
            {
                Console.WriteLine("Failed to ImageInfo");
            }

            Console.WriteLine("\n===> FileOp.Exif");
            string exifURL = Exif.MakeRequest(host);
            ExifRet exifRet = Exif.Call(exifURL);
            if (exifRet.OK)
            {
                Console.WriteLine("ApertureValue.val: " + exifRet["ApertureValue"].val);
                Console.WriteLine("ApertureValue.type: " + exifRet["ApertureValue"].type.ToString());
                Console.WriteLine("ExifInfo: " + exifRet.ToString());
            }
            else
            {
                Console.WriteLine("Failed to ImageExif");
            }

            Console.WriteLine("\n===> FileOp.ImageView");
            ImageView imageView = new ImageView { Mode = 0, Width = 200, Height = 200, Quality = 90, Format = "gif" };
            string viewUrl = imageView.MakeRequest(host);
            Console.WriteLine("ImageViewURL:" + viewUrl);

            Console.WriteLine("\n===> FileOp.ImageMogrify");
            ImageMogrify imageMogr = new ImageMogrify
            {
                Thumbnail = "!50x50r",
                Gravity = "center",
                Rotate = 90,
                Crop = "!50x50",
                Quality = 80,
                AutoOrient = true
            };
            string mogrUrl = imageMogr.MakeRequest(host);
            Console.WriteLine("ImageMogrifyURL:" + mogrUrl);
        }

        public static void PrintRet(CallRet callRet)
        {
            Console.WriteLine("\n[CallRet]");
            Console.WriteLine("StatusCode: " + callRet.StatusCode.ToString());
            Console.WriteLine("Response:\n" + callRet.Response);
            Console.WriteLine();
        }
    }
}
