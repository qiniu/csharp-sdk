using System;
using Qiniu.Util;

namespace Qiniu.Http
{
    /// <summary>
    /// HTTP辅助工具:帮助生成UA,boundary等
    /// </summary>
    public class HttpHelper
    {
        public const string ContentTypeTextPlain = "text/plain";
        public const string ContentTypeApplicationJson = "application/json";
        public const string ContentTypeApplicationOctet = "application/octet-stream";
        public const string ContentTypeWwwForm = "application/x-www-form-urlencoded";
        public const string ContentTypeMultipart = "multipart/form-data";

        public const int StatusCodeOk = 200;
        public const int StatusCodePartlyOk = 298;
        public const int StatusCodeUndefined = -256;
        public const int StatusCodeUserCanceled = -255;
        public const int StatusCodeUserPaused = -254;
        public const int StatusCodeUserResumed = -253;
        public const int StatusCodeNeedRetry = -252;
        public const int StatusCodeException = -252;

        /// <summary>
        /// 资源类型：普通文本
        /// </summary>
        [Obsolete("Use ContentTypeTextPlain instead.")]
        public static string CONTENT_TYPE_TEXT_PLAIN = "text/plain";

        /// <summary>
        /// 资源类型：JSON字符串
        /// </summary>
        [Obsolete("Use ContentTypeApplicationJson instead.")]
        public static string CONTENT_TYPE_APP_JSON = "application/json";

        /// <summary>
        /// 资源类型：未知类型(数据流)
        /// </summary>
        [Obsolete("Use ContentTypeApplicationOctet instead.")]
        public static string CONTENT_TYPE_APP_OCTET = "application/octet-stream";

        /// <summary>
        /// 资源类型：表单数据(键值对)
        /// </summary>
        [Obsolete("Use ContentTypeWwwForm instead.")]
        public static string CONTENT_TYPE_WWW_FORM = "application/x-www-form-urlencoded";

        /// <summary>
        /// 资源类型：多分部数据
        /// </summary>
        [Obsolete("Use ContentTypeMultipart instead.")]
        public static string CONTENT_TYPE_MULTIPART = "multipart/form-data";
        
        /// <summary>
        /// HTTP状态码200 (OK)
        /// </summary>
        [Obsolete("Use StatusCodeOk instead.")]
        public static int STATUS_CODE_OK = 200;

        /// <summary>
        /// HTTP状态码298 (部分OK)
        /// </summary>
        [Obsolete("Use StatusCodePartlyOk instead.")]
        public static int STATUS_CODE_PARTLY_OK = 298;

        /// <summary>
        /// 自定义HTTP状态码 (默认值)
        /// </summary>
        [Obsolete("Use StatusCodeUndefined instead.")]
        public static int STATUS_CODE_UNDEF = -256;

        /// <summary>
        /// 自定义HTTP状态码 (用户取消)
        /// </summary>
        [Obsolete("Use StatusCodeUserCanceled instead.")]
        public static int STATUS_CODE_USER_CANCELED = -255;

        /// <summary>
        /// 自定义HTTP状态码 (用户暂停)
        /// </summary>
        [Obsolete("Use StatusCodeUserPaused instead.")]
        public static int STATUS_CODE_USER_PAUSED = -254;

        /// <summary>
        /// 自定义HTTP状态码 (用户继续)
        /// </summary>
        [Obsolete("Use StatusCodeUserResumed instead.")]
        public static int STATUS_CODE_USER_RESUMED = -253;

        /// <summary>
        /// 自定义HTTP状态码 (需要重试)
        /// </summary>
        [Obsolete("Use StatusCodeNeedRetry instead.")]
        public static int STATUS_CODE_NEED_RETRY= -252;

        /// <summary>
        /// 自定义HTTP状态码 (异常或错误)
        /// </summary>
        [Obsolete("Use StatusCodeException instead.")]
        public static int STATUS_CODE_EXCEPTION = -252;

        /// <summary>
        /// 客户端标识
        /// </summary>
        /// <returns>客户端标识UA</returns>
        [Obsolete("Use GetUserAgent instead.")]
        public static string getUserAgent()
        {
            return GetUserAgent();
        }

        /// <summary>
        /// 客户端标识
        /// </summary>
        /// <returns>客户端标识UA</returns>
        public static string GetUserAgent()
        {
#if NetStandard
            string sfx = Environment.MachineName;
#else
            var osInfo = Environment.OSVersion;
            string sfx = Environment.MachineName + "; " + osInfo.Platform + "; " + osInfo.Version; 
#endif
            return $"{QiniuCSharpSDK.ALIAS}/{QiniuCSharpSDK.VERSION} ({sfx})";
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>多部分表单数据的boundary</returns>
        [Obsolete("Use CreateFormDataBoundary instead.")]
        public static string createFormDataBoundary()
        {
            return CreateFormDataBoundary();
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>多部分表单数据的boundary</returns>
        public static string CreateFormDataBoundary()
        {
            string now = DateTime.UtcNow.Ticks.ToString();
            return $"-------{QiniuCSharpSDK.ALIAS}Boundary{Hashing.CalcMD5(now)}";
        }
    }
}
