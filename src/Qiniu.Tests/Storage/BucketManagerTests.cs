using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Tests.Storage
{
    public class BucketManagerTests : TestEnv
    {
        [Fact]
        public async Task BatchChangeMimeTest()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.ChangeMimeOp(Bucket, key, "image/batch-x-png")).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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
        public async Task BatchChangeTypeTestAsync()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.ChangeTypeOp(Bucket, key, 0)).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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
        public async Task BatchCopyTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.CopyOp(Bucket, "qiniu.png", Bucket, key, true)).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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
        public async Task BatchDeleteAfterDaysTest()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
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

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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

        [Fact]
        public async Task BatchDeleteTest()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.DeleteOp(Bucket, key)).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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
        public async Task BatchMoveTest()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.MoveOp(Bucket, key, Bucket, key + "-batch-move", true)).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
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

        // batch stat, delete, copy, move, chtype, chgm, deleteAfterDays
        // 批量操作每次不能超过1000个指令
        [Fact]
        public async Task BatchStatTest()
        {
            await BatchCopyTest();

            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            string[] keys =
            {
                "qiniu-0.png",
                "qiniu-1.png",
                "qiniu-2.png"
            };

            var ops = keys.Select(key => bucketManager.StatOp(Bucket, key)).ToList();

            var ret = await bucketManager.Batch(ops);
            if (ret.Code / 100 != 2)
            {
                Assert.True(false, "batch error: " + ret);
            }

            foreach (var info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine($"{info.Data.MimeType}, {info.Data.PutTime}, {info.Data.Hash}, {info.Data.Fsize}, {info.Data.FileType}");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }

        [Fact]
        public async Task BucketsTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = await bucketManager.Buckets(true);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "buckets error: " + ret);
            }

            foreach (var bucket in ret.Result) Console.WriteLine(bucket);
        }

        [Fact]
        public async Task ChangeMimeTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = await bucketManager.ChangeMime(Bucket, "qiniu.png", "image/x-png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "change mime error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public async Task ChangeTypeTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = await bucketManager.ChangeType(Bucket, "qiniu.png", 1);
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("already in line stat"))
            {
                Assert.True(false, "change type error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public async Task CopyTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = await bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());
        }

        [Fact]
        public async Task DeleteAfterDaysTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = await bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());
            var expireRet = await bucketManager.DeleteAfterDays(Bucket, newKey, 7);
            if (expireRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "deleteAfterDays error: " + expireRet);
            }

            Console.WriteLine(expireRet.ToString());
        }

        [Fact]
        public async Task DeleteTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-delete.png";
            await bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey);
            var deleteRet = await bucketManager.Delete(Bucket, newKey);
            if (deleteRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "delete error: " + deleteRet);
            }
        }

        [Fact]
        public async Task DomainsTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = await bucketManager.Domains(Bucket);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "domains error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }


        [Fact]
        public async Task FetchTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var resUrl = "http://devtools.qiniu.com/qiniu.png";
            var ret = await bucketManager.Fetch(resUrl, Bucket, "qiniu-fetch.png");
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());

            ret = await bucketManager.Fetch(resUrl, Bucket, null);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "fetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public async Task ListBucketTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var prefix = "";
            var delimiter = "";
            var limit = 100;
            var marker = "";
            do
            {
                var listRet = await bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
                if (listRet.Code != (int)HttpCode.OK)
                {
                    Assert.True(false, "list files error: " + listRet);
                }

                Console.WriteLine(listRet.ToString());

                marker = listRet.Result.Marker;
            } while (!string.IsNullOrEmpty(marker));
        }

        [Fact]
        public async Task ListFilesTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var prefix = "";
            var delimiter = "";
            var limit = 100;
            var marker = "";
            var listRet = await bucketManager.ListFiles(Bucket, prefix, marker, limit, delimiter);
            if (listRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "list files error: " + listRet);
            }

            Console.WriteLine(listRet.ToString());
        }

        [Fact]
        public async Task MoveTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var newKey = "qiniu-to-copy.png";
            var copyRet = await bucketManager.Copy(Bucket, "qiniu.png", Bucket, newKey, true);
            if (copyRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "copy error: " + copyRet);
            }

            Console.WriteLine(copyRet.ToString());

            var moveRet = await bucketManager.Move(Bucket, newKey, Bucket, "qiniu-move-target.png", true);
            if (moveRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "move error: " + moveRet);
            }

            Console.WriteLine(moveRet.ToString());
        }

        [Fact]
        public async Task PrefetchTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var ret = await bucketManager.Prefetch(Bucket, "qiniu.png");
            if (ret.Code != (int)HttpCode.OK && !ret.Text.Contains("bucket source not set"))
            {
                Assert.True(false, "prefetch error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }

        [Fact]
        public async Task StatTest()
        {
            var config = new Config { Zone = Zone.ZoneCnEast };
            var mac = new Mac(AccessKey, SecretKey);
            var bucketManager = new BucketManager(mac, config);
            var key = "qiniu.png";
            var statRet = await bucketManager.Stat(Bucket, key);
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
