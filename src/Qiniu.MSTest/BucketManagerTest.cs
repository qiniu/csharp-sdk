using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.RS;
using Qiniu.RS.Model;

namespace Qiniu.UnitTest
{
    [TestClass]
    public class BucketManagerTest:QiniuTestEnvars
    {
        [TestMethod]
        public async Task StatTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);
            StatResult result = await target.StatAsync(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK || 
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST || 
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }
        
        [TestMethod]
        public async Task CopyTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.CopyAsync(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task MoveTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.MoveAsync(Bucket1, FileKey1, Bucket2, FileKey2, true);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.DeleteAsync(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task ChgmTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.ChgmAsync(Bucket1, FileKey1, "MimeType");

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task BucketTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            BucketResult result = await target.BucketAsync(Bucket1);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task BucketsTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            BucketsResult result = await target.BucketsAsync();

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task BatchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            string s1 = target.StatOp(Bucket1, FileKey1);
            string s2 = target.ChgmOp(Bucket2, FileKey2, "MimeType");
            string[] ops = new string[] { s1, s2, "OP-UNDEF" };
            BatchResult result = await target.BatchAsync(ops);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BAD_REQUEST||
                result.Code == (int)HttpCode.PARTLY_OK);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task ListFilesTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.ListAsync(Bucket1, null, null, 100, null);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

#if LOCAL_TEST

        [TestMethod]
        public async Task FetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.FetchAsync(TestURL1, Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_EXISTS);

            Assert.IsTrue(cond);
        }
#endif

        [TestMethod]
        public async Task PrefetchTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.PrefetchAsync(Bucket1, FileKey1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.BAD_REQUEST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task UpdateLifecycleTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            HttpResult result = await target.UpdateLifecycleAsync(Bucket1, FileKey1, 1);

            bool cond = (result.Code == (int)HttpCode.OK ||
                result.Code == (int)HttpCode.BUCKET_NOT_EXIST ||
                result.Code == (int)HttpCode.FILE_NOT_EXIST);

            Assert.IsTrue(cond);
        }

        [TestMethod]
        public async Task DomainsTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager target = new BucketManager(mac);

            DomainsResult result = await target.DomainsAsync(Bucket1);

            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

    }
}
