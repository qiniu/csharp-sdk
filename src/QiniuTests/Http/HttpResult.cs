using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Qiniu.Http;

namespace QiniuTests.Http
{
    public class NeedRetryDataClass
    {
        private static readonly IReadOnlyList<int> NotRetryableHttpCodes = new List<int>
        {
            (int)HttpCode.INVALID_ARGUMENT,
            (int)HttpCode.INVALID_FILE,
            (int)HttpCode.INVALID_TOKEN,
            (int)HttpCode.USER_CANCELED,
            (int)HttpCode.USER_PAUSED,
            // 服务端
            (int)HttpCode.NOT_IMPLEMENTED,
            (int)HttpCode.BANDWIDTH_LIMIT_EXCEEDED,
            (int)HttpCode.TOO_FREQUENT_ACCESS,
            (int)HttpCode.CALLBACK_FAILED,
            (int)HttpCode.CONTENT_MODIFIED,
            (int)HttpCode.FILE_NOT_EXIST,
            (int)HttpCode.FILE_EXISTS,
            (int)HttpCode.INVALID_SHARE_BUCKET,
            (int)HttpCode.BUCKET_IS_SHARING,
            (int)HttpCode.BUCKET_COUNT_LIMIT,
            (int)HttpCode.BUCKET_NOT_EXIST,
            (int)HttpCode.EXCEED_SHARED_BUCKETS_LIMIT,
            (int)HttpCode.INVALID_MARKER,
            (int)HttpCode.CONTEXT_EXPIRED
        };
        public static IEnumerable TestCases
        {
            get
            {
                for (int i = -5; i < 800; i++)
                {
                    if (i > 0 && i < 500)
                    {
                        yield return new TestCaseData(i).Returns(false);
                        continue;
                    }

                    if (NotRetryableHttpCodes.Contains(i))
                    {
                        yield return new TestCaseData(i).Returns(false);
                        continue;
                    }

                    yield return new TestCaseData(i).Returns(true);
                }
            }
        }
    }
    
    [TestFixture]
    public class HttpResultTests
    {
        [TestCaseSource(typeof(NeedRetryDataClass), nameof(NeedRetryDataClass.TestCases))]
        public bool NeedRetryTest(int code)
        {
            HttpResult httpResult = new HttpResult
            {
                Code = code
            };
            return httpResult.NeedRetry();
        }
    }
}