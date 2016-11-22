using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Storage.Persistent;
using System.IO;

namespace QiniuTest
{
    /// <summary>
    /// Test class of ResumeUploader
    /// </summary>
    [TestClass]
    public class ResumeUploaderTest
    {
        /// <summary>
        /// get/set
        /// </summary>
        public TestContext Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Test method of BucketManager
        /// </summary>
        [TestMethod]
        public void resumeUploadTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            
            ResumeRecorder recorder = new ResumeRecorder("dir");
            string token = "<token>";
            ResumeUploader target = new ResumeUploader(recorder, "big.record", "F:\\big.dat", "big_ResumeUpload.dat", token, null, null);
            target.uploadFile();
            
        }
    }
}
