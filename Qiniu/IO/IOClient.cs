using System;
using System.Collections.Generic;
using System.Net;
using Qiniu.Conf;
using Qiniu.Auth;
using Qiniu.RPC;
using Qiniu.Util;
using Qiniu.IO.Resumable;
using System.Collections.Specialized;

namespace Qiniu.IO
{
    /// <summary>
    /// 上传客户端
    /// </summary>
    public class IOClient
    {
        /// <summary>
        /// 无论成功或失败，上传结束时触发的事件
        /// </summary>
        public event EventHandler<PutRet> PutFinished;
        private Dictionary<string, object> putParams(string upToken, string key, PutParameter putObj, PutExtra extra)
        {
            var postParams = new Dictionary<string, object>();
            postParams["token"] = upToken;
            postParams["key"] = key;
            postParams["file"] = putObj;
            if (extra.Params != null)
            {
                if (extra.CheckCrc == CheckCrcType.CHECK_AUTO)
                {
                    postParams["crc32"] = extra.Crc32;
                }
                foreach (KeyValuePair<string, string> pair in extra.Params)
                {
                    postParams[pair.Key] = pair.Value;
                }
            }
            return postParams;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        public PutRet PutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            PutRet ret;
            NameValueCollection formData = getFormData(upToken, key, extra);
          
           
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, localFile);
                ret = new PutRet(callRet);
                putFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                putFinished(ret);
                return ret;
            } 
        }

        private static NameValueCollection getFormData(string upToken, string key, PutExtra extra)
        {
            NameValueCollection formData = new NameValueCollection();
            formData["token"] = upToken;
            formData["key"] = key;
            if (extra != null && extra.Params != null)
            {
                if (extra.CheckCrc == CheckCrcType.CHECK_AUTO)
                {
                    formData["crc32"] = extra.Crc32.ToString();
                }
                foreach (KeyValuePair<string, string> pair in extra.Params)
                {
                    formData[pair.Key] = pair.Value;
                }
            }
            return formData;
        }

        public PutRet Put(string upToken, string key, System.IO.Stream streamReader, PutExtra extra)
        {
            PutRet ret;
            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, localFile);
                ret = new PutRet(callRet);
                putFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                putFinished(ret);
                return ret;
            } 
        }


        private PutRet doPut(Dictionary<string,object> postParams)
        {
            PutRet ret;
            try
            {                
                CallRet callRet = MultiPart.Post(Config.UP_HOST, postParams);
                ret = new PutRet(callRet);
                putFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                putFinished(ret);
                return ret;
            }           
 
        }
        public PutRet Put(string upToken, string key, System.IO.StreamReader reader, PutExtra extra)
        {
            var postParams = putParams(upToken, key, new StreamParameter(reader, extra.MimeType), extra);
            return doPut(postParams);  
        }

        private void putFinished(PutRet ret)
        {
            if (PutFinished != null)
            {
                PutFinished(this, ret);
            }
        }
    }
}
