using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
    public class ImageInfoRet : CallRet
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Format { get; private set; }
        public string ColorModel { get; private set; }

        public ImageInfoRet(CallRet ret)
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
            Format = (string)dict["format"];
            Width = (int)dict["width"];
            Height = (int)dict["height"];
            ColorModel = (string)dict["colorModel"];
        }
    }
}
