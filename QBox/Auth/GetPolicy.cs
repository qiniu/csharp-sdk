using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using QBox.Util;
using QBox.Conf;

namespace QBox.Auth
{
    public class GetPolicy
    {
        public string Pattern { get; set; }
        public long Deadline { get; set; }

        public GetPolicy(string pattern, long expires)
        {
            Pattern = pattern;
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            Deadline = (long)interval.TotalSeconds + expires;
        }

        public string Marshal()
        {
            var data = new Dictionary<string, dynamic>();
            data["S"] = Pattern;
            data["E"] = Deadline;
            var jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        public string Token()
        {
            return Config.Encoding.GetString(AuthToken.Make(Marshal()));
        }
    }
}
