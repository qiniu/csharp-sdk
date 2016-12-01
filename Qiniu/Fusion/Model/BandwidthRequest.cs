using System.Text;

namespace Qiniu.Fusion.Model
{
    public class BandwidthRequest
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Granularity { get; set; }

        public string Domains { get; set; }

        public BandwidthRequest()
        {
            StartDate = "";
            EndDate = "";
            Granularity = "";
            Domains = "";
        }

        public BandwidthRequest(string startDate,string endDate,string granularity,string domains)
        {
            StartDate = startDate;
            EndDate = endDate;
            Granularity = granularity;
            Domains = domains;
        }

        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat("\"startDate\":\"{0}\", ", StartDate);
            sb.AppendFormat("\"endDate\":\"{0}\", ", EndDate);
            sb.AppendFormat("\"granularity\":\"{0}\", ", Granularity);
            sb.AppendFormat("\"domains\":\"{0}\"", Domains);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
