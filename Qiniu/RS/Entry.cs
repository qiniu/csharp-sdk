using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.RPC;

namespace Qiniu.RS
{
    public class Entry : CallRet
    {
        /// <summary>
        /// 文件的Hash值
        /// </summary>
        /// <value><c>true</c> if this instance hash; otherwise, <c>false</c>.</value>
        public string Hash { get; private set; }

        /// <summary>
        /// 文件的大小(单位: 字节)
        /// </summary>
        /// <value>The fsize.</value>
        public long Fsize { get; private set; }

        /// <summary>
        /// 文件上传到七牛云的时间(Unix时间戳)
        /// </summary>
        /// <value>The put time.</value>
        public long PutTime { get; private set; }

        /// <summary>
        /// 文件的媒体类型，比如"image/gif"
        /// </summary>
        /// <value>The type of the MIME.</value>
        public string MimeType { get; private set; }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public string Customer { get; private set; }

        public Entry(CallRet ret)
            : base(ret)
        {
            if (OK && !string.IsNullOrEmpty(Response))
            {
                try
                {
                    Unmarshal(Response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    this.Exception = e;
                }
            }
        }

        private void Unmarshal(string json)
        {
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (dict != null)
            {
                object tmp;
                if (dict.TryGetValue("hash", out tmp))
                {
                    Hash = (string)tmp;
                }
                if (dict.TryGetValue("mimeType", out tmp))
                {
                    MimeType = (string)tmp;
                }
                if (dict.TryGetValue("fsize", out tmp))
                {
                    Fsize = Convert.ToInt64(tmp);
                }
                if (dict.TryGetValue("putTime", out tmp))
                {
                    PutTime = Convert.ToInt64(tmp);
                }
                if (dict.TryGetValue("customer", out tmp))
                {
                    Customer = (string)tmp;
                }
            }
        }
    }
}
