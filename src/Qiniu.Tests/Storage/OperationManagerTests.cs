using System;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.Tests;
using Xunit;

namespace Qiniu.Storage.Tests
{
    public class OperationManagerTests :TestEnv
    {

        [Fact]
        public void PfopTest()
        {
            string saveMp4Entry = Base64.UrlSafeBase64Encode(Bucket + ":avthumb_test_target.mp4");
            string saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            string avthumbMp4Fop = "avthumb/mp4|saveas/" + saveMp4Entry;
            string vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;
            string fops = string.Join(";", new string[] { avthumbMp4Fop, vframeJpgFop });
            Mac mac = new Mac(AccessKey, SecretKey);
            Config config = new Config();
            OperationManager manager = new OperationManager(mac, config);
            string pipeline = "sdktest";
            string notifyUrl = "http://api.example.com/qiniu/pfop/notify";
            string key = "qiniu.mp4";
            bool force = true;
            PfopResult pfopRet = manager.Pfop(Bucket, key, fops, pipeline, notifyUrl, force);
            if (pfopRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "pfop error: " + pfopRet.ToString());
            }
            Console.WriteLine(pfopRet.PersistentId);
        }


        [Fact]
        public void PrefopTest()
        {
            string persistentId = "z0.59953aaa45a2650c9942144b";
            Mac mac = new Mac(AccessKey, SecretKey);
            Config config = new Config();
            OperationManager manager = new OperationManager(mac, config);
            PrefopResult ret = manager.Prefop(persistentId);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "prefop error: " + ret.ToString());
            }
            Console.WriteLine(ret.ToString());
        }

    }
}
