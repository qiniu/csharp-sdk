using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Qiniu.RPC;
using Newtonsoft.Json;

namespace Qiniu.IO.Resumable
{

    [JsonObject(MemberSerialization.OptIn)]
    public class BlkputRet
    {
        [JsonProperty("ctx")]
        public string ctx;
        [JsonProperty("checksum")]
        public string checkSum;
        [JsonProperty("crc32")]
        public UInt32 crc32;
        [JsonProperty("offset")]
        public UInt32 offset;
    }

    public class ResumablePutRet : CallRet
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
