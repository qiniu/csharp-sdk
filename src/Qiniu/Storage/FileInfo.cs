﻿using Newtonsoft.Json;

namespace Qiniu.Storage
{
    /// <summary>
    /// 获取空间文件信息(stat操作)的有效内容
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 文件大小(字节)
        /// </summary>
        [JsonProperty("fsize")]
        public long Fsize { set; get; }

        /// <summary>
        /// 文件hash(ETAG)
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { set; get; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { set; get; }

        /// <summary>
        /// 文件上传时间
        /// </summary>
        [JsonProperty("putTime")]
        public long PutTime { set; get; }

        /// <summary>
        /// 文件存储类型。
        /// 0 标准存储
        /// 1 低频存储
        /// 2 归档存储
        /// 3 深度归档存储
        /// </summary>
        [JsonProperty("type")]
        public int FileType { get; set; }

        /// <summary>
        /// 文件解冻状态。
        /// 1 解冻中
        /// 2 已解冻
        /// 0 如果是归档/深度归档，但处于冻结，后端不返回此字段，因此默认值为 0。请勿依赖 0 判断冻结状态
        /// </summary>
        [JsonProperty("restoreStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int RestoreStatus { get; set; }

        /// <summary>
        /// 文件的存储状态。
        /// 0 启用，非禁用后端不返回此字段，因此默认值为 0。
        /// 1 禁用
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }

        /// <summary>
        /// 文件的 md5 值。
        /// 服务端不确保一定返回此字段，详见：
        /// https://developer.qiniu.com/kodo/1308/stat#:~:text=%E8%AF%A5%E5%AD%97%E6%AE%B5%E3%80%82-,md5,-%E5%90%A6
        /// </summary>
        [JsonProperty("md5", NullValueHandling = NullValueHandling.Ignore)]
        public string Md5 { get; set; }
        
        /// <summary>
        /// 文件过期删除日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonProperty("expiration", NullValueHandling = NullValueHandling.Ignore)]
        public int Expiration { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为低频存储的日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonProperty("transitionToIA", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitionToIa { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为归档直读存储的日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonProperty("transitionToArchiveIR", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitionToArchiveIr { get; set; }

        /// <summary>
        /// 文件生命周期中转为归档存储的日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonProperty("transitionToARCHIVE", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitionToArchive { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为深度归档存储的日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonProperty("transitionToDeepArchive", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitionToDeepArchive { get; set; }
    }
}