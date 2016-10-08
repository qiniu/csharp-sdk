using Qiniu.Http;
using System.Collections.Generic;

namespace Qiniu.Storage.Model
{
    public class BucketsResult : HttpResult
    {
        public List<string> Buckets { set; get; }
        public BucketsResult() { }
    }
}
