using System.Text.RegularExpressions;

namespace Qiniu.Fusion.Model
{
    public class UrlHelper
    {
        private static Regex regx = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");

        private static Regex regu = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,/~\+#]*)?");

        private static Regex regd = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,/~\+#]*)?/");

        public static bool IsValidUrl(string _url)
        {
            return regx.IsMatch(_url);
        }

        public static bool IsNormalUrl(string _url)
        {
            return regu.IsMatch(_url);
        }

        public static bool IsValidDir(string _dir)
        {
            return regd.IsMatch(_dir);
        }

        public static string GetNormalUrl(string _url)
        {
            var m = regu.Match(_url);
            return m.Value;
        }

        public static void UrlSplit(string url, out string host, out string path, out string file, out string query)
        {
            int start = 0;

            Regex regHost = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+");
            host = regHost.Match(url, start).Value;
            start += host.Length;

            Regex regPath = new Regex(@"(/(\w|\-)*)+/");
            path = regPath.Match(url, start).Value;
            if(string.IsNullOrEmpty(path))
            {
                path = "/";
            }
            start += path.Length;

            int index = url.IndexOf('?', start);
            file = url.Substring(start, index - start);

            query = url.Substring(index);
        }
    }
}
