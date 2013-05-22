using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
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
            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, dynamic>>(json);
            Ctx = (string)dict["ctx"];
            Checksum = (string)dict["checksum"];
            Host = (string)dict["host"];
        }
    }
}
