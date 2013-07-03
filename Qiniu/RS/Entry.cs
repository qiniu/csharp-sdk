using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Qiniu.RPC;
using Newtonsoft.Json;
namespace Qiniu.RS
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
            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, dynamic>>(json);
            Hash = (string)dict["hash"];
            MimeType = (string)dict["mimeType"];
            Fsize = (long)dict["fsize"];
            PutTime = (long)dict["putTime"];
            if (dict.ContainsKey("customer"))
            {
                Customer = (string)dict["customer"];
            }
        }
    }
}
