namespace Qiniu.Http
{
    /// <summary>
    /// HTTP 状态码
    /// </summary>
    public enum HttpCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK = 200,

        /// <summary>
        /// 部分OK
        /// </summary>
        PARTLY_OK = 298,

        /// <summary>
        /// 请求错误
        /// </summary>
        BAD_REQUEST = 400,

        /// <summary>
        /// 无效凭证
        /// </summary>
        BAD_TOKEN = 401,

        /// <summary>
        /// 上传文件大小超限
        /// </summary>
        SIZE_EXCEEDS = 413,

        /// <summary>
        /// 回调失败
        /// </summary>
        CALLBACK_FAILED = 579,

        /// <summary>
        /// 服务端操作失败
        /// </summary>
        SERVER_FAILED = 599,        

        /// <summary>
        /// 文件不存在
        /// </summary>
        FILE_NOT_EXIST = 612,

        /// <summary>
        /// 文件已存在
        /// </summary>
        FILE_EXISTS = 614,

        /// <summary>
        /// 空间或者文件不存在
        /// </summary>
        BUCKET_NOT_EXIST = 631,

        /// <summary>
        /// 资源Context已过期
        /// </summary>
        CONTEXT_EXPIRED = 701,

        #region _USR_

        /// <summary>
        /// 自定义HTTP状态码 (默认值)
        /// </summary>
        USER_UNDEF = -256,

        /// <summary>
        /// 自定义HTTP状态码 (用户取消)
        /// </summary>
        USER_CANCELED = -255,

        /// <summary>
        /// 自定义HTTP状态码 (用户暂停)
        /// </summary>
        USER_PAUSED = -254,

        /// <summary>
        /// 自定义HTTP状态码 (用户继续)
        /// </summary>
        USER_RESUMED = -253,

        /// <summary>
        /// 自定义HTTP状态码 (需要重试)
        /// </summary>
        USER_NEED_RETRY = -252,

        /// <summary>
        /// 自定义HTTP状态码 (异常或错误)
        /// </summary>
        USER_EXCEPTION = -252,

        #endregion _USR_

    }

}
