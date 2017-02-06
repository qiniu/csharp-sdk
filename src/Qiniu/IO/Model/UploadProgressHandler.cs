namespace Qiniu.IO.Model
{
    /// <summary>
    /// 分片上传进度处理
    /// </summary>
    /// <param name="uploadedBytes">已上传的字节数</param>
    /// <param name="totalBytes">文件总字节数</param>
    public delegate void UploadProgressHandler(long uploadedBytes, long totalBytes);

    /// <summary>
    /// 数据流流上传进度处理
    /// </summary>
    /// <param name="uploadedBytes">已上传的字节数，如果设置为0或负数表示读取完毕</param>
    public delegate void StreamProgressHandler(long uploadedBytes);

}
