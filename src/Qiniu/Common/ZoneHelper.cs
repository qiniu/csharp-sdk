using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.Http;

#if Net45 || Net46 || NetCore || WINDOWS_UWP
using System.Threading.Tasks;
#endif

namespace Qiniu.Common
{
    /// <summary>
    /// Zone辅助类，查询及配置Zone
    /// </summary>
    public class ZoneHelper
    {
        /// <summary>
        /// Zone
        /// 不同区域upHost分别唯一，据此确定对应的Zone
        /// </summary>
        private static Dictionary<string, ZoneID> ZONE_DICT = new Dictionary<string, ZoneID>()
            {
                {"http://up.qiniu.com",     ZoneID.CN_East },
                {"http://up-z1.qiniu.com",  ZoneID.CN_North},
                {"http://up-z2.qiniu.com",  ZoneID.CN_South},
                {"http://up-na0.qiniu.com", ZoneID.US_North}
            };

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        /// <summary>
        /// 从uc.qbox.me查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AccessKek</param>
        /// <param name="bucket">空间名称</param>
        public static ZoneID QueryZone(string accessKey, string bucket)
        {
            ZoneID zoneId = ZoneID.Default;

            try
            {
                // HTTP/GET https://uc.qbox.me/v1/query?ak=(AK)&bucket=(Bucket)
                string queryUrl = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucket);

                HttpManager httpManager = new HttpManager();
                var hr = httpManager.Get(queryUrl, null);
                if (hr.Code == (int)HttpCode.OK)
                {
                    ZoneInfo zInfo = JsonConvert.DeserializeObject<ZoneInfo>(hr.Text);
                    string upHost = zInfo.HTTP.UP[0];
                    zoneId = ZONE_DICT[upHost];
                }
                else
                {
                    throw new Exception("text: " + hr.Text + ", ref-text:" + hr.RefText);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] queryZone Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                throw new Exception(sb.ToString());
            }

            return zoneId;
        }

#endif

#if Net45 || Net46 || NetCore || WINDOWS_UWP

        /// <summary>
        /// 从uc.qbox.me查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AccessKek</param>
        /// <param name="bucket">空间名称</param>
        public static async Task<ZoneID> QueryZoneAsync(string accessKey, string bucket)
        {
            ZoneID zoneId = ZoneID.Default;

            try
            {
                // HTTP/GET https://uc.qbox.me/v1/query?ak=(AK)&bucket=(Bucket)
                string queryUrl = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucket);

                HttpManager httpManager = new HttpManager();
                var hr = await httpManager.GetAsync(queryUrl, null);
                if (hr.Code == (int)HttpCode.OK)
                {
                    ZoneInfo zInfo = JsonConvert.DeserializeObject<ZoneInfo>(hr.Text);
                    string upHost = zInfo.HTTP.UP[0];
                    zoneId = ZONE_DICT[upHost];
                }
                else
                {
                    throw new Exception("text: " + hr.Text + ", ref-text:" + hr.RefText);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] queryZone Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                throw new Exception(sb.ToString());
            }

            return zoneId;
        }
        
#endif

    }

}
