using System;
namespace Qiniu.Storage
{
    /// <summary>
    /// 目前已支持的区域：华东/华北/华南/北美
    /// </summary>
    public class Zone:Region
    {
        /// <summary>
        /// 华东
        /// </summary>
        public static Region ZONE_CN_East = Region_CN_East;

        /// <summary>
        /// 华北
        /// </summary>
        public static Region ZONE_CN_North = Region_CN_North;

        /// <summary>
        /// 华南
        /// </summary>
        public static Region ZONE_CN_South = Region_CN_South;

        /// <summary>
        /// 北美
        /// </summary>
        public static Region ZONE_US_North = Region_US_North;
    }
}
