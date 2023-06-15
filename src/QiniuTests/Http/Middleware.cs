using System.Collections.Generic;
using System.Net;
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

        public HttpResult Send(HttpWebRequest req, DNextSend next)
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
            HttpManager httpManager = new HttpManager();

            List<string> orderRecorder = new List<string>();

            List<IMiddleware> middlewares = new List<IMiddleware>
            {
                new RecorderMiddleware(orderRecorder, "A"),
                new RecorderMiddleware(orderRecorder, "B")
            };

            httpManager.Get("https://qiniu.com/index.html", null, null, middlewares);

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
    }
}