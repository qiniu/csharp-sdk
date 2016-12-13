
namespace Qiniu.Http
{
    /// <summary>
    /// HTTP请求结果处理
    /// </summary>
    /// <param name="respInfo">请求回复信息</param>
    /// <param name="response">请求回复内容</param>
    public delegate void CompletionHandler(ResponseInfo respInfo, string response);

    /// <summary>
    /// HTTP请求结果(非json/二进制数据)
    /// </summary>
    /// <param name="respInfo"></param>
    /// <param name="respData"></param>
    public delegate void RecvDataHandler(ResponseInfo respInfo, byte[] respData);

}