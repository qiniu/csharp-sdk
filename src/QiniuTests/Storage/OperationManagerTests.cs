using NUnit.Framework;
using System;
using System.Collections;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class OperationManagerTests : TestEnv
    {
        private OperationManager getOperationManager()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Config config = new Config();
            config.UseHttps = true;

            OperationManager manager = new OperationManager(mac, config);
            return manager;
        }

        [Test]
        public void PfopAndPrefopTest()
        {
            string key = "qiniu.mp4";
            bool force = true;
            string pipeline = "sdktest";
            string notifyUrl = "http://api.example.com/qiniu/pfop/notify";
            string saveMp4Entry = Base64.UrlSafeBase64Encode(Bucket + ":avthumb_test_target.mp4");
            string saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            string avthumbMp4Fop = "avthumb/mp4|saveas/" + saveMp4Entry;
            string vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;
            string fops = string.Join(";", new string[] { avthumbMp4Fop, vframeJpgFop });

            OperationManager manager = getOperationManager();
            PfopResult pfopRet = manager.Pfop(Bucket, key, fops, pipeline, notifyUrl, force);
            if (pfopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("pfop error: " + pfopRet.ToString());
            }
            Console.WriteLine(pfopRet.PersistentId);

            PrefopResult ret = manager.Prefop(pfopRet.PersistentId);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail("prefop error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

        public static IEnumerable PfopOptionsTestCases
        {
            get
            {
                yield return new TestCaseData(
                    0, // type
                    null // workflow template id
                );
                yield return new TestCaseData(
                    1,
                    null
                );
                yield return new TestCaseData(
                    0,
                    "test-workflow"
                );
            }
        }

        [TestCaseSource(typeof(OperationManagerTests), nameof(PfopOptionsTestCases))]
        public void PfopWithOptionsTest(int type, string workflowId)
        {
            string bucketName = Bucket;
            string key = "qiniu.mp4";

            StringBuilder persistentKeyBuilder = new StringBuilder("test-pfop/test-pfop-by-api");
            if (type > 0)
            {
                persistentKeyBuilder.Append("type_" + type);
            }

            string fops;
            if (!string.IsNullOrEmpty(workflowId))
            {
                fops = null;
            }
            else
            {
                string saveEntry = Base64.UrlSafeBase64Encode(String.Join(
                    ":",
                    bucketName,
                    persistentKeyBuilder.ToString()
                ));
                fops = "avinfo|saveas/" + saveEntry;
            }

            OperationManager manager = getOperationManager();
            PfopResult pfopRet = manager.Pfop(
                Bucket,
                key,
                fops,
                null,
                null,
                true,
                type,
                workflowId
            );
            if (pfopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("pfop error: " + pfopRet);
            }

            PrefopResult prefopRet = manager.Prefop(pfopRet.PersistentId);
            if (prefopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("prefop error: " + prefopRet);
            }

            Assert.IsNotNull(prefopRet.Result.CreationDate);
            Assert.IsNotEmpty(prefopRet.Result.CreationDate);

            if (type == 1)
            {
                Assert.AreEqual(1, prefopRet.Result.Type);
            }

            if (!string.IsNullOrEmpty(workflowId))
            {
                Assert.IsNotNull(prefopRet.Result.TaskFrom);
                Assert.IsNotEmpty(prefopRet.Result.TaskFrom);
                Assert.IsTrue(prefopRet.Result.TaskFrom.Contains(workflowId));
            }
        }
    }
}
