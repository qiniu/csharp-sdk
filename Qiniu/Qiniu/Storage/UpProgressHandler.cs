
namespace Qiniu.Storage
{
    /// <summary>
    /// 上传进度处理器
    /// </summary>
    /// <param name="key">文件key</param>
    /// <param name="percent">上传进度百分比</param>
    public delegate void UpProgressHandler(string key, double percent);
}