using System;
using System.Collections.Generic;

namespace Qiniu.IO
{
    public enum CheckCrcType
    {
        /// <summary>
        /// default
        /// </summary>
        DEFAULT_CHECK = -1,
        /// <summary>
        /// 表示不进行 crc32 校验
        /// </summary>
        NO_CHECK = 0,
        /// <summary>
        ///对于 Put 等同于 CheckCrc = 2；对于 PutFile 会自动计算 crc32 值
        /// </summary>
        CHECK_AUTO = 1,
        /// <summary>
        /// 表示进行 crc32 校验，且 crc32 值就是PutExtra:Crc32
        /// </summary>
        CHECK = 2
    }
    /// <summary>
    /// 
    /// </summary>
    public class PutExtra
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Crc32 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CheckCrcType CheckCrc { get; set; }

        public string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PutExtra()
        {
            Crc32 = -1;
        }

    }
}
