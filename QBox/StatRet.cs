using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LitJson;

namespace QBox
{
    public class StatRet : CallRet
    {
        public string Hash { get; private set; }
        public long FileSize { get; private set; }
        public long PutTime { get; private set; }
        public string MimeType { get; private set; }

        public StatRet(CallRet ret)
            : base(ret)
        {
            if (OK && Response != null)
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
            JsonData putTime = data["putTime"];
            if (putTime.IsInt)
                PutTime = (int)putTime;
            else if (putTime.IsLong)
                PutTime = (long)putTime;
            MimeType = (string)data["mimeType"];
        }
    }
}
