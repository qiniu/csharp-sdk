using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.Util;
#if ABOVE45
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace Qiniu.RS
{
    /// <summary>
    /// Persistent identifier.
    /// </summary>
    public class PersistentId
    {
        public string persistentId;
    }

    /// <summary>
    /// Persitent error.
    /// </summary>
    public class PersitentError
    {
        public int code;
        public string error;

    }

    /// <summary>
    /// Persistent exception.
    /// </summary>
    public class PersistentException : Exception
    {

        private PersitentError error;

        public PersitentError Error
        {
            get
            {
                return error;
            }
        }

        public PersistentException(PersitentError err)
        {
            this.error = err;
        }
    }

    /// <summary>
    /// 对已有资源手动触发持久化
    /// POST /pfop/ HTTP/1.1
    /// Host: api.qiniu.com  
    /// Content-Type: application/x-www-form-urlencoded  
    /// Authorization: &lt;AccessToken>  
    /// bucket=&lt;bucket>&amp;key=&lt;key>&amp;fops=&lt;fop1>;&lt;fop2>...&lt;fopN>&amp;notifyURL=&lt;persistentNotifyUrl>
    /// </summary>
    public class Pfop : QiniuAuthClient
    {
#if !ABOVE45
        /// <summary>
        /// 请求持久化
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="fops"></param>
        /// <param name="notifyURL"></param>
        /// <returns></returns>
        public string Do(EntryPath entry, string[] fops, Uri notifyURL,string pipleline,int force=0)
        {
            if (fops.Length < 1 || entry == null || string.IsNullOrEmpty(entry.Bucket) || notifyURL == null || !notifyURL.IsAbsoluteUri)
            {
                throw new Exception("params error");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(fops[0]);

            for (int i = 1; i < fops.Length; ++i)
            {
                sb.Append(";");
                sb.Append(fops[i]);
            }

            string body = string.Format("bucket={0}&key={1}&fops={2}&notifyURL={3}&force={4}&pipeline={5}", entry.Bucket, StringEx.ToUrlEncode(entry.Key), Qiniu.Util.StringEx.ToUrlEncode(sb.ToString()), notifyURL.ToString(), force, pipleline);
            CallRet ret = CallWithBinary(Config.API_HOST + "/pfop/", "application/x-www-form-urlencoded",StreamEx.ToStream(body), body.Length);

            if (ret.OK)
            {
                try
                {
                    PersistentId pid = JsonConvert.DeserializeObject<PersistentId>(ret.Response);
                    return pid.persistentId;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }

        /// <summary>
        /// Queries the pfop status.
        /// </summary>
        /// <returns>The pfop status.</returns>
        /// <param name="persistentId">Persistent identifier.</param>
        public string QueryPfopStatus(string persistentId)
        {
            CallRet ret = Call(string.Format("{0}/status/get/prefop?id={1}", Config.API_HOST, persistentId));
            if (ret.OK)
            {
                return ret.Response;
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }
#else
        public async Task<string> DoAsync(EntryPath entry, string[] fops, Uri notifyURL, string pipleline, int force = 0)
        {
            if (fops.Length < 1 || entry == null || string.IsNullOrEmpty(entry.Bucket) || notifyURL == null || !notifyURL.IsAbsoluteUri)
            {
                throw new Exception("params error");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(fops[0]);

            for (int i = 1; i < fops.Length; ++i)
            {
                sb.Append(";");
                sb.Append(fops[i]);
            }

            // FormUrlEncodedContent 自带 UrlEncode
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "bucket", entry.Bucket },
                { "key", entry.Key },
                { "fops", sb.ToString() },
                { "notifyURL", notifyURL.ToString() },
                { "force", force.ToString() },
                { "pipeline", pipleline }
            });

            CallRet ret = await CallWithBinaryAsync(Config.API_HOST + "/pfop/", content);

            if (ret.OK)
            {
                try
                {
                    PersistentId pid = JsonConvert.DeserializeObject<PersistentId>(ret.Response);
                    return pid.persistentId;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }

        public async Task<string> QueryPfopStatusAsync(string persistentId)
        {
            CallRet ret = await CallAsync(string.Format("{0}/status/get/prefop?id={1}", Config.API_HOST, persistentId));
            if (ret.OK)
            {
                return ret.Response;
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }
#endif
    }
}
