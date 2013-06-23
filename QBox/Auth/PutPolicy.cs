using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using QBox.Util;
using QBox.Conf;
using Newtonsoft.Json;

namespace QBox.Auth
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PutPolicy
    {
        private string scope;
        private string callBackUrl;
        private string callBackBody;
        private string returnUrl;
        private string returnBody;
        private string asyncOps;
        private string endUser;
        private UInt32 expires = 3600;       

        /// <summary>
        /// 一般指文件要上传到的目标存储空间（Bucket）。若为”Bucket”，表示限定只能传到该Bucket（仅限于新增文件）；若为”Bucket:Key”，表示限定特定的文件，可修改该文件。
        /// </summary>
        [JsonProperty("scope")]
        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }
        /// <summary>
        /// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的URL，必须是公网上可以正常进行POST请求并能响应 HTTP Status 200 OK 的有效 URL
        /// </summary>
       [JsonProperty("callBackUrl")]
        public string CallBackUrl
        {
            get { return string.IsNullOrEmpty(callBackUrl) ? string.Empty : callBackUrl; }
            set { callBackUrl = value; }
        }        
        /// <summary>
        /// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的数据。支持 魔法变量 和 自定义变量，不可与 returnBody 同时使用。
        /// </summary>
        [JsonProperty("callBackBody")]
        public string CallBackBody
        {
            get { return string.IsNullOrEmpty(callBackBody) ? string.Empty : callBackBody; }
            set { callBackBody = value; }
        }

       
        /// <summary>
        /// 设置用于浏览器端文件上传成功后，浏览器执行301跳转的URL，一般为 HTML Form 上传时使用。文件上传成功后会跳转到 returnUrl?query_string, query_string 会包含 returnBody 内容。returnUrl 不可与 callbackUrl 同时使用
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl
        {
            get { return string.IsNullOrEmpty(returnUrl) ? string.Empty : returnUrl;  }
            set { returnUrl = value; }
        }
        /// <summary>
        /// 文件上传成功后，自定义从 Qiniu-Cloud-Server 最终返回給终端 App-Client 的数据。支持 魔法变量，不可与 callbackBody 同时使用。
        /// </summary>    
        [JsonProperty("returnBody")]
        public string ReturnBody
        {
            get { return string.IsNullOrEmpty(returnBody) ? string.Empty : returnBody;  }
            set { returnBody = value; }
        }
        /// <summary>
        /// 指定文件（图片/音频/视频）上传成功后异步地执行指定的预转操作。每个预转指令是一个API规格字符串，多个预转指令可以使用分号“;”隔开
        /// </summary>
        [JsonProperty("asyncOps")]
        public string AsyncOps
        {
            get { return string.IsNullOrEmpty(asyncOps) ? string.Empty : asyncOps; }
            set { asyncOps = value; }
        }
        /// <summary>
        /// 给上传的文件添加唯一属主标识，特殊场景下非常有用，比如根据终端用户标识给图片或视频打水印
        /// </summary>
        [JsonProperty("endUser")]
        public string EndUser
        {
            get { return string.IsNullOrEmpty(endUser) ? string.Empty : endUser; }
            set { endUser = value; }
        }
        /// <summary>
        /// 定义 uploadToken 的失效时间，Unix时间戳，精确到秒，缺省为 3600 秒
        /// </summary>
        [JsonProperty("deadline")]
        public UInt32 Expires
        {
            get { return expires;  }
            set { expires = value; }
        }
        
        public PutPolicy(string scope, UInt32 expires=3600)
        {
            Scope = scope;
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            Expires = (UInt32)interval.TotalSeconds + expires;
        }

        public string Marshal()
        {
            // 步骤1：组织元数据（JSONString）
            string flag = JsonConvert.SerializeObject(this);
            // 步骤2：将 Flags 进行安全编码
            string encodedFlags = Base64URLSafe.Encode(flag);
            return encodedFlags;           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Token()
        {
            // 步骤3：将编码后的元数据混入私钥进行签名
            return Config.Encoding.GetString(AuthToken.Make(Marshal()));
        }
    }
}
