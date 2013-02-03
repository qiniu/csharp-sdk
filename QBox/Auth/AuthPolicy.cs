using System;
using System.Text;
using System.IO;
using LitJson;
using System.Security.Cryptography;
using QBox.RS;

namespace QBox.Auth
{
    public class AuthPolicy
    {
        public string Scope { get; set; }
        public long Deadline { get; set; }
        public string CallbackUrl { get; set; }
        public string CallbackBodyType { get; set; }
        public bool Escape { get; set; }
        public string AsyncOps { get; set; }
        public string ReturnBody { get; set; }

        public AuthPolicy(string scope, long expires)
        {
            Scope = scope;
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            Deadline = (long)interval.TotalSeconds + expires;
        }

        public string Marshal()
        {
            JsonData data = new JsonData();
            data["scope"] = Scope;
            data["deadline"] = Deadline;
            if (!String.IsNullOrEmpty(CallbackUrl))
                data["callbackUrl"] = CallbackUrl;
            if (!String.IsNullOrEmpty(CallbackBodyType))
                data["callbackBodyType"] = CallbackBodyType;
            if (Escape)
                data["escape"] = 1;
            if (!String.IsNullOrEmpty(AsyncOps))
                data["asyncOps"] = AsyncOps;
            if (!String.IsNullOrEmpty(ReturnBody))
                data["returnBody"] = ReturnBody;
            return data.ToJson();
        }

        public byte[] MakeAuthToken()
        {
            Encoding encoding = Encoding.ASCII;
            byte[] accessKey = encoding.GetBytes(Config.ACCESS_KEY);
            byte[] secretKey = encoding.GetBytes(Config.SECRET_KEY);
            byte[] upToken = null;
            try
            {
                byte[] policyBase64 = encoding.GetBytes(Base64UrlSafe.Encode(Marshal()));
                byte[] digestBase64 = null;
                using (HMACSHA1 hmac = new HMACSHA1(secretKey))
                {
                    byte[] digest = hmac.ComputeHash(policyBase64);
                    digestBase64 = encoding.GetBytes(Base64UrlSafe.Encode(digest));
                }
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(accessKey, 0, accessKey.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(digestBase64, 0, digestBase64.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(policyBase64, 0, policyBase64.Length);
                    upToken = buffer.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return upToken;
        }

        public string MakeAuthTokenString()
        {
            return Encoding.ASCII.GetString(MakeAuthToken());
        }
    }
}
