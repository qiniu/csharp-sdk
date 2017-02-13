using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.RS;
using Qiniu.RS.Model;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class BucketManagerTest:QiniuTestEnvars
    {
        [Test]
        public void StatTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);
            StatResult result = target.Stat(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK || 
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST || 
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }
        
        [Test]
        public void CopyTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Copy(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void MoveTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Move(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DeleteTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Delete(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void ChgmTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Chgm(Bucket1, FileKey1, "MimeType");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void BucketTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            BucketResult result = target.Bucket(Bucket1);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void BucketsTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            BucketsResult result = target.Buckets();

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void BatchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

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
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.ListFiles(Bucket1, null, null, 100, null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

#if LOCAL_TEST

        [Test]
        public void FetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Fetch(TestURL1, Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_EXISTS);

            Assert.IsTrue(cond);
        }
#endif

        [Test]
        public void PrefetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.Prefetch(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.BAD_REQUEST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void UpdateLifecycleTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = target.UpdateLifecycle(Bucket1, FileKey1, 1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [Test]
        public void DomainsTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            DomainsResult result = target.Domains(Bucket1);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

    }
}
