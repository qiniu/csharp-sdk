using Qiniu.Http;
using System.Collections.Generic;

namespace Qiniu.Storage.Model
{
    public class DomainsResult : HttpResult
    {
        public List<string> Domains { set; get; }
        public DomainsResult() { }
    }
}