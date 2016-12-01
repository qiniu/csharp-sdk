using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Qiniu.Common
{
    /// 华东(CN_East)
    /// 华北(CN_North)
    /// 华南(CN_South)
    /// 北美(US_North)
    public enum ZoneID
    {
        CN_East,
        CN_North,
        CN_South,
        US_North,
        Default=CN_East
    };

    /// <summary>
    /// 目前已支持的区域：华东/华北/华南/北美
    /// </summary>
    public class Zone
    {
        // 资源管理
        public string RsHost { set; get; }

        // 资源列表
        public string RsfHost { set; get; }

        // 数据处理
        public string ApiHost { set; get; }

        // 镜像刷新、资源抓取
        public string IovipHost { set; get; }

        // 资源上传
        public string UpHost { set; get; }

        // CDN加速
        public string UploadHost { set; get; }

        /// <summary>
        /// 默认(华东)
        /// </summary>
        /// <returns></returns>
        public static Zone ZONE_Default()
        {
            return ZONE_CN_East();
        }

        /// <summary>
        /// 华东
        /// xx-(NULL)
        /// </summary>
        public static Zone ZONE_CN_East()
        {
            return new Zone()
            {
                RsHost = "http://rs.qbox.me",
                RsfHost = "http://rsf.qbox.me",
                ApiHost = "http://api.qiniu.com",
                IovipHost = "http://iovip.qbox.me",
                UpHost = "http://up.qiniu.com",
                UploadHost = "http://upload.qiniu.com"
            };
        }

        /// <summary>
        /// 华北
        /// xx-z1
        /// </summary>
        public static Zone ZONE_CN_North()
        {
            return new Zone()
            {
                RsHost = "http://rs-z1.qbox.me",
                RsfHost = "http://rsf-z1.qbox.me",
                ApiHost = "http://api-z1.qiniu.com",
                IovipHost = "http://iovip-z1.qbox.me",
                UpHost = "http://up-z1.qiniu.com",
                UploadHost = "http://upload-z1.qiniu.com"
            };
        }

        /// <summary>
        /// 华南
        /// xx-z2
        /// </summary>
        public static Zone ZONE_CN_South()
        {
            return new Zone()
            {
                RsHost = "http://rs-z2.qbox.me",
                RsfHost = "http://rsf-z2.qbox.me",
                ApiHost = "http://api-z2.qiniu.com",
                IovipHost = "http://iovip-z2.qbox.me",
                UpHost = "http://up-z2.qiniu.com",
                UploadHost = "http://upload-z2.qiniu.com"
            };
        }

        /// <summary>
        /// 北美
        /// xx-na0
        /// </summary>
        public static Zone ZONE_US_North()
        {
            return new Zone()
            {
                RsHost = "http://rs-na0.qbox.me",
                RsfHost = "http://rsf-na0.qbox.me",
                ApiHost = "http://api-na0.qiniu.com",
                IovipHost = "http://iovip-na0.qbox.me",
                UpHost = "http://up-na0.qiniu.com",
                UploadHost = "http://upload-na0.qiniu.com"
            };
        }

    }
}
