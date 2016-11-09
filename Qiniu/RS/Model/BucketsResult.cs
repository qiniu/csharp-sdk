using System.Collections.Generic;
using Qiniu.Http;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间列表(buckets操作)的返回消息
    /// </summary>
    public class BucketsResult:HttpResult
    {
        public List<string> Buckets { get; set; }
    }
}
