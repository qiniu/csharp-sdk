using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     Zone辅助类，查询及配置Zone
    /// </summary>
    public class ZoneHelper
    {
        private static readonly Dictionary<string, Zone> ZoneCache = new Dictionary<string, Zone>();
        private static readonly object RwLock = new object();

        /// <summary>
        ///     从uc.qbox.me查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AccessKek</param>
        /// <param name="bucket">空间名称</param>
        public static async Task<Zone> QueryZone(string accessKey, string bucket)
        {
            Zone zone = null;

            var cacheKey = $"{accessKey}:{bucket}";

            //check from cache
            lock (RwLock)
            {
                if (ZoneCache.ContainsKey(cacheKey))
                {
                    zone = ZoneCache[cacheKey];
                }
            }

            if (zone != null)
            {
                return zone;
            }

            //query from uc api
            HttpResult hr = null;
            try
            {
                var queryUrl = $"https://uc.qbox.me/v2/query?ak={accessKey}&bucket={bucket}";
                hr = await HttpManager.SharedInstance.GetAsync(queryUrl);
                if (hr.Code == (int)HttpCode.OK)
                {
                    var zInfo = JsonConvert.DeserializeObject<ZoneInfo>(hr.Text);
                    if (zInfo != null)
                    {
                        zone = new Zone
                        {
                            SrcUpHosts = zInfo.Up.Src.Main,
                            CdnUpHosts = zInfo.Up.Acc.Main,
                            IovipHost = zInfo.Io.Src.Main[0]
                        };
                        if (zone.IovipHost.Contains("z1"))
                        {
                            zone.ApiHost = "api-z1.qiniu.com";
                            zone.RsHost = "rs-z1.qiniu.com";
                            zone.RsfHost = "rsf-z1.qiniu.com";
                        }
                        else if (zone.IovipHost.Contains("z2"))
                        {
                            zone.ApiHost = "api-z2.qiniu.com";
                            zone.RsHost = "rs-z2.qiniu.com";
                            zone.RsfHost = "rsf-z2.qiniu.com";
                        }
                        else if (zone.IovipHost.Contains("na0"))
                        {
                            zone.ApiHost = "api-na0.qiniu.com";
                            zone.RsHost = "rs-na0.qiniu.com";
                            zone.RsfHost = "rsf-na0.qiniu.com";
                        }
                        else if (zone.IovipHost.Contains("as0"))
                        {
                            zone.ApiHost = "api-as0.qiniu.com";
                            zone.RsHost = "rs-as0.qiniu.com";
                            zone.RsfHost = "rsf-as0.qiniu.com";
                        }
                        else
                        {
                            zone.ApiHost = "api.qiniu.com";
                            zone.RsHost = "rs.qiniu.com";
                            zone.RsfHost = "rsf.qiniu.com";
                        }

                        lock (RwLock)
                        {
                            ZoneCache[cacheKey] = zone;
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
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] QueryZone Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                throw new QiniuException(hr, sb.ToString());
            }

            return zone;
        }
    }
}
