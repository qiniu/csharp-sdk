using System;
using System.Collections.Generic;
using System.Net;
using Qiniu.Conf;
using Qiniu.RPC;

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
            try
            {

                var postParams = new Dictionary<string, object>();
                postParams["token"] = upToken;
                postParams["key"] = key;
                postParams["file"] = new FileParameter(localFile, extra.MimeType);
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

        private void putFinished(PutRet ret)
        {
            if (PutFinished != null)
            {
                PutFinished(this, ret);
            }
        }
    }
}
