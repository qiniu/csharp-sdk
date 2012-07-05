using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox
{
    public class DropRet : CallRet
    {
        public DropRet(CallRet ret)
            : base(ret) { }
    }
}
