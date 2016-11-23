using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Util;

namespace Qiniu.Test
{
    [TestClass]
    public class PutPolicyExtensionTest
    {
        [TestMethod]
        public void TestWithScope()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.Scope = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithScope(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDeadline()
        {
            var value = new Random().Next(1, 10000);
            var policy = new PutPolicy();
            policy.Deadline = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDeadline(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDeadline_II()
        {
            var value = new Random().Next(1, 10000);
            var policy = new PutPolicy();
            policy.SetExpires(value);
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDeadline(TimeSpan.FromSeconds(value)).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithInsertOnly()
        {
            var policy = new PutPolicy();
            policy.InsertOnly = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithInsertOnly(true).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithInsertOnly_Default()
        {
            var policy = new PutPolicy();
            policy.InsertOnly = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithInsertOnly().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithInsertOnly_False()
        {
            var policy = new PutPolicy();
            policy.InsertOnly = 0;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithInsertOnly(false).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithEndUser()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.EndUser = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithEndUser(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithReturnUrl()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.ReturnUrl = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithReturnUrl(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithReturnBody()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.ReturnBody = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithReturnBody(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackUrl()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.CallbackUrl = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackUrl(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackHost()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.CallbackHost = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackHost(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackBody()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.CallbackBody = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackBody(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackBodyType()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.CallbackBodyType = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackBodyType(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackBodyType_Default()
        {
            var value = "application/x-www-form-urlencoded";
            var policy = new PutPolicy();
            policy.CallbackBodyType = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackBodyType().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackFetchKey()
        {
            var policy = new PutPolicy();
            policy.CallbackFetchKey = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackFetchKey(true).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackFetchKey_Default()
        {
            var policy = new PutPolicy();
            policy.CallbackFetchKey = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackFetchKey().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithCallbackFetchKey_False()
        {
            var policy = new PutPolicy();
            policy.CallbackFetchKey = 0;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithCallbackFetchKey(false).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentOps()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.PersistentOps = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentOps(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentOps_NULL()
        {
            var policy = new PutPolicy();
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentOps().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentOps_SINGLE()
        {
            var policy = new PutPolicy();
            policy.PersistentOps = "avthumb/mp3";
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentOps(new Models.PersistentOpsAvthumbTest("mp3")).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentOps_DOUBLE()
        {
            var policy = new PutPolicy();
            policy.PersistentOps = "avthumb/mp3;avthumb/mp4";
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentOps(new Models.PersistentOpsAvthumbTest("mp3"), new Models.PersistentOpsAvthumbTest("mp4")).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentNotifyUrl()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.PersistentNotifyUrl = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentNotifyUrl(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithPersistentPipeline()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.PersistentPipeline = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithPersistentPipeline(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithSaveKey()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.SaveKey = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithSaveKey(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithFsizeMin()
        {
            var value = new Random().Next(1, 10000);
            var policy = new PutPolicy();
            policy.FsizeMin = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithFsizeMin(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithFsizeLimit()
        {
            var value = new Random().Next(1, 10000);
            var policy = new PutPolicy();
            policy.FsizeLimit = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithFsizeLimit(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDetectMime()
        {
            var policy = new PutPolicy();
            policy.DetectMime = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDetectMime(true).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDetectMime_Default()
        {
            var policy = new PutPolicy();
            policy.DetectMime = 1;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDetectMime().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDetectMime_False()
        {
            var policy = new PutPolicy();
            policy.DetectMime = 0;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDetectMime(false).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithMimeLimit()
        {
            var value = Guid.NewGuid().ToString();
            var policy = new PutPolicy();
            policy.MimeLimit = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithMimeLimit(value).ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithDeleteAfterDays()
        {
            var value = new Random().Next(1, 10000);
            var policy = new PutPolicy();
            policy.DeleteAfterDays = value;
            var expected = policy.ToString();
            var actual = new PutPolicy().WithDeleteAfterDays(value).ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
