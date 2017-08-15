using NUnit.Framework;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;

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
            StatResult result = target.Stat(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK || 
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST || 
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }
        
        [Test]
        public void CopyTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.Copy(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void MoveTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.Move(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DeleteTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.Delete(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void ChgmTest()
        {
            BucketManager target = new BucketManager(mac, config);

            HttpResult result = target.Chgm(Bucket1, FileKey1, "MimeType");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

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

            string s1 = target.StatOp(Bucket1, FileKey1);
            string s2 = target.ChgmOp(Bucket2, FileKey2, "MimeType");
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

            HttpResult result = target.ListFiles(Bucket1, null, null, 100, null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }


        [Test]
        public void FetchTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.Fetch(TestURL1, Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_EXISTS);

            Assert.IsTrue(cond);
        }

        [Test]
        public void PrefetchTest()
        {
            BucketManager target = new BucketManager(mac,config);
            HttpResult result = target.Prefetch(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.BAD_REQUEST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void UpdateLifecycleTest()
        {
            BucketManager target = new BucketManager(mac,config);

            HttpResult result = target.UpdateLifecycle(Bucket1, FileKey1, 1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DomainsTest()
        {
            BucketManager target = new BucketManager(mac,config);

            DomainsResult result = target.Domains(Bucket1);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

    }
}
