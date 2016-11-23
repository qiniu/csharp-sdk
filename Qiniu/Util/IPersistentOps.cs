using System;
namespace Qiniu.Util
{
    /// <summary>
    /// 表示该对象是一个预处理操作。
    /// </summary>
    public interface IPersistentOps
    {
        /// <summary>
        /// 获取操作字符串。
        /// </summary>
        /// <returns>操作字符串。</returns>
        string GetOpsString();
    }
}
