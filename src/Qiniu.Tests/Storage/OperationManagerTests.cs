using System;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Tests.Storage
{
    public class OperationManagerTests : TestEnv
    {
        [Fact]
        public async Task PfopTest()
        {
            var saveMp4Entry = Base64.UrlSafeBase64Encode(Bucket + ":avthumb_test_target.mp4");
            var saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            var avthumbMp4Fop = "avthumb/mp4|saveas/" + saveMp4Entry;
            var vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;
            var fops = string.Join(";", avthumbMp4Fop, vframeJpgFop);
            var mac = new Mac(AccessKey, SecretKey);
            var config = new Config();
            var manager = new OperationManager(mac, config);
            var pipeline = "sdktest";
            var notifyUrl = "http://api.example.com/qiniu/pfop/notify";
            var key = "qiniu.mp4";
            var force = true;
            var pfopRet = await manager.Pfop(Bucket, key, fops, pipeline, notifyUrl, force);
            if (pfopRet.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "pfop error: " + pfopRet);
            }

            Console.WriteLine(pfopRet.PersistentId);
        }


        [Fact]
        public async Task PrefopTest()
        {
            var persistentId = "z0.59953aaa45a2650c9942144b";
            var mac = new Mac(AccessKey, SecretKey);
            var config = new Config();
            var manager = new OperationManager(mac, config);
            var ret = await manager.Prefop(persistentId);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, "prefop error: " + ret);
            }

            Console.WriteLine(ret.ToString());
        }
    }
}
