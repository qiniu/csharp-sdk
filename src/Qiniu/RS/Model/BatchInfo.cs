namespace Qiniu.RS.Model
{
    /// <summary>
    /// 批量处理返回的信息
    /// </summary>
    public class BatchInfo
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public object Data { get; set; }
    }
}
