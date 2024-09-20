using NUnit.Framework;
using System;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class OperationManagerTests :TestEnv
    {
        private OperationManager getOperationManager()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            Config config = new Config();
            // config.UseHttps = true;

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

        [Test]
        public void PfopWithIdleTimeTest()
        {
            string key = "qiniu.mp4";
            bool force = true;
            int type = 1;
            string pipeline = null;
            string saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            string vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;

            OperationManager manager = getOperationManager();
            PfopResult pfopRet = manager.Pfop(Bucket, key, vframeJpgFop, pipeline, null, force, type);
            if (pfopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("pfop error: " + pfopRet.ToString());
            }

            PrefopResult prefopRet = manager.Prefop(pfopRet.PersistentId);
            if (prefopRet.Code != (int)HttpCode.OK)
            {
                Assert.Fail("prefop error: " + prefopRet.ToString());
            }
            Assert.AreEqual(1, prefopRet.Result.Type);
            Assert.IsNotNull(prefopRet.Result.CreationDate);
            Assert.IsNotEmpty(prefopRet.Result.CreationDate);
        }
    }
}
