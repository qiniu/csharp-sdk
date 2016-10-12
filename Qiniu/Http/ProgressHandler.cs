
namespace Qiniu.Http
{
    /// <summary>
    /// HTTP请求发送进度处理
    /// </summary>
    /// <param name="bytesWritten">已写入字节数</param>
    /// <param name="totalBytes">总字节数</param>
    public delegate void ProgressHandler(long bytesWritten, long totalBytes);
}