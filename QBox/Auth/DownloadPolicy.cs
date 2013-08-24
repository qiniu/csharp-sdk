using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using LitJson;
using QBox.RS;
using QBox.Util;

namespace QBox.Auth
{
    public class DownloadPolicy
    {
        public string Pattern { get; set; }
        public long Deadline { get; set; }

        public DownloadPolicy(string pattern, long expires)
        {
            Pattern = pattern;
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.UtcNow;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            Deadline = (long)interval.TotalSeconds + expires;
        }

        public string Marshal()
        {
            JsonData data = new JsonData();
            data["S"] = Pattern;
            data["E"] = Deadline;
            return data.ToJson();
        }

        public byte[] MakeAuthToken()
        {
            return AuthToken.Make(Marshal());
        }

        public string MakeAuthTokenString()
        {
            return Encoding.ASCII.GetString(MakeAuthToken());
        }
    }
}
