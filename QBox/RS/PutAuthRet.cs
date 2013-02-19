using System;
using LitJson;
using QBox.RPC;

namespace QBox.RS
{
    public class PutAuthRet : CallRet
    {
        public int Expires { get; private set; }
        public string Url { get; private set; }

        public PutAuthRet(CallRet ret)
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
            Expires = (int)data["expiresIn"];
            Url = (string)data["url"];
        }
    }
}
