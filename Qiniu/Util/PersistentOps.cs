using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    /// <summary>
    /// 预处理操作
    /// </summary>
    public abstract class PersistentOps : IPersistentOps
    {
        /// <summary>
        /// 获取操作字符串。
        /// </summary>
        /// <returns>操作字符串。</returns>
        public abstract string GetOpsString();
    }
}
