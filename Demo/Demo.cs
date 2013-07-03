using System;
using System.Collections.Generic;
using Qiniu.Conf;
using Qiniu.Auth;
using Qiniu.FileOp;
using Qiniu.RPC;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using Qiniu.RSF;

namespace Qiniu.Demo
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
            localLargeFile = "Resource/QQWubi_Setup_2.0.313.400.exe";
            //List(localBucket);
            string[] keys = new string[3] { "gogophser.jpg", "gogophesdr1.jpg", "gogopher.jpg" };
            Stat("icattlecoder3", "Makefilessdf");
            PutFile(localBucket, localKey, localFile);
            BatchStat(localBucket, keys);
            BatchDelete(localBucket, keys);
            BatchCopy(localBucket, keys);
            ImageOps(DEMO_DOMAIN, localKey);
         
            ResumablePutFile(localBucket, localKey, localLargeFile);
            FileManage();
            MakeGetToken(DEMO_DOMAIN,localKey);      
           
            Console.ReadLine();
        }
        /// <summary>
        /// Fetch 测试
        /// </summary>
        /// <param name="bucket"></param>
		public static void List (string bucket)
		{
			RSF.RSFClient rsf = new Qiniu.RSF.RSFClient(bucket);
            rsf.Prefix = "test";
            rsf.Limit = 100;
            List<DumpItem> items;
            while ((items=rsf.Next())!=null)
            {                
                //todo
            }
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
            IOClient client = new IOClient();   
            client.PutFinished += new EventHandler<PutRet>((o, ret) => {
                if (ret.OK)
                {
                    Console.WriteLine("Hash: " + ret.Hash);
                }
                else
                {
                    Console.WriteLine("Failed to PutFile");
                }
            });
            client.PutFile(upToken, key, fname, extra);
        }

        static void client_PutFinished(object sender, PutRet ret)
        {
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
            Console.WriteLine("\n===> ResumablePutFile {0}:{1} fname:{2}", bucket, key, fname);
            PutPolicy policy = new PutPolicy(localBucket, 3600);
            string upToken = policy.Token();
            Settings setting = new Settings();
            ResumablePutExtra extra = new ResumablePutExtra();
            extra.Bucket = bucket;
            ResumablePut client = new ResumablePut(setting, extra);
            client.Progress += new Action<float>((p) => {
                Console.WriteLine("当前进度:{0}%", p * 100);
            
            });
            client.PutFinished += new EventHandler<CallRet>((o, ret) => {
                if (ret.OK)
                {
                    Console.WriteLine("上传成功:{0}",ret.Response);
                }
                else
                {
                    Console.WriteLine("上传失败:{0}", ret.Response);
                }
            });
            client.PutFile(upToken, fname, Guid.NewGuid().ToString());
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

        #region FileManage
        /// <summary>
        /// 移动单个文件
        /// </summary>
        /// <param name="bucketSrc">需要移动的文件所在的空间名</param>
        /// <param name="keySrc">需要移动的文件</param>
        /// <param name="bucketDest">目标文件所在的空间名</param>
        /// <param name="keyDest">目标文件key</param>
        public static void Move(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            Console.WriteLine("\n===> Move {0}:{1} To {2}:{3}", 
                bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
            CallRet ret = client.Move(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
            if (ret.OK)
            {
                Console.WriteLine("Move OK");
            }
            else
            {
                Console.WriteLine("Failed to Move");
            }
        }
        /// <summary>
        /// 复制单个文件
        /// </summary>
        /// <param name="bucketSrc">需要复制的文件所在的空间名</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">目标文件所在的空间名</param>
        /// <param name="keyDest">目标文件key</param>
        public static void Copy(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            Console.WriteLine("\n===> Copy {0}:{1} To {2}:{3}",
                bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            CallRet ret = client.Copy(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
            if (ret.OK)
            {
                Console.WriteLine("Copy OK");
            }
            else
            {
                Console.WriteLine("Failed to Copy");
            }
        }
        /// <summary>
        /// 查看单个文件属性信息
        /// </summary>
        /// <param name="bucket">文件所在的空间名</param>
        /// <param name="key">文件key</param>
        public static void Stat(string bucket, string key)
        {
            Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            Entry entry = client.Stat(new Scope(bucket, key));
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
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="bucket">文件所在的空间名</param>
        /// <param name="key">文件key</param>
        public static void Delete(string bucket, string key)
        {
            Console.WriteLine("\n===> Delete {0}:{1}", bucket, key);
            RSClient client = new RSClient();
            CallRet ret = client.Delete(new Scope(bucket, key));
            if (ret.OK)
            {
                Console.WriteLine("Delete OK");
            }
            else
            {
                Console.WriteLine("Failed to delete");
            }
        }
        #endregion 
       
        #region Batch FileManage
        /// <summary>
        /// 批量查看文件信息
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="keys"></param>
        public static void BatchStat(string bucket, string[] keys)
        {
            RSClient client = new RSClient();
            List<Scope> scopes= new List<Scope>();
            foreach(string key in keys)
            {
                Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
                scopes.Add(new Scope(bucket,key));
            }
            List<BatchRetItem> ret = client.BatchStat(scopes.ToArray());
        }

        public static void BatchDelete(string bucket, string[] keys)
        {
            RSClient client = new RSClient();
            List<Scope> scopes = new List<Scope>();
            foreach (string key in keys)
            {
                Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
                scopes.Add(new Scope(bucket, key));
            }
            client.BatchDelete(scopes.ToArray());
        }
        public static void BatchCopy(string bucket, string[] keys)
        {
            List<EntryPathPair> pairs = new List<EntryPathPair>();
            foreach (string key in keys)
            {
                EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
                pairs.Add(entry);
            }
            RSClient client = new RSClient();
            client.BatchCopy(pairs.ToArray());
        }
        public static void BatchMove(string bucket, string[] keys)
        {
            List<EntryPathPair> pairs = new List<EntryPathPair>();
            foreach (string key in keys)
            {
                EntryPathPair entry = new EntryPathPair(bucket, key, Guid.NewGuid().ToString());
                pairs.Add(entry);
            }
            RSClient client = new RSClient();
            client.BatchMove(pairs.ToArray());
        }
        #endregion



        public static void MakeGetToken(string domain, string key)
        {
            string baseUrl = GetPolicy.MakeBaseUrl(domain, key);
            GetPolicy.MakeRequest(baseUrl);
            //Console.WriteLine("\n===> GetPolicy Token");
            //var policy = new GetPolicy(3600);
            //string getToken = policy.MakeBaseUrl(DEMO_DOMAIN, localKey);
            //Console.WriteLine("GetToken: " + getToken);
        }

        public static void MakePutToken(string bucketName)
        {
            PutPolicy put = new PutPolicy(bucketName);
            put.Token();
        }

        public static void ImageOps(string domian, string key)
        {

            Console.WriteLine("\n===> FileOp.ImageInfo");
            //生成base_url
            string url = Qiniu.RS.GetPolicy.MakeBaseUrl(domian, key);

            //生成fop_url
            string imageInfoURL = ImageInfo.MakeRequest(url);

            //对其签名，生成private_url。如果是公有bucket此步可以省略
            imageInfoURL = GetPolicy.MakeRequest(imageInfoURL);

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
            string exifURL = Exif.MakeRequest(url);
            exifURL = GetPolicy.MakeRequest(exifURL);
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
            string viewUrl = imageView.MakeRequest(url);
            viewUrl = GetPolicy.MakeRequest(viewUrl);
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
            string mogrUrl = imageMogr.MakeRequest(url);
            mogrUrl = GetPolicy.MakeRequest(mogrUrl);
            Console.WriteLine("ImageMogrifyURL:" + mogrUrl);

            //文字水印
            WaterMarker marker = new TextWaterMarker("hello,qiniu cloud!","","red");
            string MarkerUrl = marker.MakeRequest(url);
            //图片水印
            marker = new ImageWaterMarker("http://www.b1.qiniudn.com/images/logo-2.png");
            MarkerUrl = marker.MakeRequest(url);

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
