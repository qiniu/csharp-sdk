namespace Qiniu.Storage
{
    /// <summary>
    /// 配置信息，主要包括Region配置(另请参阅Region模块)
    /// 目前已支持的机房包括：
    /// 华东(CN_East), 华北(CN_North), 华南(CN_South), 北美(US_North), 新加坡（AS_Singapore）
    /// 默认设置为华东机房(CN_East) 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 默认高级资源管理域名
        /// </summary>
        public static string DefaultRsHost = "rs.qiniu.com";
        /// <summary>
        /// 默认数据处理域名
        /// </summary>
        public static string DefaultApiHost = "api.qiniu.com";
        /// <summary>
        /// 空间所在的区域(Zone)
        /// </summary>
        public Region Zone = null;
        /// <summary>
        /// 空间所在的区域(Region)
        /// </summary>
        public Region Region = null;
        /// <summary>
        /// 是否采用https域名
        /// </summary>
        public bool UseHttps = false;
        /// <summary>
        /// 是否采用CDN加速域名，对上传有效
        /// </summary>
        public bool UseCdnDomains = false;
        /// <summary>
        /// 分片上传时，片的大小，默认为4MB，以提高上传效率
        /// </summary>
        public ChunkUnit ChunkSize = ChunkUnit.U4096K;
        /// <summary>
        /// 分片上传的阈值，超过该大小采用分片上传的方式
        /// </summary>
        public int PutThreshold =ResumeChunk.GetChunkSize(ChunkUnit.U1024K) * 10;
        /// <summary>
        /// 重试请求次数
        /// </summary>
        public int MaxRetryTimes = 3;

        /// <summary>
        /// 获取资源管理域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string RsHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Region r = null;
            if (this.Zone!=null)
            {
               r = this.Zone;
            }else
            {
                r = this.Region;
            }
           
            if (r == null)
            {
                r = RegionHelper.QueryRegion(ak, bucket);
            }
            return string.Format("{0}{1}", scheme, r.RsHost);
        }

        /// <summary>
        /// 获取资源列表域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string RsfHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Region r = null;
            if (this.Zone != null)
            {
                r = this.Zone;
            }
            else
            {
                r = this.Region;
            }
            if (r == null)
            {
                r = RegionHelper.QueryRegion(ak, bucket);
            }
            return string.Format("{0}{1}", scheme, r.RsfHost);
        }

        /// <summary>
        /// 获取数据处理域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string ApiHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Region r = null;
            if (this.Zone != null)
            {
                r = this.Zone;
            }
            else
            {
                r = this.Region;
            }
            if (r == null)
            {
                r = RegionHelper.QueryRegion(ak, bucket);
            }
            return string.Format("{0}{1}", scheme, r.ApiHost);
        }

        /// <summary>
        /// 获取资源抓取更新域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string IovipHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Region r = null;
            if (this.Zone != null)
            {
                r = this.Zone;
            }
            else
            {
                r = this.Region;
            }
            if (r == null)
            {
                r = RegionHelper.QueryRegion(ak, bucket);
            }
            return string.Format("{0}{1}", scheme, r.IovipHost);
        }

        /// <summary>
        /// 获取文件上传域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string UpHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Region r = null;
            if (this.Zone != null)
            {
                r = this.Zone;
            }
            else
            {
                r = this.Region;
            }
            if (r == null)
            {
                r = RegionHelper.QueryRegion(ak, bucket);
            }
            string upHost = r.SrcUpHosts[0];
            if (this.UseCdnDomains)
            {
                upHost = r.CdnUpHosts[0];
            }

            return string.Format("{0}{1}", scheme, upHost);
        }
    }
}