using System.Collections.Generic;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class PrefetchRequest
    {
        public List<string> Urls { get; set; }

        public PrefetchRequest()
        {
            Urls = new List<string>();
        }

        public PrefetchRequest(IEnumerable<string> urls)
        {
            Urls = new List<string>(urls);
        }

        public void AddUrls(IEnumerable<string> urls)
        {
            Urls.AddRange(urls);
        }

        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");

            sb.Append("\"urls\":[");
            for (int i = 0; i < Urls.Count; ++i)
            {
                string url = Urls[i];
                if (!UrlHelper.IsValidUrl(url))
                {
                    continue;
                }

                if (i < Urls.Count - 1)
                {
                    sb.Append(string.Format("\"{0}\",", url));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\"", url));
                }
            }
            sb.Append("] }");

            return sb.ToString();
        }
    }
}
