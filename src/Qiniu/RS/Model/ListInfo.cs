using System.Collections.Generic;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间文件(list操作)
    /// 
    /// 返回JSON字符串
    /// 
    /// {
    ///     "marker":"MARKER",
    ///     "items":
    ///     [
    ///         {
    ///             "key":"KEY",
    ///             "hash":"HASH",
    ///             "fsize":FSIZE,
    ///             "mimeType":"MIME_TYPE",
    ///             "putTime":PUT_TIME
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ],
    ///     "CmmonPrefixes":"COMMON_PREFIXES"
    /// }
    /// 
    /// </summary>
    public class ListInfo
    {
        /// <summary>
        /// marker标记
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileDesc> Items { get; set; }

        /// <summary>
        /// 公共前缀
        /// </summary>
        public List<string> CommonPrefixes { get; set; }
    }
}
