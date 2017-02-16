/// <summary>
/// Qiniu (Cloud) C# SDK for .NET Framework 2.0+/Core/UWP
/// Modules in this SDK:
/// "IO", File/Stream Uploading and Downlopading, 文件(流)上传下载;
/// "RS", Resource (Bucket) Management,空间资源管理;
/// "RSF", File/Data Processing, 文件/数据处理; 
/// "CDN",  Fusion CDN, 融合CDN加速; 
/// "Util", Utilities such as MD5 hashing, 实用工具(如MD5哈希计算等);
/// "Common", Common things like Zone Configurations, 公共模块(如Zone配置等);
/// "Http", HTTP Request Manager, HTTP请求管理器
/// </summary>
public class QiniuCSharpSDK
{
    /// <summary>
    /// SDK名称
    /// </summary>
    public const string ALIAS = "QiniuCSharpSDK";

    /// <summary>
    /// 目标框架
    /// </summary>
#if Net20
    public const string RTFX = "NET20";
#elif Net35
    public const string RTFX = "NET35";
#elif Net40
    public const string RTFX = "NET40";
#elif Net45
    public const string RTFX = "NET45";
#elif Net46
    public const string RTFX = "NET46";
#elif NetCore
    public const string RTFX = "NETCore";
#elif WINDOWS_UWP
    public const string RTFX = "UWP";
#else
    public const string RTFX = "UNKNOWN";
#endif

    /// <summary>
    /// SDK版本号
    /// </summary>
    public const string VERSION = "7.2.11";

}
