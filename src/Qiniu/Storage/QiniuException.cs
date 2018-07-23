using System;
using Qiniu.Http;

namespace Qiniu.Storage
{
    internal class QiniuException : Exception
    {
        public HttpResult HttpResult;
        public string message;

        public QiniuException(HttpResult httpResult, string message)
        {
            HttpResult = httpResult == null ? new HttpResult() : httpResult;
            this.message = message;
        }

        public override string Message => message;
    }
}
