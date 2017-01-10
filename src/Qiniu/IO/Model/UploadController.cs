namespace Qiniu.IO.Model
{
    /// <summary>
    /// 上传任务的状态
    /// </summary>
    public enum UPTS
    {
        /// <summary>
        /// 任务状态:激活
        /// </summary>
        Activated,         

        /// <summary>
        /// 任务状态:暂停
        /// </summary>
        Suspended, 

        /// <summary>
        /// 任务状态:退出
        /// </summary>
        Aborted 
    };

    /// <summary>
    /// 上传任务的控制函数
    /// </summary>
    /// <returns></returns>
    public delegate UPTS UploadController();
}
