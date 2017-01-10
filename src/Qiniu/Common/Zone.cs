namespace Qiniu.Common
{
    /// 多机房-自定义区域编号
    /// 华东(CN_East)
    /// 华北(CN_North)
    /// 华南(CN_South)
    /// 北美(US_North)
    public enum ZoneID
    {
        /// <summary>
        /// 华东
        /// </summary>
        CN_East,

        /// <summary>
        /// 华北
        /// </summary>
        CN_North,

        /// <summary>
        /// 华南
        /// </summary>
        CN_South,

        /// <summary>
        /// 北美
        /// </summary>
        US_North,

        /// <summary>
        /// 默认-华东
        /// </summary>
        Default = CN_East
    };

    /// <summary>
    /// 目前已支持的区域：华东/华北/华南/北美
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// 资源管理
        /// </summary>
        public string RsHost { set; get; }

        /// <summary>
        /// 源列表
        /// </summary>
        public string RsfHost { set; get; }

        /// <summary>
        /// 数据处理
        /// </summary>
        public string ApiHost { set; get; }

        /// <summary>
        /// 镜像刷新、资源抓取
        /// </summary>
        public string IovipHost { set; get; }

        /// <summary>
        /// 资源上传
        /// </summary>
        public string UpHost { set; get; }

        /// <summary>
        /// CDN加速
        /// </summary>
        public string UploadHost { set; get; }

        /// <summary>
        /// 根据ZoneID取得对应Zone设置
        /// </summary>
        /// <param name="zoneId">区域编号</param>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        /// <returns></returns>
        public static Zone getZone(ZoneID zoneId, bool useHTTPS = false)
        {
            switch (zoneId)
            {
                case ZoneID.CN_East:
                    return Zone.ZONE_CN_East(useHTTPS);
                case ZoneID.CN_North:
                    return Zone.ZONE_CN_North(useHTTPS);
                case ZoneID.CN_South:
                    return Zone.ZONE_CN_South(useHTTPS);
                case ZoneID.US_North:
                    return Zone.ZONE_US_North(useHTTPS);
                default:
                    return ZONE_CN_East(useHTTPS);
            }
        }

        /// <summary>
        /// 华东
        /// xx-(NULL)
        /// </summary>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static Zone ZONE_CN_East(bool useHTTPS)
        {
            if (useHTTPS)
            {
                return new Zone()
                {
                    RsHost = "https://rs.qbox.me",
                    RsfHost = "https://rsf.qbox.me",
                    ApiHost = "https://api.qiniu.com",
                    IovipHost = "https://iovip.qbox.me",
                    UpHost = "https://up.qbox.me",
                    UploadHost = "https://upload.qbox.me"
                };
            }
            else
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
        }

        /// <summary>
        /// 华北
        /// xx-z1
        /// </summary>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static Zone ZONE_CN_North(bool useHTTPS)
        {
            if (useHTTPS)
            {
                return new Zone()
                {
                    RsHost = "https://rs-z1.qbox.me",
                    RsfHost = "https://rsf-z1.qbox.me",
                    ApiHost = "https://api-z1.qiniu.com",
                    IovipHost = "https://iovip-z1.qbox.me",
                    UpHost = "https://up-z1.qbox.me",
                    UploadHost = "https://upload-z1.qbox.me"
                };
            }
            else
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
        }

        /// <summary>
        /// 华南
        /// xx-z2
        /// </summary>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        public static Zone ZONE_CN_South(bool useHTTPS)
        {
            if (useHTTPS)
            {
                return new Zone()
                {
                    RsHost = "https://rs-z2.qbox.me",
                    RsfHost = "https://rsf-z2.qbox.me",
                    ApiHost = "https://api-z2.qiniu.com",
                    IovipHost = "https://iovip-z2.qbox.me",
                    UpHost = "https://up-z2.qbox.me",
                    UploadHost = "https://upload-z2.qbox.me"
                };
            }
            else
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
        }

        /// <summary>
        /// 北美
        /// xx-na0
        /// </summary>
        /// <param name="useHTTPS">是否使用HTTPS</param>
        /// <returns></returns>
        public static Zone ZONE_US_North(bool useHTTPS)
        {
            if (useHTTPS)
            {
                return new Zone()
                {
                    RsHost = "https://rs-na0.qbox.me",
                    RsfHost = "https://rsf-na0.qbox.me",
                    ApiHost = "https://api-na0.qiniu.com",
                    IovipHost = "https://iovip-na0.qbox.me",
                    UpHost = "https://up-na0.qbox.me",
                    UploadHost = "https://upload-na0.qbox.me"
                };
            }
            else
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
}
