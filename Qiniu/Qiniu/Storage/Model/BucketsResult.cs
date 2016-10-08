using Qiniu.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Storage.Model
{
    public class BucketsResult : HttpResult
    {
        public List<string> Buckets { set; get; }
        public BucketsResult() { }
    }
}
