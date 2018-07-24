using System;
using System.Collections.Generic;
using Qiniu.Http;
using Qiniu.Tests;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class BucketManagerTests : TestEnv
    {
        [Fact]
        public void BatchChangeMimeTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.ChangeMimeOp(Bucket, key, "image/batch-x-png");
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("chgm success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        [Fact]
        public void BatchChangeTypeTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.ChangeTypeOp(Bucket, key, 0);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("chtype success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        [Fact]
        public void BatchCopyTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.CopyOp(Bucket, "qiniu.png", Bucket, key, true);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("copy success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        [Fact]
        public void BatchDeleteAfterDaysTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.DeleteAfterDaysOp(Bucket, key, 7);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("deleteAfterDays success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        [Fact]
        public void BatchDeleteTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.DeleteOp(Bucket, key);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("delete success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        [Fact]
        public void BatchMoveTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.MoveOp(Bucket, key, Bucket, key + "-batch-move", true);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("move success");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }

        // batch stat, delete, copy, move, chtype, chgm, deleteAfterDays
        // 批量操作每次不能超过1000个指令
        [Fact]
        public void BatchStatTest()
        {
            BatchCopyTest();

            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = new List<string>();
            foreach (var key in keys)
            {
                var op = bucketManager.StatOp(Bucket, key);
                ops.Add(op);
            }

            var ret = bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine(
                        "{0}, {1}, {2}, {3}, {4}",
                        info.Data.MimeType,
                        info.Data.PutTime,
                        info.Data.Hash,
                        info.Data.Fsize,
                        info.Data.FileType);
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
        }


        [Fact]
        public void BucketsTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = bucketManager.Buckets(true);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "buckets error: " + ret);
            }

            foreach (var bucket in ret.Result) Console.WriteLine(bucket);
        }

        [Fact]
        public void ChangeMimeTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = bucketManager.ChangeMime(Bucket, "qiniu.png", "image/x-png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "change mime error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void ChangeTypeTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = bucketManager.ChangeType(Bucket, "qiniu.png", 1);
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("already in line stat"))
            {
                Assert.True(false, "change type error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void CopyTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());
        }

        [Fact]
        public void DeleteAfterDaysTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());
            var expireRet = bucketManager.DeleteAfterDays(Bucket, newKey, 7);
            if (expireRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "deleteAfterDays error: " + expireRet);
            }

            Console.WriteLine(expireRet.ToString());
        }

        [Fact]
        public void DeleteTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-delete.png";
            bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey);
            var deleteRet = bucketManager.Delete(Bucket, newKey);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "delete error: " + deleteRet);
            }
        }

        [Fact]
        public void DomainsTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = bucketManager.Domains(Bucket);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "domains error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }


        [Fact]
        public void FetchTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var resUrl = "http://devtools.qiniu.com/qiniu.png";
            var ret = bucketManager.Fetch(resUrl, Bucket, "qiniu-fetch.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());

            ret = bucketManager.Fetch(resUrl, Bucket, null);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void ListBucketTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var prefix = "";
            var delimiter = "";
            var limit = 100;
            var marker = "";
            do
            {
                var listRet = bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
                if (listRet.Code != (int)HttpCode.OK)
                {
                    Assert.True(false, "list files error: " + listRet);
                }

                Console.WriteLine(listRet.ToString());

                marker = listRet.Result.Marker;
            } while (!string.IsNullOrEmpty(marker));
        }

        [Fact]
        public void ListFilesTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var prefix = "";
            var delimiter = "";
            var limit = 100;
            var marker = "";
            var listRet = bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "list files error: " + listRet);
            }

            Console.WriteLine(listRet.ToString());
        }

        [Fact]
        public void MoveTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());

            var moveRet = bucketManager.Move(Bucket, newKey, Bucket, "qiniu-move-target.png", true);
            if (moveRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "move error: " + moveRet);
            }

            Console.WriteLine(moveRet.ToString());
        }

        [Fact]
        public void PrefetchTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = bucketManager.Prefetch(Bucket, "qiniu.png");
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("bucket source not set"))
            {
                Assert.True(false, "prefetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public void StatTest()
        {
            var config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var key = "qiniu.png";
            var statRet = bucketManager.Stat(Bucket, key);
            if (statRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "stat error: " + statRet);
            }

            Console.WriteLine(statRet.Result.Hash);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.Fsize);
            Console.WriteLine(statRet.Result.MimeType);
            Console.WriteLine(statRet.Result.FileType);
        }
    }
}
