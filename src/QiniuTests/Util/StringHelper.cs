using System.Collections;
using NUnit.Framework;

using Qiniu.Util;
using Qiniu.Tests;

namespace QiniuTests.Util
{
    [TestFixture]
    public class StringHelperTests : TestEnv
    {
        [TestCaseSource(typeof(CanonicalMimeHeaderKeyDataClass), nameof(CanonicalMimeHeaderKeyDataClass.TestCases))]
        public string CanonicalMimeHeaderKeyTest(string fieldName)
        {
                return StringHelper.CanonicalMimeHeaderKey(fieldName);
        }
    }
}
