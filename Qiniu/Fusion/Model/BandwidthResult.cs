using System.Collections.Generic;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class BandwidthResult
    {
        public int Code { get; set; }

        public string Error { get; set; }

        public List<string> Time { get; set; }

        public Dictionary<string,BandWidthData> Data { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code:{0}\n", Code);
            sb.AppendFormat("error:{0}\n", Error);
            sb.Append("time:");
            foreach (var t in Time)
            {
                sb.Append(t + " ");
            }
            sb.Append("\n");
            sb.Append("bandwidth:");
            foreach (var kvp in Data)
            {
                sb.AppendFormat("{0}:\n{1}\n", kvp.Key, kvp.Value);
            }
            sb.Append("\n");

            return sb.ToString();
        }
    }

    public class BandWidthData
    {
        public List<int> China { get; set; }

        public List<int> Oversea { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("China:");
            if (China != null)
            {
                foreach (var c in China)
                {
                    sb.AppendFormat("{0} ", c);
                }
            }
            sb.Append("\n");
            sb.Append("Oversea:");
            if (Oversea != null)
            {
                foreach (var x in Oversea)
                {
                    sb.AppendFormat("{0} ", x);
                }
            }
            sb.Append("\n");

            return sb.ToString();
        }
    }
}
