using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LitJson;

namespace QBox
{
    public class GetRet : CallRet
    {
        public string Hash { get; private set; }
        public long FileSize { get; private set; }
        public string MimeType { get; private set; }
        public string Url { get; private set; }

        public GetRet(CallRet ret)
            : base(ret)
        {
            if (OK && !String.IsNullOrEmpty(Response))
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
            JsonData data = JsonMapper.ToObject(json);
            Hash = (string)data["hash"];
            JsonData fsize = data["fsize"];
            if (fsize.IsInt)
                FileSize = (int)fsize;
            else if (fsize.IsLong)
                FileSize = (long)fsize;
            MimeType = (string)data["mimeType"];
            Url = (string)data["url"];
        }
    }
}
