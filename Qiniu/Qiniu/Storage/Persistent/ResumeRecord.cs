using Newtonsoft.Json;
using Qiniu.Util;
namespace Qiniu.Storage.Persistent
{
    /// <summary>
    /// 分片上传进度记录内容
    /// </summary>
    public class ResumeRecord
    {
        public long Size { get; private set; }
        public long Offset { get; private set; }
        public long LastModified { get; private set; }
        public string[] Contexts { get; private set; }

        /// <summary>
        /// 构建上传进度记录对象
        /// </summary>
        /// <param name="size">文件总大小</param>
        /// <param name="offset">已上传偏移位置</param>
        /// <param name="lastModified">文件最后修改时间</param>
        /// <param name="contexts">已上传块的Context集合</param>
        public ResumeRecord(long size, long offset, long lastModified, string[] contexts)
        {
            this.Size = size;
            this.Offset = offset;
            this.LastModified = lastModified;
            this.Contexts = contexts;
        }

        /// <summary>
        /// 从文件记录构建上传进度对象
        /// </summary>
        /// <param name="jsonData">文件内容</param>
        /// <returns>上传进度对象</returns>
        public static ResumeRecord fromJsonData(string jsonData)
        {
            ResumeRecord record = JsonConvert.DeserializeObject<ResumeRecord>(jsonData);
            return record;
        }

        /// <summary>
        /// 序列化上传进度记录
        /// </summary>
        /// <returns>序列化后的上传进度字符串</returns>
        public string toJsonData()
        {
            return string.Format("{{\"size\":{0}, \"offset\":{1}, \"modify_time\":{2}, \"contexts\":[{3}]}}",
                this.Size, this.Offset, this.LastModified, StringUtils.jsonJoin(this.Contexts));
        }
    }
}
