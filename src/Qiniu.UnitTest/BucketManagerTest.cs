using NUnit.Framework;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;
using System;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class BucketManagerTest:QiniuTestEnvars
    {
        private Mac mac;
        private Config config;

        [SetUp]
        public void Init()
        {
            this.mac = new Mac(AccessKey, SecretKey);
            this.config = new Config();
        }

        [Test]
        public void StatTest()
        {
            BucketManager target = new BucketManager(mac,config);
            StatResult result = target.Stat(Bucket, "qiniu.png");

            bool cond = (result.Code == (int)HttpCode.OK || 
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST || 
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }
        
        [Test]
        public void CopyTest()
        {
            BucketManager target = new BucketManager(mac,config);
            Random rand = new Random();
            string targetKey = string.Format("CopyTest_{0}.png", rand.Next());
            HttpResult result = target.Copy(Bucket, "qiniu.png", Bucket, targetKey, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void MoveTest()
        {
            BucketManager target = new BucketManager(mac, config);
            Random rand = new Random();
            string copyKey = string.Format("CopyTest_{0}.png", rand.Next());
            string targetKey = string.Format("MoveTest_{0}.png", rand.Next());
            target.Copy(Bucket, "qiniu.png", Bucket, copyKey, true);
            HttpResult result = target.Move(Bucket, copyKey, Bucket, targetKey, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DeleteTest()
        {
            BucketManager target = new BucketManager(mac,config);
            Random rand = new Random();
            string.Format("CopyTest_{0}.png", rand.Next());
          
            string copyKey = string.Format("CopyTest_{0}.png", rand.Next());
            target.Copy(Bucket, "qiniu.png", Bucket, copyKey, true);

            HttpResult result = target.Delete(Bucket, copyKey);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void ChgmTest()
        {
            BucketManager target = new BucketManager(mac, config);

            HttpResult result = target.ChangeMime(Bucket, "qiniu.png", "image/x-png");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Console.WriteLine(result.RefText);
            Assert.IsTrue(cond);
        }

        [Test]
        public void BucketsTest()
        {
            BucketManager target = new BucketManager(mac,config);

            BucketsResult result = target.Buckets(true);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void BatchTest()
        {
            BucketManager target = new BucketManager(mac,config);

            string s1 = target.StatOp(Bucket, "qiniu.png");
            string s2 = target.ChangeMimeOp(Bucket, "qiniu.png", "image/x-png");
            string[] ops = new string[] { s1, s2, "OP-UNDEF" };
            BatchResult result = target.Batch(ops);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BAD_REQUEST||
                result.Code == (int)HttpCode.PARTLY_OK);

            Assert.IsTrue(cond);
        }

        [Test]
        public void ListFilesTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.ListFiles(Bucket, null, null, 100, null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }


        [Test]
        public void FetchTest()
        {
            BucketManager target = new BucketManager(mac,config);
            string fetchUrl = "http://devtools.qiniu.com/qiniu.png";
            HttpResult result = target.Fetch(fetchUrl, Bucket, "qiniu-fetch.png");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_EXISTS);

            Assert.IsTrue(cond);
        }

        [Test]
        public void PrefetchTest()
        {
            BucketManager target = new BucketManager(mac,config);
            HttpResult result = target.Prefetch(Bucket, "qiniu.png");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.BAD_REQUEST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void UpdateLifecycleTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.DeleteAfterDays(Bucket, "qiniu-fetch.png", 1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DomainsTest()
        {
            BucketManager target = new BucketManager(mac,config);
            DomainsResult result = target.Domains(Bucket);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

    }
}
