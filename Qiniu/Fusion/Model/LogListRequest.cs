using System.Text;

namespace Qiniu.Fusion.Model
{
    public class LogListRequest
    {
        public string Day { get; set; }

        public string Domains { get; set; }

        public LogListRequest()
        {
            Day = "";
            Domains = "";
        }

        public LogListRequest(string day,string domains)
        {
            Day = day;
            Domains = domains;
        }

        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat("\"day\":\"{0}\", ", Day);
            sb.AppendFormat("\"domains\":\"{0}\"", Domains);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
