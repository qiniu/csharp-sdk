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
        // 空间所在的区域(Zone)
        public static Zone ZONE = Zone.ZONE_CN_East();

        // Fusion API
        public const string FUSION_API_HOST = "http://fusion.qiniuapi.com";

        /// <summary>
        /// 根据Zone配置对应参数(RS_HOST,API_HOST等)
        /// </summary>
        /// <param name="zoneId">ZoneID</param>
        public static void SetZone(ZoneID zoneId)
        {
            switch (zoneId)
            {
                case ZoneID.CN_East:
                    ZONE = Zone.ZONE_CN_East();
                    break;
                case ZoneID.CN_North:
                    ZONE = Zone.ZONE_CN_North();
                    break;
                case ZoneID.CN_South:
                    ZONE = Zone.ZONE_CN_South();
                    break;
                case ZoneID.US_North:
                    ZONE = Zone.ZONE_US_North();
                    break;
                default:
                    break;
            }
        }

    }

}
