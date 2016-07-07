using System;
using System.Collections.Generic;
using System.Net;
using Qiniu.Conf;
using Qiniu.Auth;
using Qiniu.RPC;
using Qiniu.Util;
using System.Collections.Specialized;
#if ABOVE45
using System.Threading.Tasks;
#endif

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
            if (key!=null) {
                formData["key"] = key;
            }
            if (extra != null)
            {
                if (extra.CheckCrc == CheckCrcType.CHECK_AUTO)
                {
                    formData["crc32"] = extra.Crc32.ToString();
                }
                if (extra.Params != null)
                {
                    foreach (KeyValuePair<string, string> pair in extra.Params)
                    {
                        formData[pair.Key] = pair.Value;
                    }
                }
            }
            return formData;
        }

        /// <summary>
        /// 设置连接代理
        /// </summary>
        public IWebProxy Proxy { get; set; }

#if !ABOVE45
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>h
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        public PutRet PutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            if (!System.IO.File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }
            PutRet ret;

            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, localFile, this.Proxy);
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
        /// Puts the file without key.
        /// </summary>
        /// <returns>The file without key.</returns>
        /// <param name="upToken">Up token.</param>
        /// <param name="localFile">Local file.</param>
        /// <param name="extra">Extra.</param>
        public PutRet PutFileWithoutKey(string upToken, string localFile, PutExtra extra)
        {
            return PutFile(upToken, null, localFile, extra);
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
            if (!putStream.CanRead)
            {
                throw new Exception("read put Stream error");
            }
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
#else
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>h
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        public async Task<PutRet> PutFileAsync(string upToken, string key, string localFile, PutExtra extra)
        {
            if (!System.IO.File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }
            PutRet ret;

            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
                CallRet callRet = await MultiPart.MultiPostAsync(Config.UP_HOST, formData, localFile, this.Proxy);
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
        /// Puts the file without key.
        /// </summary>
        /// <returns>The file without key.</returns>
        /// <param name="upToken">Up token.</param>
        /// <param name="localFile">Local file.</param>
        /// <param name="extra">Extra.</param>
        public async Task<PutRet> PutFileWithoutKeyAsync(string upToken, string localFile, PutExtra extra)
        {
            return await PutFileAsync(upToken, null, localFile, extra);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upToken">Up token.</param>
        /// <param name="key">Key.</param>
        /// <param name="putStream">Put stream.</param>
        /// <param name="extra">Extra.</param>
        public async Task<PutRet> PutAsync(string upToken, string key, System.IO.Stream putStream, PutExtra extra)
        {
            if (!putStream.CanRead)
            {
                throw new Exception("read put Stream error");
            }
            PutRet ret;
            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
                CallRet callRet = await MultiPart.MultiPostAsync(Config.UP_HOST, formData, putStream);
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
#endif

        protected void onPutFinished(PutRet ret)
        {
            if (PutFinished != null)
            {
                PutFinished(this, ret);
            }
        }
    }
}
