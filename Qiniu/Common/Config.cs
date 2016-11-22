
namespace Qiniu.Common
{
    /// <summary>
    /// 配置信息，主要包括Zone配置
    /// [2016-09-05 18:10] 添加北美(US_North)机房 @fengyh
    /// [2016-09-23 10:26] 添加华南(CN_South)机房 @fengyh
    /// [2016-09-27 17:50] 更新并精简部分代码 @fengyh
    /// </summary>
    public class Config
    {
        // SDK的版本号
        public const string VERSION = "1.2.0";


        // 空间所在的区域(Zone)
        public static Zone ZONE = Zone.ZONE_CN_East();

        // Fusion API
        public const string FUSION_API_HOST = "http://fusion.qiniuapi.com";


        //分片上传块的大小，固定为4M，不可修改
        public const int BLOCK_SIZE = 4 * 1024 * 1024;

        //上传失败重试次数
        public const int RETRY_MAX = 5;

        //回复超时时间，单位微秒
        public const int TIMEOUT_INTERVAL = 30 * 1000;

        //分片上传切片大小
        public static int CHUNK_SIZE = 2 * 1024 * 1024;

        //分片上传的阈值，文件超过该大小采用分片上传
        public static int PUT_THRESHOLD = 512 * 1024;

        /// <summary>
        /// 根据Zone配置对应参数(RS_HOST,API_HOST等)
        /// </summary>
        /// <param name="zoneId">ZoneID</param>
        public static void ConfigZone(ZoneID zoneId)
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