using System.Text.Json.Serialization;
namespace Qiniu.Storage
{
    /// <summary>
    /// 批量处理返回的信息
    /// </summary>
    public class BatchInfo
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BatchData Data { get; set; }
    }

    /// <summary>
    /// 批量处理的结果内容
    /// </summary>
    public class BatchData
    {
        /// <summary>
        /// 处理遇到的错误信息
        /// </summary>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }

        /// <summary>
        /// 文件hash(ETAG)
        /// </summary>
        [JsonPropertyName("hash")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Hash { get; set; }

        /// <summary>
        /// 文件大小(字节)
        /// </summary>
        [JsonPropertyName("fsize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long Fsize { get; set; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        [JsonPropertyName("mimeType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MimeType { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [JsonPropertyName("putTime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long PutTime { get; set; }

        /// <summary>
        /// 文件存储类型。
        /// 0 标准存储
        /// 1 低频存储
        /// 2 归档存储
        /// 3 深度归档存储
        /// 4 归档直读存储
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int FileType { get; set; }

        /// <summary>
        /// 文件解冻状态。
        /// 1 解冻中
        /// 2 已解冻
        /// 0 如果是归档/深度归档，但处于冻结，后端不返回此字段，因此默认值为 0。请勿依赖 0 判断冻结状态
        /// </summary>
        [JsonPropertyName("restoreStatus")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int RestoreStatus { get; set; }

        /// <summary>
        /// 文件的存储状态。
        /// 0 启用，非禁用后端不返回此字段，因此默认值为 0。
        /// 1 禁用
        /// </summary>
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Status { get; set; }

        /// <summary>
        /// 文件的 md5 值。
        /// 服务端不确保一定返回此字段，详见：
        /// https://developer.qiniu.com/kodo/1308/stat#:~:text=%E8%AF%A5%E5%AD%97%E6%AE%B5%E3%80%82-,md5,-%E5%90%A6
        /// </summary>
        [JsonPropertyName("md5")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Md5 { get; set; }
        
        /// <summary>
        /// 文件过期删除日期，Unix 时间戳格式。
        /// 文件在设置过期时间后才会返回该字段。
        /// 历史文件过期仍会自动删除，但不会返回该字段，重新设置文件过期时间可使历史文件返回该字段。
        /// </summary>
        [JsonPropertyName("expiration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Expiration { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为低频存储的日期，Unix 时间戳格式。
        /// 文件在设置低频存储转换时间后才会返回该字段。
        /// 历史文件到期仍会自动转换，但不会返回该字段，重新设置文件转换时间可使历史文件返回该字段。
        /// </summary>
        [JsonPropertyName("TransitionToIA")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int TransitionToIa { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为归档直读存储的日期，Unix 时间戳格式。
        /// 文件在设置归档直读存储转换时间后才会返回该字段。
        /// 历史文件到期仍会自动转换，但不会返回该字段，重新设置文件转换时间可使历史文件返回该字段。
        /// </summary>
        [JsonPropertyName("transitionToArchiveIR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int TransitionToArchiveIr { get; set; }

        /// <summary>
        /// 文件生命周期中转为归档存储的日期，Unix 时间戳格式。
        /// 文件在设置归档存储转换时间后才会返回该字段。
        /// 历史文件到期仍会自动转换，但不会返回该字段，重新设置文件转换时间可使历史文件返回该字段。
        /// </summary>
        [JsonPropertyName("transitionToARCHIVE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int TransitionToArchive { get; set; }
        
        /// <summary>
        /// 文件生命周期中转为深度归档存储的日期，Unix 时间戳格式。
        /// 文件在设置深度归档存储转换时间后才会返回该字段。
        /// 历史文件到期仍会自动转换，但不会返回该字段，重新设置文件转换时间可使历史文件返回该字段。
        /// </summary>
        [JsonPropertyName("transitionToDeepArchive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int TransitionToDeepArchive { get; set; }
    }
}
