using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using QBox.Util;
using QBox.Conf;

namespace QBox.Auth
{
    public class PutPolicy
    {
        public string Scope { get; set; }
        public string CallbackUrl { get; set; }
        public string CallbackBodyType { get; set; }
        public string Customer { get; set; }
        public string AsyncOps { get; set; }
        public string ReturnBody { get; set; }
        public UInt32 Expires { get; set; }
        public UInt16 Escape { get; set; }
        public UInt16 DetectMime { get; set; }

        public PutPolicy(string scope, UInt32 expires)
        {
            Scope = scope;
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            Expires = (UInt32)interval.TotalSeconds + expires;
        }

        public string Marshal()
        {
            var data = new Dictionary<string,dynamic>();
            data["scope"] = Scope;
            data["deadline"] = Expires;
            if (!String.IsNullOrEmpty(CallbackUrl))
                data["callbackUrl"] = CallbackUrl;
            if (!String.IsNullOrEmpty(CallbackBodyType))
                data["callbackBodyType"] = CallbackBodyType;
            if (!String.IsNullOrEmpty(AsyncOps))
                data["asyncOps"] = AsyncOps;
            if (!String.IsNullOrEmpty(ReturnBody))
                data["returnBody"] = ReturnBody;
            if (!String.IsNullOrEmpty(Customer))
                data["customer"] = Customer;
            if (Escape != 0)
                data["escape"] = Escape;
            if (DetectMime != 0)
                data["detectMime"] = DetectMime;
            var jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        public string Token()
        {
            return Config.Encoding.GetString(AuthToken.Make(Marshal()));
        }
    }
}
