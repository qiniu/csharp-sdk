
namespace Qiniu.Http
{
    /// <summary>
    /// HTTP请求取消信号
    /// </summary>
    /// <returns>true取消，false继续</returns>
    public delegate bool CancellationSignal();
}
