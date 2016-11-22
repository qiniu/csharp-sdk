using Qiniu.Http;
using System.Collections.Generic;

namespace Qiniu.Storage.Model
{
    /// <summary>
    /// 获取空间文件列表
    /// </summary>
    public class ListFilesResult : HttpResult
    {
        public string Marker { get; set; }
        public List<FileDesc> Items { set; get; }

        public List<string> CommonPrefixes { get; set; }
        public ListFilesResult() { }
    }

    /// <summary>
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
    public class ListFilesResponse
    {
        public string Marker { get; set; }

        public List<FileDesc> Items { get; set; }

        public List<string> CommonPrefixes { get; set; }
    }

    public class FileDesc
    {
        public string Key { get; set; }

        public string Hash { get; set; }

        public long Fsize { get; set; }

        public string MimeType { get; set; }

        public long PutTime { get; set; }
    }

}
