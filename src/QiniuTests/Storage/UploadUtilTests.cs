using NUnit.Framework;
using System.Collections;
using Qiniu.Tests;

namespace Qiniu.Storage.Tests
{
    public class UploadUtilTestCases
    {
        public static IEnumerable ShouldRetryTestCases
        {
            get
            {
                // 200 always false
                yield return new TestCaseData(200, 200).Returns(false);
                
                // some refCode should retry
                yield return new TestCaseData(0, 3).Returns(true);
                yield return new TestCaseData(0, 0).Returns(true);
                
                // 4xx shouldn't retry but 406
                yield return new TestCaseData(406, 406).Returns(true);
                yield return new TestCaseData(400, 400).Returns(false);
                
                // some code shouldn't retry
                yield return new TestCaseData(612, 612).Returns(false);
                yield return new TestCaseData(614, 614).Returns(false);
                yield return new TestCaseData(579, 579).Returns(false);
                
                // any others should retry
                yield return new TestCaseData(500, 500).Returns(true);
                yield return new TestCaseData(502, 502).Returns(true);
            }
        }
    }

    [TestFixture]
    public class UploadUtilTests : TestEnv
    {
        [TestCaseSource(typeof(UploadUtilTestCases), nameof(UploadUtilTestCases.ShouldRetryTestCases))]
        public bool ShouldRetryTest(int code, int refCode)
        {
            return UploadUtil.ShouldRetry(code, refCode);
        }
    }
}