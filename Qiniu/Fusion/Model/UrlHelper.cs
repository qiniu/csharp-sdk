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

    }
}
