using System.Text.Json.Serialization;

namespace Qiniu.Storage
{
    /// <summary>
    /// 持久化请求的回复
    /// </summary>
    public class PfopInfo 
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id;
        /// <summary>
        /// 任务类型，为 1 代表为闲时任务
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Type;
        /// <summary>
        /// 任务创建时间
        /// </summary>
        [JsonPropertyName("creationDate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CreationDate;
        /// <summary>
        /// 任务结果状态码
        /// </summary>
        [JsonPropertyName("code")]
        public int Code;
        /// <summary>
        /// 任务结果状态描述
        /// </summary>
        [JsonPropertyName("desc")]
        public string Desc;
        /// <summary>
        /// 待处理的数据文件
        /// </summary>
        [JsonPropertyName("inputKey")]
        public string InputKey;
        /// <summary>
        /// 待处理文件所在空间
        /// </summary>
        [JsonPropertyName("inputBucket")]
        public string InputBucket;
        /// <summary>
        /// 数据处理队列
        /// </summary>
        [JsonPropertyName("pipeline")]
        public string Pipeline;
        /// <summary>
        /// 任务的Reqid
        /// </summary>
        [JsonPropertyName("reqid")]
        public string Reqid;
        /// <summary>
        /// 任务来源
        /// </summary>
        [JsonPropertyName("taskFrom")]
        public string TaskFrom;
        /// <summary>
        /// 数据处理的命令集合
        /// </summary>
        [JsonPropertyName("items")]
        public PfopItems[] Items;
    }

    /// <summary>
    /// 持久化处理命令
    /// </summary>
    public class PfopItems
    {
        /// <summary>
        /// 命令
        /// </summary>
        [JsonPropertyName("cmd")]
        public string Cmd;
        /// <summary>
        /// 命令执行结果状态码
        /// </summary>
        [JsonPropertyName("code")]
        public string Code;
        /// <summary>
        /// 命令执行结果描述
        /// </summary>
        [JsonPropertyName("desc")]
        public string Desc;
        /// <summary>
        /// 命令执行错误
        /// </summary>
        [JsonPropertyName("Error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error;
        /// <summary>
        /// VSample命令的生成文件名列表
        /// </summary>
        [JsonPropertyName("keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] Keys;
        /// <summary>
        /// 命令生成的文件名
        /// </summary>
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Key;
        /// <summary>
        /// 命令生成的文件内容hash
        /// </summary>
        [JsonPropertyName("hash")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Hash;
        /// <summary>
        /// 该命令是否返回了上一次相同命令生成的结果
        /// </summary>
        [JsonPropertyName("returnOld")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ReturnOld;
    }
}
