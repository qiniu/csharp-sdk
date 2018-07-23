using Qiniu.Util;
using Qiniu.Tests;
using Qiniu.Http;
using System;
using System.Collections.Generic;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class BucketManagerTests : TestEnv
    {
        [Fact]
        public void StatTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string key = "qiniu.png";
            StatResult statRet = bucketManager.Stat(Bucket, key);
            if (statRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "stat error: " + statRet.ToString());
            }
            Console.WriteLine(statRet.Result.Hash);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.Fsize);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.FileType);
        }

        [Fact]
        public void DeleteTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-delete.png";
            bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey);
            HttpResult deleteRet = bucketManager.Delete(Bucket, newKey);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "delete error: " + deleteRet.ToString());
            }
        }

        [Fact]
        public void CopyTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
        }

        [Fact]
        public void MoveTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());

            HttpResult moveRet = bucketManager.Move(Bucket, newKey, Bucket, "qiniu-move-target.png", true);
            if (moveRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "move error: " + moveRet.ToString());
            }
            Console.WriteLine(moveRet.ToString());
        }

        [Fact]
        public void ChangeMimeTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.ChangeMime(Bucket, "qiniu.png", "image/x-png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "change mime error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void ChangeTypeTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.ChangeType(Bucket, "qiniu.png", 1);
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("already in line stat"))
            {
                Assert.True(false, "change type error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void DeleteAfterDaysTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
            HttpResult expireRet = bucketManager.DeleteAfterDays(Bucket, newKey, 7);
            if (expireRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "deleteAfterDays error: " + expireRet.ToString());
            }
            Console.WriteLine(expireRet.ToString());
        }

        [Fact]
        public void PrefetchTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            HttpResult ret = bucketManager.Prefetch(Bucket, "qiniu.png");
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("bucket source not set"))
            { 
                Assert.True(false, "prefetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void DomainsTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            DomainsResult ret = bucketManager.Domains(Bucket);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "domains error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }


        [Fact]
        public void BucketsTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            BucketsResult ret = bucketManager.Buckets(true);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "buckets error: " + ret.ToString());
            }

            foreach (string bucket in ret.Result)
            {
                Console.WriteLine(bucket);
            }
        }


        [Fact]
        public void FetchTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string resUrl = "http://devtools.qiniu.com/qiniu.png";
            FetchResult ret = bucketManager.Fetch(resUrl, Bucket, "qiniu-fetch.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());

            ret = bucketManager.Fetch(resUrl, Bucket, null);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void ListFilesTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);
            string prefix = "";
            string delimiter = "";
            int limit = 100;
            string marker = "";
            ListResult listRet = bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "list files error: " + listRet.ToString());
            }
            Console.WriteLine(listRet.ToString());
        }

        [Fact]
        public void ListBucketTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                    Assert.True(false, "list files error: " + listRet.ToString());
                }
                Console.WriteLine(listRet.ToString());

                marker = listRet.Result.Marker;
            } while (!string.IsNullOrEmpty(marker));
        }

        // batch stat, delete, copy, move, chtype, chgm, deleteAfterDays
        // 批量操作每次不能超过1000个指令
        [Fact]
        public void BatchStatTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchDeleteTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchCopyTest()
        {
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchMoveTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchChangeMimeTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchChangeTypeTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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

        [Fact]
        public void BatchDeleteAfterDaysTest()
        {
            BatchCopyTest();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
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
                Assert.True(false, "batch error: " + ret.ToString());
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
