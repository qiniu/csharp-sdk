using System.Collections;
using System.Collections.Specialized;
using NUnit.Framework;

namespace QiniuTests.Util
{
    public class CanonicalMimeHeaderKeyDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(":status").Returns(":status");
                yield return new TestCaseData(":x-test-1").Returns(":x-test-1");
                yield return new TestCaseData(":x-Test-2").Returns(":x-Test-2");
                yield return new TestCaseData("content-type").Returns("Content-Type");
                yield return new TestCaseData("CONTENT-LENGTH").Returns("Content-Length");
                yield return new TestCaseData("oRiGin").Returns("Origin");
                yield return new TestCaseData("ReFer").Returns("Refer");
                yield return new TestCaseData("Last-Modified").Returns("Last-Modified");
                yield return new TestCaseData("acCePt-ChArsEt").Returns("Accept-Charset");
                yield return new TestCaseData("x-test-3").Returns("X-Test-3");
                yield return new TestCaseData("cache-control").Returns("Cache-Control");
            }
        }
    }
    
    public class SignatureV2DataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "GET",
                    "http://rs.qbox.me",
                    new StringDictionary
                    {
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    ""
                ).Returns("Qiniu ak:oFWoJ99aQR9W5c2nH--qEuywWA0=");

                yield return new TestCaseData(
                    "GET",
                    "http://rs.qbox.me",
                    new StringDictionary
                    {
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:CK4wBVOL6sLbVE4G4mrXqL_yEc4=");

                yield return new TestCaseData(
                    "GET",
                    "http://rs.qbox.me",
                    new StringDictionary
                    {
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:CK4wBVOL6sLbVE4G4mrXqL_yEc4=");

                yield return new TestCaseData(
                    "GET",
                    "http://rs.qbox.me",
                    new StringDictionary
                    {
                        {"Content-Type", "application/json"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:ksh7bJBnBzFO0yxJ_tLLUcg0csM=");

                yield return new TestCaseData(
                    "POST",
                    "http://rs.qbox.me",
                    new StringDictionary
                    {
                        {"Content-Type", "application/json"},
                        {"X-Qiniu", "b"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:IlW01tHjGQ0pGPXV_3jjR1AdD34=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:156x8Q4x1zadPcAyMRVDsioIyAk=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"Content-Type", "application/json"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:eOaX4RziJPW9ywnJ02jshmEMfhI=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"Content-Type", "application/octet-stream"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:GQQrYvDCdN_RaVjyJC7hIkv5TYk=");

                yield return new TestCaseData(
                    "gET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"Content-Type", "application/json"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"x-qIniu-aAa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "{\"name\": \"test\"}"
                ).Returns("Qiniu ak:eOaX4RziJPW9ywnJ02jshmEMfhI=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"Content-Type", "application/x-www-form-urlencoded"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "name=test&language=go"
                ).Returns("Qiniu ak:A5PMXECSPZQxitJqLj0op2B2GEM=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com",
                    new StringDictionary
                    {
                        {"Content-Type", "application/x-www-form-urlencoded"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"}
                    },
                    "name=test&language=go"
                ).Returns("Qiniu ak:A5PMXECSPZQxitJqLj0op2B2GEM=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com/mkfile/sdf.jpg",
                    new StringDictionary
                    {
                        {"Content-Type", "application/x-www-form-urlencoded"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "name=test&language=go"
                ).Returns("Qiniu ak:fkRck5_LeyfwdkyyLk-hyNwGKac=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com/mkfile/sdf.jpg?s=er3&df",
                    new StringDictionary
                    {
                        {"Content-Type", "application/x-www-form-urlencoded"},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "name=test&language=go"
                ).Returns("Qiniu ak:PUFPWsEUIpk_dzUvvxTTmwhp3p4=");

                yield return new TestCaseData(
                    "GET",
                    "http://upload.qiniup.com/mkfile/sdf.jpg?s=er3&df",
                    new StringDictionary
                    {
                        {"X-Qiniu-Bbb", "BBB"},
                        {"X-Qiniu-Aaa", "DDD"},
                        {"X-Qiniu-", "a"},
                        {"X-Qiniu", "b"}
                    },
                    "name=test&language=go"
                ).Returns("Qiniu ak:Z0kRph7PJaIkPfKbGQVvWQHgOig=");
            }
        }
    }

    public class VerifyRequestDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "",
                    "https://test.qiniu.com/callback",
                    new StringDictionary
                    {
                        {"Authorization", "QBox abcdefghklmnopq:T7F-SjxX7X2zI4Fc1vANiNt1AUE="},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    "name=sunflower.jpg&hash=Fn6qeQi4VDLQ347NiRm-RlQx_4O2&location=Shanghai&price=1500.00&uid=123"
                ).Returns(true);

                yield return new TestCaseData(
                    "GET",
                    "https://test.qiniu.com/callback",
                    new StringDictionary
                    {
                        {"Authorization", "Qiniu abcdefghklmnopq:ZqS7EZuAKrhZaEIxqNGxDJi41IQ="},
                        {"X-Qiniu-Bbb", "BBB"},
                        {"Content-Type", "application/x-www-form-urlencoded"}
                    },
                    "name=sunflower.jpg&hash=Fn6qeQi4VDLQ347NiRm-RlQx_4O2&location=Shanghai&price=1500.00&uid=123"
                ).Returns(true);
            }
        }
    }
}
