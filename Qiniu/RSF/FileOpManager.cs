using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using Qiniu.Util;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RSF.Model;
using Newtonsoft.Json;

namespace Qiniu.RSF
{
    /// <summary>
    /// 数据处理
    /// </summary>
    public class FileOpManager
    {
        private Signature signature;
        private HttpClient client;

        public FileOpManager(Mac mac)
        {
            signature = new Signature(mac);
            client = new HttpClient();
        }

        /// <summary>
        /// 生成管理凭证
        /// </summary>
        /// <param name="pfopUrl"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public string CreateManageToken(string pfopUrl,byte[] body)
        {
            return string.Format("QBox {0}", signature.SignRequest(pfopUrl, body));
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns></returns>
        public HttpResult pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force)
        {
            HttpResult result = new HttpResult();

            string pfopUrl = Config.ZONE.ApiHost + "/pfop/";
            Dictionary<string, string> pfopParams = new Dictionary<string, string>();
            pfopParams.Add("bucket", bucket );
            pfopParams.Add("key", key );
            pfopParams.Add("fops",  fops );
            if (!string.IsNullOrEmpty(notifyUrl))
            {
                pfopParams.Add("notifyURL", notifyUrl );
            }
            if (force)
            {
                pfopParams.Add("force",  "1" );
            }
            if (!string.IsNullOrEmpty(pipeline))
            {
                pfopParams.Add("pipeline", pipeline);
            }

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, pfopUrl);
                req.Content = new FormUrlEncodedContent(pfopParams);
                var rs = req.Content.ReadAsByteArrayAsync();
                string token = CreateManageToken(pfopUrl, rs.Result);
                req.Headers.Add("Authorization", token);

                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                var pi = JsonConvert.DeserializeObject<PersistentInfo>(ret.Result);
                result.Message = pi.PersistentId;
            }
            catch(Exception ex)
            {
                result.Message = "[pfop] Error: " + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns></returns>
        public HttpResult pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force)
        {
            string newFops = string.Join(";", fops);
            return pfop(bucket, key, newFops, pipeline, notifyUrl, force);
        }

        /// <summary>
        /// 查询pfop操作处理结果(或状态)
        /// </summary>
        /// <param name="persistentId">持久化ID</param>
        /// <returns></returns>
        public HttpResult prefop(string persistentId)
        {
            HttpResult result = new HttpResult();

            string url = string.Format("{0}/status/get/prefop?id={1}", Config.ZONE.ApiHost, persistentId);
            try
            {
                var msg = client.GetAsync(url);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch(Exception ex)
            {
                result.Message = "[prefop] Error: " + ex.Message;
            }

            return result;
        }
    }
}
