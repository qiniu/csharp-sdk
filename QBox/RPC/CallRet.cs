﻿using System;
using System.Net;

namespace QBox.RPC
{
    public class CallRet
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public Exception Exception { get; protected set; }
        public string Response { get; protected set; }
        public bool OK { get { return (int)StatusCode / 100 == 2; } }

        public CallRet(HttpStatusCode statusCode, string response)
        {
            StatusCode = statusCode;
            Response = response;
        }

        public CallRet(HttpStatusCode statusCode, Exception e)
        {
            StatusCode = statusCode;
            Exception = e;
        }

        public CallRet(CallRet ret)
        {
            StatusCode = ret.StatusCode;
            Exception = ret.Exception;
            Response = ret.Response;
        }
    }
}
