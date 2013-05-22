using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using QBox.RPC;

namespace QBox.IO
{
    public class PutRet : CallRet
    {
        public string Hash { get; private set; }

        public PutRet(CallRet ret)
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
            Hash = (string)dict["hash"];
        }
    }
}
