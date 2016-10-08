using Qiniu.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Storage.Model
{
    public class DomainsResult : HttpResult
    {
        public List<string> Domains { set; get; }
        public DomainsResult() { }
    }
}