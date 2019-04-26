using System.Collections.Generic;
using Newtonsoft.Json;
namespace Qiniu.Storage
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
    ///             "putTime":PUT_TIME,
    ///             "type":FILE_TYPE
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ],
    ///     "CmmonPrefixes":"COMMON_PREFIXES"
    /// }
    /// 
    /// </summary>
    public class ListInfoV2
    {
        

        /// <summary>
        /// 文件列表
        /// </summary>
        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public ListItemV2 Item { get; set; }

        /// <summary>
        /// marker标记
        /// </summary>
        [JsonProperty("marker", NullValueHandling = NullValueHandling.Ignore)]
        public string Marker { get; set; }

        /// <summary>
        /// dir
        /// </summary>
        [JsonProperty("dir", NullValueHandling = NullValueHandling.Ignore)]
        public string Dir { get; set; }
    }
}

