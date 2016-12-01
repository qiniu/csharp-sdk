using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class RefreshResult
    {
        public int Code { get; set; }

        public string Error { get; set; }

        public string RequestId { get; set; }

        public List<string> InvalidUrls { get; set; }

        public List<string> InvalidDirs { get; set; }

        public int UrlQuotaDay { get; set; }

        public int UrlSurplusaDay { get; set; }

        public int DirQuotaDay { get; set; }

        public int DirSurplusaDay { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code:{0}\n", Code);
            sb.AppendFormat("error:{0}\n", Error);
            sb.AppendFormat("requestId:{0}\n", RequestId);
            sb.Append("invalidUrls:");
            if (InvalidUrls != null)
            {
                foreach (var s in InvalidUrls)
                {
                    sb.Append(s + " ");
                }
            }
            sb.Append("\n");
            sb.Append("invalidDirs:");
            if (InvalidDirs != null)
            {
                foreach (var s in InvalidDirs)
                {
                    sb.Append(s + " ");
                }
            }
            sb.Append("\n");
            sb.AppendFormat("urlQuotaDay:{0}\n", UrlQuotaDay);
            sb.AppendFormat("urlSurplusaDay:{0}\n", UrlSurplusaDay);
            sb.AppendFormat("dirQuotaDay:{0}\n", DirQuotaDay);
            sb.AppendFormat("dirSurplusaDay:{0}\n", DirSurplusaDay);

            return sb.ToString();
        }
    }
}
