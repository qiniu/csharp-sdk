#if Net45||Net46||NetCore||WINDOWS_UWP
using System.Threading.Tasks;
#endif

namespace Qiniu.Common
{
    /// <summary>
    /// 配置信息，主要包括Zone配置(另请参阅Zone模块)
    /// 目前已支持的机房包括：
    /// 华东(CN_East), 华北(CN_North), 华南(CN_South), 北美(US_North)
    /// 默认设置为华东机房(CN_East) 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 空间所在的区域(Zone)
        /// </summary>
        public static Zone ZONE = Zone.GetZone(ZoneID.Default);

        /// <summary>
        /// Fusion API Host
        /// </summary>
        public const string FUSION_API_HOST = "http://fusion.qiniuapi.com";

        /// <summary>
        /// DFOP API Host
        /// </summary>
        public const string DFOP_API_HOST = "http://api.qiniu.com";

        /// <summary>
        /// PILI API Host
        /// </summary>
        public const string PILI_API_HOST = "http://pili.qiniuapi.com";

        /// <summary>
        /// 根据Zone配置对应参数(RS_HOST,API_HOST等)
        /// </summary>
        /// <param name="zoneId">ZoneID</param>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static void SetZone(ZoneID zoneId, bool useHTTPS)
        {
            ZONE = Zone.GetZone(zoneId, useHTTPS);
        }

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        /// <summary>
        /// 自动配置Zone
        /// </summary>
        /// <param name="accessKey">AccessKey</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static void AutoZone(string accessKey, string bucket, bool useHTTPS)
        {
            ZoneID id = ZoneHelper.QueryZone(accessKey, bucket);
            SetZone(id, useHTTPS);
        }

#endif

#if Net45 || Net46 || NetCore || WINDOWS_UWP

        /// <summary>
        /// 自动配置Zone
        /// </summary>
        /// <param name="accessKey">AccessKey</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static async Task AutoZoneAsync(string accessKey, string bucket, bool useHTTPS)
        {
            ZoneID id = await ZoneHelper.QueryZoneAsync(accessKey, bucket);
            SetZone(id, useHTTPS);
        }     

#endif

    }

}
