using System.Collections.Generic;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class PrefetchResult
    {
        public int Code { get; set; }

        public string Error { get; set; }

        public string RequestId { get; set; }

        public List<string> InvalidUrls { get; set; }

        public int QuotaDay { get; set; }

        public int SurplusaDay { get; set; }

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
            sb.AppendFormat("quotaDay:{0}\n", QuotaDay);
            sb.AppendFormat("surplusaDay:{0}\n", SurplusaDay);

            return sb.ToString();
        }
    }
}
