using System.Collections.Generic;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class RefreshRequest
    {
        public List<string> Urls { get; set; }

        public List<string> Dirs { get; set; }

        public RefreshRequest()
        {
            Urls = new List<string>();
            Dirs = new List<string>();
        }

        public RefreshRequest(IEnumerable<string> urls,IEnumerable<string> dirs)
        {
            Urls = new List<string>(urls);
            Dirs = new List<string>(dirs);
        }

        public void AddUrls(IEnumerable<string> urls)
        {
            Urls.AddRange(urls);
        }

        public void AddDirs(IEnumerable<string> dirs)
        {
            Dirs.AddRange(dirs);
        }

        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");

            sb.Append("\"urls\":[");
            for (int i = 0; i < Urls.Count; ++i)
            {
                if (i < Urls.Count - 1)
                {
                    sb.Append(string.Format("\"{0}\",", Urls[i]));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\"", Urls[i]));
                }
            }
            sb.Append("], ");

            sb.Append("\"dirs\":[");
            for (int i = 0; i < Dirs.Count; ++i)
            {
                if (i < Dirs.Count - 1)
                {
                    sb.Append(string.Format("\"{0}\",", Dirs[i]));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\"", Dirs[i]));
                }
            }
            sb.Append("] }");

            return sb.ToString();
        }

    }
}
