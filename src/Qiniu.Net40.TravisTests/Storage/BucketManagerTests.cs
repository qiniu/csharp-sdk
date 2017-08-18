using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Util;
using Qiniu.Net40.TravisTests;
using Qiniu.Http;
using System;
using System.Collections.Generic;
namespace Qiniu.Storage.Tests
{
    [TestClass()]
    public class BucketManagerTests : TestEnv
    {
        private BucketManager bucketManager;
        [TestInitialize]
        public void Init()
        {
            Config config = new Config();
            Mac mac = new Mac(AccessKey, SecretKey);
            this.bucketManager = new BucketManager(mac, config);
        }

        [TestMethod()]
        public void ChgmTest()
        {
            string key = "qiniu.png";
            string newMime = "image/x-png";
            HttpResult chgmRet = this.bucketManager.ChangeMime(Bucket, key, newMime);
            if (chgmRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("chgm error: " + chgmRet.ToString());
            }

            StatResult statRet = this.bucketManager.Stat(Bucket, key);
            if (!statRet.Result.MimeType.Equals(newMime))
            {
                Assert.Fail("stat error: " + statRet.ToString());
            }
        }

        [TestMethod()]
        public void StatTest()
        {
            string key = "qiniu.png";
            StatResult statRet = this.bucketManager.Stat(Bucket, key);
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

        [TestMethod()]
        public void BucketsTest()
        {
            BucketsResult ret = this.bucketManager.Buckets(true);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("buckets error: " + ret.ToString());
            }

            foreach (string bucket in ret.Result)
            {
                Console.WriteLine(bucket);
            }
        }

        [TestMethod()]
        public void DeleteTest()
        {
            string newKey = "qiniu-to-delete.png";
            this.bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey);
            HttpResult deleteRet = this.bucketManager.Delete(Bucket, newKey);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("delete error: " + deleteRet.ToString());
            }
        }

        [TestMethod()]
        public void CopyTest()
        {
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = this.bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
        }

        [TestMethod()]
        public void MoveTest()
        {
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = this.bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());

            HttpResult moveRet = this.bucketManager.Move(Bucket, newKey, Bucket, "qiniu-move-target.png", true);
            if (moveRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("move error: " + moveRet.ToString());
            }
            Console.WriteLine(moveRet.ToString());
        }

        [TestMethod()]
        public void ChangeMimeTest()
        {
            HttpResult ret = this.bucketManager.ChangeMime(Bucket, "qiniu.png", "image/x-png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("change mime error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [TestMethod()]
        public void ChangeTypeTest()
        {
            HttpResult ret = this.bucketManager.ChangeType(Bucket, "qiniu.png", 1);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("change type error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [TestMethod()]
        public void DeleteAfterDaysTest()
        {
            string newKey = "qiniu-to-copy.png";
            HttpResult copyRet = this.bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("copy error: " + copyRet.ToString());
            }
            Console.WriteLine(copyRet.ToString());
            HttpResult expireRet = this.bucketManager.DeleteAfterDays(Bucket, newKey, 7);
            if (expireRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("deleteAfterDays error: " + expireRet.ToString());
            }
            Console.WriteLine(expireRet.ToString());
        }

        [TestMethod()]
        public void PrefetchTest()
        {
            HttpResult ret = this.bucketManager.Prefetch(Bucket, "qiniu.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("prefetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [TestMethod()]
        public void DomainsTest()
        {
            DomainsResult ret = this.bucketManager.Domains(Bucket);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("domains error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }


        [TestMethod()]
        public void FetchTest()
        {
            string resUrl = "http://devtools.qiniu.com/qiniu.png";
            FetchResult ret = this.bucketManager.Fetch(resUrl, Bucket, "qiniu-fetch.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());

            ret = this.bucketManager.Fetch(resUrl, Bucket, null);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("fetch error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        [TestMethod()]
        public void ListFilesTest()
        {
            string prefix = "";
            string delimiter = "";
            int limit = 100;
            string marker = "";
            ListResult listRet = this.bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("list files error: " + listRet.ToString());
            }
            Console.WriteLine(listRet.ToString());
        }

        [TestMethod()]
        public void ListBucketTest()
        {
            string prefix = "";
            string delimiter = "";
            int limit = 100;
            string marker = "";
            do
            {
                ListResult listRet = this.bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
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
        [TestMethod()]
        public void BatchStatTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.StatOp(Bucket, key);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchDeleteTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.DeleteOp(Bucket, key);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchCopyTest()
        {
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.CopyOp(Bucket, "qiniu.png", Bucket, key, true);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchMoveTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.MoveOp(Bucket, key, Bucket, key + "-batch-move", true);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchChangeMimeTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.ChangeMimeOp(Bucket, key, "image/batch-x-png");
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchChangeTypeTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.ChangeTypeOp(Bucket, key, 0);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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

        [TestMethod()]
        public void BatchDeleteAfterDaysTest()
        {
            BatchCopyTest();
            string[] keys = {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            List<string> ops = new List<string>();
            foreach (string key in keys)
            {
                string op = this.bucketManager.DeleteAfterDaysOp(Bucket, key, 7);
                ops.Add(op);
            }

            BatchResult ret = this.bucketManager.Batch(ops);
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