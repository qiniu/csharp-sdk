using System;
using LitJson;

namespace QBox.RS
{
    public class PutFileRet : CallRet
    {
        public string Hash { get; private set; }

        public PutFileRet(CallRet ret)
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
            Hash = (string)data["hash"];
        }
    }
}
