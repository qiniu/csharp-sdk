using System;
using LitJson;
using QBox.RPC;

namespace QBox.RS
{
    public class Entry : CallRet
    {
        public string Hash { get; private set; }
        public long Fsize { get; private set; }
        public long PutTime { get; private set; }
        public string MimeType { get; private set; }
        public string Customer { get; private set; }

        public Entry(CallRet ret)
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
            MimeType = (string)data["mimeType"];

            JsonData fsize = data["fsize"];
            if (fsize.IsInt)
                Fsize = (int)fsize;
            else if (fsize.IsLong)
                Fsize = (long)fsize;

            JsonData putTime = data["putTime"];
            if (putTime.IsInt)
                PutTime = (int)putTime;
            else if (putTime.IsLong)
                PutTime = (long)putTime;
        }
    }
}
