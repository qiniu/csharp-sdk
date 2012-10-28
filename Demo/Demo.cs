using System;
using QBox.Auth;
using QBox.RS;
using QBox.FileOp;

namespace QBox.Demo
{
    public class Demo
    {
        public static string bucketName;
        public static string key;
        public static string localFile;
        public static string DEMO_DOMAIN;
        public static Client conn;
        public static RSService rs;
        public static ImageOp imageOp;

        public static void Main()
        {
            Config.ACCESS_KEY = "<Please apply your access key>";
            Config.SECRET_KEY = "<Dont send your secret key to anyone>";

            bucketName = "csharpbucket";
            DEMO_DOMAIN = "csharpbucket.dn.qbox.me";
            localFile = "Resource/gogopher.jpg";
            key = "gogopher.jpg";

            conn = new DigestAuthClient();
            rs = new RSService(conn, bucketName);
            imageOp = new ImageOp(conn);

            MkBucket();
            RSClientPutFile();
            Get();
            Stat();
            Publish();
            UnPublish();
            Delete();
            Drop();

            MkBucket();
            RSPutFile();
            Publish();
            ImageOps();

            Console.ReadLine();
        }

        public static void MkBucket()
        {
            Console.WriteLine("\n===> RSService.MkBucket");
            CallRet callRet = rs.MkBucket();
            PrintRet(callRet);
        }

        public static void RSPutFile()
        {
            Console.WriteLine("\n===> RSService.PutFile");
            PutFileRet putFileRet = rs.PutFile(key, null, localFile, null);
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to PutFile");
            }
        }

        public static void RSClientPutFile()
        {
            Console.WriteLine("\n==> PutAuth");
            PutAuthRet putAuthRet = rs.PutAuth();
            PrintRet(putAuthRet);
            if (putAuthRet.OK)
            {
                Console.WriteLine("Expires: " + putAuthRet.Expires.ToString());
                Console.WriteLine("Url: " + putAuthRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to PutAuth");
            }

            Console.WriteLine("\n===> RSClient.PutFile");
            PutFileRet putFileRet = RSClient.PutFile(putAuthRet.Url, bucketName, key, null, localFile, null, "key=<key>");
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to RSClient.PutFile");
            }

            Console.WriteLine("\n===> Generate UpToken");
            var authPolicy = new AuthPolicy(bucketName, 3600);
            string upToken = authPolicy.MakeAuthTokenString();
            Console.WriteLine("upToken: " + upToken);

            Console.WriteLine("\n===> RSClient.PutFileWithUpToken");
            putFileRet = RSClient.PutFileWithUpToken(upToken, bucketName, key, null, localFile, null, "key=<key>");
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to RSClient.PutFileWithUpToken");
            }
        }

        public static void Get()
        {
            Console.WriteLine("\n===> Get");
            GetRet getRet = rs.Get(key, "attName");
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to Get");
            }

            Console.WriteLine("\n===> GetIfNotModified");
            getRet = rs.GetIfNotModified(key, "attName", getRet.Hash);
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to GetIfNotModified");
            }
        }

        public static void Stat()
        {
            Console.WriteLine("\n===> Stat");
            StatRet statRet = rs.Stat(key);
            PrintRet(statRet);
            if (statRet.OK)
            {
                Console.WriteLine("Hash: " + statRet.Hash);
                Console.WriteLine("FileSize: " + statRet.FileSize);
                Console.WriteLine("PutTime: " + statRet.PutTime);
                Console.WriteLine("MimeType: " + statRet.MimeType);
            }
            else
            {
                Console.WriteLine("Failed to Stat");
            }
        }

        public static void Delete()
        {
            Console.WriteLine("\n===> Delete");
            CallRet deleteRet = rs.Delete(key);
            PrintRet(deleteRet);
            if (!deleteRet.OK)
            {
                Console.WriteLine("Failed to Delete");
            }
        }

        public static void Drop()
        {
            Console.WriteLine("\n===> Drop");
            CallRet dropRet = rs.Drop();
            PrintRet(dropRet);
            if (!dropRet.OK)
            {
                Console.WriteLine("Failed to Drop");
            }
        }

        public static void Publish()
        {
            Console.WriteLine("\n===> Publish");
            CallRet publishRet = rs.Publish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to Publish");
            }
        }

        public static void UnPublish()
        {
            Console.WriteLine("\n===> UnPublish");
            CallRet publishRet = rs.Unpublish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to UnPublish");
            }
        }

        public static void ImageOps()
        {
            Console.WriteLine("\n===> ImageInfo");
            ImageInfoRet infoRet = imageOp.ImageInfo("http://" + DEMO_DOMAIN + "/" + key);
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

            Console.WriteLine("\n===> ImageExif");
            CallRet exifRet = imageOp.ImageExif("http://" + DEMO_DOMAIN + "/" + key);
            PrintRet(exifRet);
            if (!exifRet.OK)
            {
                Console.WriteLine("Failed to ImageExif");
            }

            Console.WriteLine("\n===> ImageViewUrl");
            ImageViewSpec viewSpec = new ImageViewSpec{Mode = 0, Width = 200, Height= 200};
            string viewUrl = imageOp.ImageViewUrl("http://" + DEMO_DOMAIN + "/" + key, viewSpec);
            Console.WriteLine("ImageViewUrl 1:" + viewUrl);
            viewSpec.Quality = 1;
            viewSpec.Format = "gif";
            viewUrl = imageOp.ImageViewUrl("http://" + DEMO_DOMAIN + "/" + key, viewSpec);
            Console.WriteLine("ImageViewUrl 2:" + viewUrl);
            viewSpec.Quality = 90;
            viewSpec.Sharpen = 10;
            viewSpec.Format = "png";
            viewUrl = imageOp.ImageViewUrl("http://" + DEMO_DOMAIN + "/" + key, viewSpec);
            Console.WriteLine("ImageViewUrl 3:" + viewUrl);

            Console.WriteLine("\n===> ImageMogrifyUrl");
            ImageMogrifySpec mogrSpec = new ImageMogrifySpec {
                Thumbnail = "!50x50r", Gravity = "center", Rotate = 90,
                Crop = "!50x50", Quality = 80, AutoOrient = true
            };
            string mogrUrl = imageOp.ImageMogrifyUrl("http://" + DEMO_DOMAIN + "/" + key, mogrSpec);
            Console.WriteLine("ImageMogrifyUrl:" + mogrUrl);

            Console.WriteLine("\n===> Get");
            GetRet getRet = rs.Get(key, "save-as");
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to Get");
            }
            Console.WriteLine("\n===> ImageMogrifySaveAs");
            PutFileRet saveAsRet = rs.ImageMogrifySaveAs(getRet.Url, mogrSpec, key + ".mogr-save-as");
            PrintRet(saveAsRet);
            if (saveAsRet.OK)
            {
                Console.WriteLine("Hash: " + saveAsRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to ImageMogrifySaveAs");
            }
            Console.WriteLine("\n===> Get");
            getRet = rs.Get(key + ".mogr-save-as", "mogr-save-as.jpg");
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to Get");
            }
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
