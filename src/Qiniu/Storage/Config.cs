﻿
using System.Collections.Generic;

namespace Qiniu.Storage
{
    /// <summary>
    /// 配置信息，主要包括Zone配置(另请参阅Zone模块)
    /// 目前已支持的机房包括：
    /// 华东(CN_East), 华北(CN_North), 华南(CN_South), 北美(US_North), 新加坡（AS_Singapore）
    /// 默认设置为华东机房(CN_East) 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 默认空间管理域名
        /// </summary>
        public static string DefaultUcHost = "uc.qbox.me";
        /// <summary>
        /// 默认查询区域域名
        /// </summary>
        public static string DefaultQueryRegionHost = "uc.qiniuapi.com";
        /// <summary>
        /// 默认备用查询区域域名
        /// </summary>
        public static List<string> DefaultBackupQueryRegionHosts = new List<string>
        {
            "kodo-config.qiniuapi.com",
            "uc.qbox.me"
        };
        
        /// <summary>
        /// 默认高级资源管理域名
        /// </summary>
        public static string DefaultRsHost = "rs.qiniu.com";
        /// <summary>
        /// 默认数据处理域名
        /// </summary>
        public static string DefaultApiHost = "api.qiniuapi.com";
        /// <summary>
        /// 默认数据处理域名
        /// </summary>
        public static string DefaultIoHost = "iovip.qiniuio.com";
        /// <summary>
        /// 默认数据处理域名
        /// </summary>
        public static string DefaultRsfHost = "rsf.qiniu.com";
        /// <summary>
        /// 空间所在的区域(Zone)
        /// </summary>
        public Zone Zone = null;
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
        /// 默认值应与 <see cref="PutExtra.MaxRetryTimes"/> 一致
        public int MaxRetryTimes { set; get; } = 3;
        
        private string _ucHost = DefaultUcHost;

        private string _queryRegionHost = DefaultQueryRegionHost;

        private List<string> _backupQueryRegionHosts = DefaultBackupQueryRegionHosts;

        public void SetUcHost(string val)
        {
            _ucHost = val;
            _queryRegionHost = val;
            _backupQueryRegionHosts.Clear();
        }

        public string UcHost()
        {
            string scheme = UseHttps ? "https://" : "http://";
            return string.Format("{0}{1}", scheme, _ucHost);
        }

        public void SetQueryRegionHost(string val)
        {
            _queryRegionHost = val;
            _backupQueryRegionHosts.Clear();
        }

        public string QueryRegionHost()
        {
            string scheme = UseHttps ? "https://" : "http://";
            return string.Format("{0}{1}", scheme, _queryRegionHost);
        }

        public void SetBackupQueryRegionHosts(List<string> val)
        {
            _backupQueryRegionHosts = val;
        }

        public List<string> BackupQueryRegionHosts()
        {
            return _backupQueryRegionHosts;
        }

        /// <summary>
        /// 获取资源管理域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string RsHost(string ak, string bucket)
        {
            string scheme = UseHttps ? "https://" : "http://";
            Zone z = this.Zone;
            if (z == null)
            {
                z = ZoneHelper.QueryZone(ak, bucket, QueryRegionHost(), BackupQueryRegionHosts());
            }
            return string.Format("{0}{1}", scheme, z.RsHost);
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
            Zone z = this.Zone;
            if (z == null)
            {
                z = ZoneHelper.QueryZone(ak, bucket, QueryRegionHost(), BackupQueryRegionHosts());
            }
            return string.Format("{0}{1}", scheme, z.RsfHost);
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
            Zone z = this.Zone;
            if (z == null)
            {
                z = ZoneHelper.QueryZone(ak, bucket, QueryRegionHost(), BackupQueryRegionHosts());
            }
            return string.Format("{0}{1}", scheme, z.ApiHost);
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
            Zone z = this.Zone;
            if (z == null)
            {
                z = ZoneHelper.QueryZone(ak, bucket, QueryRegionHost(), BackupQueryRegionHosts());
            }
            return string.Format("{0}{1}", scheme, z.IovipHost);
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
            Zone z = this.Zone;
            if (z == null)
            {
                z = ZoneHelper.QueryZone(ak, bucket, QueryRegionHost(), BackupQueryRegionHosts());
            }
            string upHost = z.SrcUpHosts[0];
            if (this.UseCdnDomains)
            {
                upHost = z.CdnUpHosts[0];
            }

            return string.Format("{0}{1}", scheme, upHost);
        }
    }
}
