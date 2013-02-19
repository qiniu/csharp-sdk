using System;
using LitJson;
using QBox.RPC;

namespace QBox.FileOp
{
    public class ImageInfoRet : CallRet
    {
        public string Format { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
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
            JsonData data = JsonMapper.ToObject(json);
            Format = (string)data["format"];
            Width = (int)data["width"];
            Height = (int)data["height"];
            ColorModel = (string)data["colorModel"];
        }
    }
}
