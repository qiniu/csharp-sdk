
namespace Qiniu.Http
{
    public class HttpResult
    {
        public ResponseInfo ResponseInfo { set; get; }
        public string Response { set; get; }

        public HttpResult() { }

        public HttpResult(ResponseInfo respInfo, string response)
        {
            this.ResponseInfo = respInfo;
            this.Response = response;
        }
    }
}
