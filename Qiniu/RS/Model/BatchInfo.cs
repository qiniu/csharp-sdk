namespace Qiniu.RS.Model
{
    /// <summary>
    /// 批量处理返回的信息
    /// Code:状态码
    /// Data:消息文本
    /// </summary>
    public class BatchInfo
    {
        public int Code { get; set; }

        public object Data { get; set; }
    }
}
