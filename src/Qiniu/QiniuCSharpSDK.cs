using System.Diagnostics.CodeAnalysis;

namespace Qiniu
{
    /// <summary>
    ///     Qiniu (Cloud) C# SDK for .NET Framework 2.0+/Core/UWP
    ///     Modules in this SDK:
    ///     "Storage" 存储相关功能，上传，下载，数据处理，资源管理
    ///     "CDN",    Fusion CDN, 融合CDN加速;
    ///     "Util",   Utilities such as MD5 hashing, 实用工具(如MD5哈希计算等);
    ///     "Http", HTTP Request Manager, HTTP请求管理器
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class QiniuCSharpSdk
    {
        /// <summary>
        ///     SDK名称
        /// </summary>
        public const string Alias = "QiniuCSharpSDK";

        /// <summary>
        ///     目标框架
        /// </summary>
#if Net45
        public const string RTFX = "NET45";
#elif Net46
        public const string RTFX = "NET46";
#elif NETSTANDARD1_3
        public const string RTFX = "NETSTANDARD1_3";
#elif NETSTANDARD2_0
        public const string RTFX = "NETSTANDARD2_0";
#elif WINDOWS_UWP
        public const string RTFX = "UWP";
#else
        public const string RTFX = "UNKNOWN";
#endif

        /// <summary>
        ///     SDK版本号
        /// </summary>
        public const string Version = "7.3.0";
    }
}
