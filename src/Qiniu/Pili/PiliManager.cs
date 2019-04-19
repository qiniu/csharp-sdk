using Qiniu.Http;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Pili
{
    /// <summary>
    /// 直播云服务端
    /// </summary>
    public class PiliManager
    {
        private const string PILI_API_HOST = "http://pili.qiniuapi.com";
        private Auth _auth;
        private HttpManager _httpManager;
        private readonly string _hub;
        private readonly string _encodedStreamTitle;

        public PiliManager(Mac mac, string hub, string streamTitle)
        {
            _auth = new Auth(mac);
            _httpManager = new HttpManager();
            _hub = hub;
            _encodedStreamTitle = Base64.UrlSafeBase64Encode(streamTitle);
        }

        private string saveAsEntry()
        {
            return string.Format("{0}/v2/hubs/{1}/streams/{2}/saveas", PILI_API_HOST, _hub, _encodedStreamTitle);
        }

        /// <summary>
        /// 录制直播回放
        /// </summary>
        /// <param name="fname">保存的文件名</param>
        /// <param name="start">要保存的直播的起始时间</param>
        /// <param name="end">要保存的直播的结束时间</param>
        /// <param name="format">保存的文件格式</param>
        /// <param name="pipeline">数据处理的私有队列</param>
        /// <param name="notify">保存成功回调通知地址</param>
        /// <param name="expireDays">更改ts文件的过期时间</param>
        /// <returns></returns>
        public SaveAsResult SaveAs(string fname = "", long start = 0, long end = 0, string format = "", string pipeline = "", string notify = "", int expireDays = 0)
        {
            SaveAsRequest request = new SaveAsRequest(fname, start, end, format, pipeline, notify, expireDays);

            SaveAsResult result = new SaveAsResult();

            try
            {
                string url = saveAsEntry();
                string body = request.ToJsonStr();
                string token = _auth.CreateStreamManageToken(url, body);

                HttpResult hr = _httpManager.PostJson(url, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [saveas] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }
    }
}