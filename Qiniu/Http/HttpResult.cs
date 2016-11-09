namespace Qiniu.Http
{
    /// <summary>
    /// (HTTP请求的)返回消息
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// 状态码 (200表示OK)
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 消息(或错误)文本
        /// </summary>
        public string Message { get; set; }

        public HttpResult()
        {
            StatusCode = 0;
            Message = "";
        }
    }
}
