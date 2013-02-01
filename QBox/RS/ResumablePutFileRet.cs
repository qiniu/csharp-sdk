using System;
using LitJson;

namespace QBox.RS
{
    public class ResumablePutFileRet : CallRet
    {
        public string Ctx { get; private set; }
        public string Checksum { get; private set; }
        public uint Crc32 { get; private set; }
        public uint Offset { get; private set; }
        public string Host { get; private set; }

        public ResumablePutFileRet(CallRet ret)
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
            Crc32 = (uint)data["crc32"];
            Offset = (uint)data["offset"];
            Host = (string)data["host"];
        }
    }
}
