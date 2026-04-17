using System;

namespace Qiniu.Util
{
    /// <summary>
    /// QINIU ETAG(文件hash)
    /// </summary>
    public class QETag
    {
        /// <summary>
        /// 计算文件hash(ETAG)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>文件hash</returns>
        [Obsolete("Use CalcHash instead.")]
        public static string calcHash(string filePath)
        {
            return CalcHash(filePath);
        }

        /// <summary>
        /// 计算文件hash(ETAG)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件hash</returns>
        public static string CalcHash(string filePath)
        {
            return ETag.CalcHash(filePath);
        }
    }
}