using System;
using Qiniu.Http;

namespace Qiniu.Storage
{
    internal class QiniuException : Exception
    {
        public HttpResult HttpResult;

        public QiniuException(HttpResult httpResult, string message) : base(message)
        {
            HttpResult = httpResult ?? new HttpResult();
        }
    }
}
