using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox
{
    public class DeleteRet : CallRet
    {
        public DeleteRet(CallRet ret)
            : base(ret) { }
    }
}
