using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    /// 上传完成结果处理器
    /// </summary>
    /// <param name="key">上传文件的key</param>
    /// <param name="respInfo">上传请求回复信息</param>
    /// <param name="response">上传请求回复内容</param>
    public delegate void UpCompletionHandler(string key, ResponseInfo respInfo, string response);
}
