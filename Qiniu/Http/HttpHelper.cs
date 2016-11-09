using System;
using Qiniu.Common;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
    public class HttpHelper
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        /// <returns></returns>
        public string GetUserAgent()
        {
            return string.Format("{0}/{1} ({2})", QiniuSDK.ALIAS, QiniuSDK.VERSION, Environment.MachineName);
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns></returns>
        public string CreateFormDataBoundary()
        {
            string now = DateTime.UtcNow.Ticks.ToString();
            return string.Format("-------{0}Boundary{1}", QiniuSDK.ALIAS, StringHelper.CalcMD5(now));
        }
    }
}
