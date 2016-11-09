using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Qiniu.Common
{
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
        /// <param name="accessKey">AK</param>
        /// <param name="bucketName">Bucket</param>
        public static ZoneID QueryZone(string accessKey, string bucket)
        {
            ZoneID zoneId = ZoneID.Default;

            // HTTP/GET https://uc.qbox.me/v1/query?ak=(AK)&bucket=(Bucket)
            // 该请求的返回数据参见后面的 QueryResponse 结构
            // 根据response消息提取出upHost
            string queryUrl = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucket);

            try
            {
                HttpClient client = new HttpClient();
                var msg = client.GetStringAsync(queryUrl);
                QueryResponse qr = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryResponse>(msg.Result);
                string upHost = qr.HTTP.UP[0];
                zoneId = ZONE_DICT[upHost];
            }
            catch (Exception ex)
            {
                throw new Exception("[ConfigZone] Error: " + ex.Message);
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
