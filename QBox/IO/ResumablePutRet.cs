using System;
using LitJson;
using QBox.RPC;

namespace QBox.IO
{
    class ResumablePutRet : CallRet
    {
        public string Ctx { get; private set; }
        public string Checksum { get; private set; }
        public string Host { get; private set; }

        public ResumablePutRet(CallRet ret)
            : base(ret)
        {
            if (!String.IsNullOrEmpty(Response))
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
            Ctx = (string)data["ctx"];
            Checksum = (string)data["checksum"];
            Host = (string)data["host"];
        }
    }
}
