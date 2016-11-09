using Qiniu.Http;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间文件列表(list操作)的返回消息
    /// </summary>
    public class ListResult:HttpResult
    {
        public ListInfo ListInfo { get; set; }
    }
}
