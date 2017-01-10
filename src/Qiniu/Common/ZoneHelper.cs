using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.Http;

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

        /// <summary>
        /// 从uc.qbox.me查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AccessKek</param>
        /// <param name="bucket">空间名称</param>
        public static ZoneID queryZone(string accessKey, string bucket)
        {
            ZoneID zoneId = ZoneID.Default;

            try
            {
                // HTTP/GET https://uc.qbox.me/v1/query?ak=(AK)&bucket=(Bucket)
                // 该请求的返回数据参见后面的 QueryResponse 结构
                // 根据response消息提取出upHost
                string queryUrl = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucket);

                HttpManager httpManager = new Http.HttpManager();
                var hr = httpManager.get(queryUrl, null);
                if (hr.Code == HttpHelper.STATUS_CODE_OK)
                {
                    QueryResponse qr = JsonConvert.DeserializeObject<QueryResponse>(hr.Text);
                    string upHost = qr.HTTP.UP[0];
                    zoneId = ZONE_DICT[upHost];
                }
                else
                {
                    throw new Exception(hr.RefText);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[ConfigZone] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                throw new Exception(sb.ToString());
            }

            return zoneId;
        }

        #region UC_QUERY_RESPONSE

        // 从uc.qbox.me返回的消息，使用Json解析
        // 以下是一个response示例
        // {
        //     "ttl" : 86400,
        //     "http" : {
        //         "up" : [
        //                     "http://up.qiniu.com",
        //                     "http://upload.qiniu.com",
        //                     "-H up.qiniu.com http://183.136.139.16"
        //                 ],
        //        "io" : [
        //                      "http://iovip.qbox.me"
        //                ]
        //             },
        //     "https" : {
        //          "io" : [
        //                     "https://iovip.qbox.me"
        //                  ],
        //         "up" : [
        //                     "https://up.qbox.me"
        //                  ]
        //                  }
        // }

        /// <summary>
        /// 从uc.qbox.me返回的消息
        /// </summary>
        private class QueryResponse
        {
            public string TTL { get; set; }
            public HttpBulk HTTP { get; set; }

            public HttpBulk HTTPS { get; set; }
        }

        /// <summary>
        /// HttpBulk作为QueryResponse的成员
        /// 包含uploadHost和iovip等
        /// </summary>
        private class HttpBulk
        {
            public string[] UP { get; set; }
            public string[] IO { get; set; }
        }

        #endregion UC_QUERY_RESPONSE  
    }

}
