using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Http;

namespace Qiniu.Processing
{
    public class DfopResult
    {
        public ResponseInfo ResponseInfo { get; set; }

        public byte[] ResponseData { get; set; }

        public string Response()
        {
            return Encoding.UTF8.GetString(ResponseData);
        }
    }
}
