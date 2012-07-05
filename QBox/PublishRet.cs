using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox
{
    public class PublishRet : CallRet
    {
        public PublishRet(CallRet ret)
            : base(ret) { }
    }
}
