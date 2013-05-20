using System;
using QBox.Conf;
using QBox.Auth;
using QBox.FileOp;
using QBox.RPC;
using QBox.Util;
using QBox.IO;
using QBox.RS;

namespace QBox.Demo
{
    public class Demo
    {
        public static string bucketName;
        public static string key;
        public static string localFile;
        public static string DEMO_DOMAIN;

        public static void Main()
        {
            Config.ACCESS_KEY = "<Please apply your access key>";
            Config.SECRET_KEY = "<Dont send your secret key to anyone>";

            bucketName = "yourbucket";
            DEMO_DOMAIN = bucketName + ".qiniudn.com";
            key = "gogopher.jpg";
            localFile = "Resource/gogopher.jpg";

            PutFile();
            Stat();
            Delete();
            ResumablePutFile();
            MakeGetToken();
            ImageOps();

            Console.ReadLine();
        }

        public static void PutFile()
        {
            Console.WriteLine("\n===>PutFile: Generate UpToken");
            var policy = new PutPolicy(bucketName, 3600);
            string upToken = policy.Token();
            Console.WriteLine("upToken: " + upToken);

            Console.WriteLine("\n===> PutFile");
            PutExtra extra = new PutExtra { Bucket = bucketName, MimeType = "image/jpeg" };
            PutRet ret = IOClient.PutFile(upToken, key, localFile, extra);
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

        public static void ResumablePutFile()
        {
            Console.WriteLine("\n===> ResumablePutFile: Generate UpToken");
            var policy = new PutPolicy(bucketName, 3600);
            string upToken = policy.Token();
            Console.WriteLine("upToken: " + upToken);

            PutExtra extra = new PutExtra { Bucket = bucketName, MimeType = "image/jpeg" };
            PutRet ret = IOClient.ResumablePutFile(upToken, key, localFile, extra);
            PrintRet(ret);
            if (ret.OK)
            {
                Console.WriteLine("Hash: " + ret.Hash);
            }
            else
            {
                Console.WriteLine("Failed to ResumablePutFile");
            }
        }

        public static void Stat()
        {
            Console.WriteLine("\n===> Stat");
            RSClient client = new RSClient();
            Entry entry = client.Stat(bucketName, key);
            PrintRet(entry);
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

        public static void Delete()
        {
            Console.WriteLine("\n===> Delete");
            RSClient client = new RSClient();
            CallRet ret = client.Delete(bucketName, key);
            PrintRet(ret);
            if (!ret.OK)
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
            string host = "http://" + DEMO_DOMAIN + "/" + key;

            Console.WriteLine("\n===> FileOp.ImageInfo");
            string imageInfoURL = ImageInfo.MakeRequest(host);
            ImageInfoRet infoRet = ImageInfo.Call(imageInfoURL);
            PrintRet(infoRet);
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
            CallRet exifRet = Exif.Call(exifURL);
            PrintRet(exifRet);
            if (!exifRet.OK)
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
