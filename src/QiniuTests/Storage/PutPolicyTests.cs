using System;
using NUnit.Framework;
using Qiniu.Tests;
using Qiniu.Util;

namespace Qiniu.Storage.Tests
{
    [TestFixture]
    public class PutPolicyTests : TestEnv
    {
        [Test]
        public void SimplePutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;

            string expectedScope = string.Format("\"scope\":\"{0}\"", Bucket);
            string deadlineSubstring = DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds().ToString();
            // discard the precision to the second level
            deadlineSubstring = deadlineSubstring.Substring(0, deadlineSubstring.Length - 1);
            string expectedDeadline = string.Format("\"deadline\":{0}", deadlineSubstring);
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedScope));
            Assert.That(actual, Does.Contain(expectedDeadline));
        }

        [Test]
        public void InsertOnlyPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = String.Format("{0}:{1}", Bucket, "key/of/file");
            putPolicy.InsertOnly = 1;

            string expectedScope = String.Format("\"scope\":\"{0}:{1}\"", Bucket, "key/of/file");
            string expectedInsertOnly = "\"insertOnly\":1";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedScope));
            Assert.That(actual, Does.Contain(expectedInsertOnly));
        }

        [Test]
        public void IsPrefixalScopePutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = String.Format("{0}:{1}", Bucket, "key/of/prefix-");
            putPolicy.isPrefixalScope = 1;

            string expectedScope = String.Format("\"scope\":\"{0}:{1}\"", Bucket, "key/of/prefix-");
            string expectedIsPrefixalScope = "\"isPrefixalScope\":1";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedScope));
            Assert.That(actual, Does.Contain(expectedIsPrefixalScope));
        }

        [Test]
        public void ExpiresPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.SetExpires(7200);

            string deadlineSubstring = DateTimeOffset.UtcNow.AddSeconds(7200).ToUnixTimeSeconds().ToString();
            // discard the precision to the second level
            deadlineSubstring = deadlineSubstring.Substring(0, deadlineSubstring.Length - 1);
            string expectedDeadline = string.Format("\"deadline\":{0}", deadlineSubstring);
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedDeadline));
        }

        [Test]
        public void ForceSavaKeyPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.ForceSaveKey = true;
            putPolicy.SaveKey = "key/of/file";

            string expectedForceSaveKey = "\"forceSaveKey\":true";
            string expectedSaveKey = "\"saveKey\":\"key/of/file\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedForceSaveKey));
            Assert.That(actual, Does.Contain(expectedSaveKey));
        }

        [Test]
        public void OverwritePutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            string keyToOverwrite = "qiniu.png";
            putPolicy.Scope = string.Format("{0}:{1}", Bucket, keyToOverwrite);

            string expectedScope = String.Format("\"scope\":\"{0}:{1}\"", Bucket, keyToOverwrite);
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedScope));
        }

        [Test]
        public void CustomReturnBodyPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.ReturnBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";

            string expectedReturnBody = "\"returnBody\":\"{\\\"key\\\":\\\"$(key)\\\",\\\"hash\\\":\\\"$(etag)\\\",\\\"fsiz\\\":$(fsize),\\\"bucket\\\":\\\"$(bucket)\\\",\\\"name\\\":\\\"$(x:name)\\\"}\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedReturnBody));
        }

        [Test]
        public void CustomReturnUrlPutPolicyTest()
        {
            
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.ReturnUrl = "http://api.example.com/qiniu/upload/return";

            string expectedReturnBody = "\"returnUrl\":\"http://api.example.com/qiniu/upload/return\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedReturnBody));
        }

        [Test]
        public void CallbackJsonPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.CallbackUrl = "http://api.example.com/qiniu/upload/callback";
            putPolicy.CallbackBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";
            putPolicy.CallbackBodyType = "application/json";

            string expectedCallbackUrl = "\"callbackUrl\":\"http://api.example.com/qiniu/upload/callback\"";
            string expectedCallbackBody = "\"callbackBody\":\"{\\\"key\\\":\\\"$(key)\\\",\\\"hash\\\":\\\"$(etag)\\\",\\\"fsiz\\\":$(fsize),\\\"bucket\\\":\\\"$(bucket)\\\",\\\"name\\\":\\\"$(x:name)\\\"}\"";
            string expectedCallbackBodyType = "\"callbackBodyType\":\"application/json\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedCallbackUrl));
            Assert.That(actual, Does.Contain(expectedCallbackBody));
            Assert.That(actual, Does.Contain(expectedCallbackBodyType));
        }

        [Test]
        public void CallbackUrlEncodedPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.CallbackUrl = "http://api.example.com/qiniu/upload/callback";
            putPolicy.CallbackBody = "key=$(key)&hash=$(etag)&bucket=$(bucket)&fsize=$(fsize)&name=$(x:name)";
            putPolicy.CallbackHost = "api.example.com";

            string expectedCallbackUrl = "\"callbackUrl\":\"http://api.example.com/qiniu/upload/callback\"";
            string expectedCallbackBody = "\"callbackBody\":\"key=$(key)&hash=$(etag)&bucket=$(bucket)&fsize=$(fsize)&name=$(x:name)\"";
            string expectedCallbackHost = "\"callbackHost\":\"api.example.com\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedCallbackUrl));
            Assert.That(actual, Does.Contain(expectedCallbackBody));
            Assert.That(actual, Does.Contain(expectedCallbackHost));
        }

        [Test]
        public void CallbackFetchKeyPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.CallbackUrl = "http://api.example.com/qiniu/upload/callback";
            putPolicy.CallbackFetchKey = 1;
            putPolicy.CallbackBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";

            string expectedCallbackUrl = "\"callbackUrl\":\"http://api.example.com/qiniu/upload/callback\"";
            string expectedCallbackFetchKey = "\"callbackFetchKey\":1";
            string expectedCallbackBody = "\"callbackBody\":\"{\\\"key\\\":\\\"$(key)\\\",\\\"hash\\\":\\\"$(etag)\\\",\\\"fsiz\\\":$(fsize),\\\"bucket\\\":\\\"$(bucket)\\\",\\\"name\\\":\\\"$(x:name)\\\"}\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedCallbackUrl));
            Assert.That(actual, Does.Contain(expectedCallbackFetchKey));
            Assert.That(actual, Does.Contain(expectedCallbackBody));
        }

        [Test]
        public void DataProcessPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            string saveMp4Entry = Base64.UrlSafeBase64Encode(Bucket + ":avthumb_test_target.mp4");
            string saveJpgEntry = Base64.UrlSafeBase64Encode(Bucket + ":vframe_test_target.jpg");
            string avthumbMp4Fop = "avthumb/mp4|saveas/" + saveMp4Entry;
            string vframeJpgFop = "vframe/jpg/offset/1|saveas/" + saveJpgEntry;
            string fops = string.Join(";", new string[] { avthumbMp4Fop, vframeJpgFop });
            putPolicy.Scope = Bucket;
            putPolicy.PersistentOps = fops;
            putPolicy.PersistentPipeline = "video-pipe";
            putPolicy.PersistentNotifyUrl = "http://api.example.com/qiniu/pfop/notify";

            string expectedPersistentOps = string.Format("\"persistentOps\":\"{0}\"", fops);
            string expectedPersistentPipeline = "\"persistentPipeline\":\"video-pipe\"";
            string expectedPersistentNotifyUrl = "\"persistentNotifyUrl\":\"http://api.example.com/qiniu/pfop/notify\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedPersistentOps));
            Assert.That(actual, Does.Contain(expectedPersistentPipeline));
            Assert.That(actual, Does.Contain(expectedPersistentNotifyUrl));
        }

        [Test]
        public void EndUserPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.EndUser = "mock_end_user";

            string expectedEndUser = "\"endUser\":\"mock_end_user\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedEndUser));
        }

        [Test]
        public void FileLimitPutPolicyTest()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.FsizeMin = 1024; // 1KB
            putPolicy.FsizeLimit = 1024 * 1024; // 1MB
            putPolicy.DetectMime = 1;
            putPolicy.MimeLimit = "image/*";

            string expectedFsizeMin = "\"fsizeMin\":1024";
            string expectedFsizeLimit = "\"fsizeLimit\":1048576";
            string expectedDetectMime = "\"detectMime\":1";
            string expectedMimeLimit = "\"mimeLimit\":\"image/*\"";
            string actual = putPolicy.ToJsonString();

            Assert.That(actual, Does.Contain(expectedFsizeMin));
            Assert.That(actual, Does.Contain(expectedFsizeLimit));
            Assert.That(actual, Does.Contain(expectedDetectMime));
            Assert.That(actual, Does.Contain(expectedMimeLimit));
        }
    }
}