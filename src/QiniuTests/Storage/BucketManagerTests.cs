using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Tests;
using Qiniu.Http;
using System;
using System.Collections.Generic;
namespace Qiniu.Storage.Tests
{
    /// <summary>
    ///  BucketManagerTests
    /// </summary>
    [TestFixture]
    public class BucketManagerTests : TestEnv
    {
        /// <summary>
        ///  StatTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void StatTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string key = "qiniu.png";
            StatResult statRet = bucketManager.Stat(Bucket, key);
            if (statRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("stat error: " + statRet.ToString());
            }
            Console.WriteLine(statRet.Result.Hash);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.Fsize);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.FileType);
        }

        /// <summary>
        ///  DeleteTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void DeleteTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-delete.png";
            bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey);
            HttpResult deleteRet = bucketManager.Delete(Bucket, newKey);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("delete error: " + deleteRet.ToString());
            }
        }

        /// <summary>
        ///  CopyTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void CopyTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
        }
        /// <summary>
        ///  MoveTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void MoveTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());

            HttpResult moveRet = bucketManager.Move(Bucket, newKey, Bucket, "qiniu-move-target.png", true);
            if (moveRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("move error: " + moveRet.ToString());
            }
            Console.WriteLine(moveRet.ToString());
        }
        /// <summary>
        ///   ChangeMimeTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void ChangeMimeTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.ChangeMime(Bucket, "qiniu.png", "image/x-png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("change mime error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }
        /// <summary>
        ///  ChangeTypeTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void ChangeTypeTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.ChangeType(Bucket, "qiniu.png", 1);
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("already in line stat"))
            {
                Assert.Fail("change type error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }
        /// <summary>
        ///  ChangeStatusTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void ChangeStatusTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            //条件匹配，只有匹配上才会执行修改操作
            //cond 可以填写 空，一个或者多个
            Dictionary<string, string> cond = new Dictionary<string, string>();
            cond.Add("fsize", "186371");
            cond.Add("putTime", "14899798962573916");
            cond.Add("hash", "FiRxWzeeD6ofGTpwTZub5Fx1ozvi");
            cond.Add("mime", "application/vnd.apple.mpegurl");
            HttpResult ret = bucketManager.ChangeStatus(Bucket, "qiniu.png", 1,cond);
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("already disabled"))
            {
                Assert.Fail("change status error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        /// <summary>
        ///  DeleteAfterDaysTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void DeleteAfterDaysTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
            HttpResult expireRet = bucketManager.DeleteAfterDays(Bucket, newKey, 7);
            if (expireRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("deleteAfterDays error: " + expireRet.ToString());
            }
            Console.WriteLine(expireRet.ToString());
        }

        /// <summary>
        ///   PrefetchTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void PrefetchTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.Prefetch(Bucket, "qiniu.png");
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("bucket source not set"))
            { 
                Assert.Fail("prefetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        /// <summary>
        ///   DomainsTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void DomainsTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            DomainsResult ret = bucketManager.Domains(Bucket);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("domains error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        /// <summary>
        ///  BucketsTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BucketsTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            BucketsResult ret = bucketManager.Buckets(true);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("buckets error: " + ret.ToString());
            }

            foreach (string bucket in ret.Result)
            {
                Console.WriteLine(bucket);
            }
        }

        /// <summary>
        ///   FetchTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void FetchTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string resUrl = "http://devtools.qiniu.com/qiniu.png";
            FetchResult ret = bucketManager.Fetch(resUrl, Bucket, "qiniu-fetch.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());

            ret = bucketManager.Fetch(resUrl, Bucket, null);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        /// <summary>
        ///  CreateBucketTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void CreateBucketTest()
        {
            Config config = new Config();
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac,config);
            // 填写存储区域代号   z0:华东  z1:华北 z2:华南  na0:北美
            // Region = "z0"
            HttpResult ret = bucketManager.CreateBucket(Bucket, Region);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("create bucket error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        } 

        /// <summary>
        ///   ListFilesTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void ListFilesTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string prefix = "";
            string delimiter = "";
            int limit = 100;
            string marker = "";
            ListResult listRet = bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("list files error: " + listRet.ToString());
            }
            Console.WriteLine(listRet.ToString());
        }

        /// <summary>
        ///   ListBucketTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void ListBucketTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string prefix = "";
            string delimiter = "";
            int limit = 100;
            string marker = "";
            do
            {
                ListResult listRet = bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
                if (listRet.Code != (int)HttpCode.OK)
                {
                    Assert.Fail("list files error: " + listRet.ToString());
                }
                Console.WriteLine(listRet.ToString());

                marker = listRet.Result.Marker;
            } while (!string.IsNullOrEmpty(marker));
        }

        // batch stat, delete, copy, move, chtype, chgm, deleteAfterDays
        // 批量操作每次不能超过1000个指令
        /// <summary>
        ///  BatchStatTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchStatTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.StatOp(Bucket, key);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}", info.Data.MimeType,
                        info.Data.PutTime, info.Data.Hash, info.Data.Fsize, info.Data.FileType);
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///  BatchDeleteTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchDeleteTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.DeleteOp(Bucket, key);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("delete success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///  BatchCopyTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchCopyTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.CopyOp(Bucket, "qiniu.png", Bucket, key, true);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("copy success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///  BatchMoveTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchMoveTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.MoveOp(Bucket, key, Bucket, key + "-batch-move", true);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("move success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///   BatchChangeMimeTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchChangeMimeTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.ChangeMimeOp(Bucket, key, "image/batch-x-png");
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("chgm success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///   BatchChangeTypeTest
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchChangeTypeTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.ChangeTypeOp(Bucket, key, 0);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("chtype success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
        /// <summary>
        ///  BatchDeleteAfterDaysTes
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void BatchDeleteAfterDaysTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            //config.Region = Region.Region_CN_East;  
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = bucketManager.DeleteAfterDaysOp(Bucket, key, 7);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.Fail("batch error: " + ret.ToString());
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("deleteAfterDays success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }
    }
}