using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace QBox
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
