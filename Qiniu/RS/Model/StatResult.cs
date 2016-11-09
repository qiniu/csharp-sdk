using Qiniu.Http;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间文件信息(stat操作)的返回消息
    /// </summary>
    public class StatResult : HttpResult
    {
        public StatInfo StatInfo { get; set; }
    }
}
