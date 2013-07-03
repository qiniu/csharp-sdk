using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
    public class ExifValType
    {
        public string val { get; set; }
        public int type { get; set; }
    }

    public class ExifRet : CallRet
    {
        private Dictionary<string, ExifValType> dict;

        public ExifValType this[string key]
        {
            get
            {
                return dict[key];
            }
        }

        public ExifRet(CallRet ret)
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
            dict = jss.Deserialize<Dictionary<string, ExifValType>>(json);
        }

        public override string ToString()
        {
            var jss = new JavaScriptSerializer();
            return jss.Serialize(dict);
        }
    }
}
