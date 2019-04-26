using System;
using System.Text;
using System.Collections.Generic;
using Qiniu.Http;
using Newtonsoft.Json;

namespace Qiniu.Storage
{
    /// <summary>
    /// Zone辅助类，查询及配置Zone
    /// </summary>
    public class RegionHelper
    {
        private static Dictionary<string, Region> RegionCache = new Dictionary<string, Region>();
        private static object rwLock = new object();

        /// <summary>
        /// 从api.qiniu.com查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AccessKek</param>
        /// <param name="bucket">空间名称</param>
        public static Region QueryRegion(string accessKey, string bucket)
        {
            Region region = null;

            string cacheKey = string.Format("{0}:{1}", accessKey, bucket);

            //check from cache
            lock (rwLock)
            {
                if (RegionCache.ContainsKey(cacheKey))
                {
                    region = RegionCache[cacheKey];
                }
            }

            if (region != null)
            {
                return region;
            }

            //query from api
            HttpResult hr = null;
            try
            {
                string queryUrl = string.Format("https://api.qiniu.com/v2/query?ak={0}&bucket={1}", accessKey, bucket);
                HttpManager httpManager = new HttpManager();
                hr = httpManager.Get(queryUrl, null);
                if (hr.Code == (int)HttpCode.OK)
                {
                    RegionInfo rInfo = JsonConvert.DeserializeObject<RegionInfo>(hr.Text);
                    if (rInfo != null)
                    {
                        region = new Region();
                        region.SrcUpHosts = rInfo.Up.Src.Main;
                        region.CdnUpHosts = rInfo.Up.Acc.Main;
                        region.IovipHost = rInfo.Io.Src.Main[0];
                        if (region.IovipHost.Contains("z1"))
                        {
                            region.ApiHost = "api-z1.qiniu.com";
                            region.RsHost = "rs-z1.qiniu.com";
                            region.RsfHost = "rsf-z1.qiniu.com";
                        }
                        else if (region.IovipHost.Contains("z2"))
                        {
                            region.ApiHost = "api-z2.qiniu.com";
                            region.RsHost = "rs-z2.qiniu.com";
                            region.RsfHost = "rsf-z2.qiniu.com"; 
                        }
                        else if (region.IovipHost.Contains("na0"))
                        {
                            region.ApiHost = "api-na0.qiniu.com";
                            region.RsHost = "rs-na0.qiniu.com";
                            region.RsfHost = "rsf-na0.qiniu.com";
                        }
                        else
                        {
                            region.ApiHost = "api.qiniu.com";
                            region.RsHost = "rs.qiniu.com";
                            region.RsfHost = "rsf.qiniu.com";
                        }

                        lock (rwLock)
                        {
                            RegionCache[cacheKey] = region;
                        }
                    }
                    else
                    {
                        throw new Exception("JSON Deserialize failed: " + hr.Text);
                    }
                }
                else
                {
                    throw new Exception("code: " + hr.Code + ", text: " + hr.Text + ", ref-text:" + hr.RefText);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] QueryRegion Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                throw new QiniuException(hr, sb.ToString());
            }

            return region;
        }
    }
}
