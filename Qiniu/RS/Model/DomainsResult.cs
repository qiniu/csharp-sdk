using System.Collections.Generic;
using Qiniu.Http;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间域名(domains操作)的返回消息
    /// </summary>
    public class DomainsResult:HttpResult
    {
        public List<string> Domains { get; set; }
    }
}
