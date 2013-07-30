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

        
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>h
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
				onPutFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
				onPutFinished(ret);
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upToken">Up token.</param>
        /// <param name="key">Key.</param>
        /// <param name="putStream">Put stream.</param>
        /// <param name="extra">Extra.</param>
        public PutRet Put(string upToken, string key, System.IO.Stream putStream, PutExtra extra)
        {
            PutRet ret;
            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
				CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, putStream);
                ret = new PutRet(callRet);
				onPutFinished(ret);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
				onPutFinished(ret);
                return ret;
            } 
		}

        protected void onPutFinished(PutRet ret)
        {
            if (PutFinished != null)
            {
                PutFinished(this, ret);
            }
        }
    }
}
