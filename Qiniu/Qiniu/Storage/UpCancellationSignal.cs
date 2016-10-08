
namespace Qiniu.Storage
{
    /// <summary>
    /// 文件上传取消信号
    /// </summary>
    /// <returns>取消状态,true为取消，false为继续</returns>
    public delegate bool UpCancellationSignal();
}
