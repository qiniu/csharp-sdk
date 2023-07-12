using System.Collections.Generic;
using NUnit.Framework;
using Qiniu.Http;

namespace QiniuTests.Http
{
    class RecorderMiddleware : IMiddleware
    {
        private readonly List<string> _orderRecorder;

        private readonly string _label;

        public RecorderMiddleware(List<string> orderRecorder, string label)
        {
            _orderRecorder = orderRecorder;
            _label = label;
        }

        public HttpResult Send(HttpRequestOptions req, DNextSend next)
        {
            _orderRecorder.Add("bef_" + _label + _orderRecorder.Count);
            HttpResult result = next(req);
            _orderRecorder.Add("aft_" + _label + _orderRecorder.Count);
            return result;
        }
    }

    [TestFixture]
    public class MiddlewareTests
    {
        [Test]
        public void SendWithMiddlewareTest()
        {
            HttpManager httpManager = new HttpManager(true);

            List<string> orderRecorder = new List<string>();

            List<IMiddleware> middlewares = new List<IMiddleware>
            {
                new RecorderMiddleware(orderRecorder, "A"),
                new RecorderMiddleware(orderRecorder, "B")
            };

            HttpResult resp = httpManager.Get("https://qiniu.com/index.html", null, null, middlewares);

            Assert.AreEqual((int)HttpCode.OK, resp.Code, resp.ToString());
            CollectionAssert.AreEqual(
                new List<string>
                {
                    "bef_A0",
                    "bef_B1",
                    "aft_B2",
                    "aft_A3"
                },
                orderRecorder
            );
        }
        
        [Test]
        public void RetryDomainsMiddlewareTest()
        {
            
            HttpManager httpManager = new HttpManager(true);

            List<string> orderRecorder = new List<string>();

            List<IMiddleware> middlewares = new List<IMiddleware>
            {
                new RetryDomainsMiddleware(
                    new List<string>
                    {
                        "unavailable.csharpsdk.qiniu.com",
                        "qiniu.com"
                    },
                    3
                ),
                new RecorderMiddleware(orderRecorder, "A")
            };

            HttpResult resp = httpManager.Get("https://fake.csharpsdk.qiniu.com/index.html", null, null, middlewares);

            Assert.AreEqual((int)HttpCode.OK, resp.Code, resp.ToString());

            CollectionAssert.AreEqual(
                new List<string>
                {
                    // fake.csharpsdk.qiniu.com
                    "bef_A0",
                    "aft_A1",
                    "bef_A2",
                    "aft_A3",
                    "bef_A4",
                    "aft_A5",
                    // unavailable.csharpsdk.qiniu.com
                    "bef_A6",
                    "aft_A7",
                    "bef_A8",
                    "aft_A9",
                    "bef_A10",
                    "aft_A11",
                    // qiniu.com
                    "bef_A12",
                    "aft_A13"
                },
                orderRecorder
            );
        }
    }
}