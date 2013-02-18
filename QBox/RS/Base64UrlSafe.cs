using System;
using System.Text;

namespace QBox.RS
{
    public static class Base64UrlSafe
    {
        public static string Encode(string text)
        {
            if (String.IsNullOrEmpty(text)) return "";
            byte[] bs = Encoding.UTF8.GetBytes(text);
            string encodedStr = Convert.ToBase64String(bs);
            encodedStr = encodedStr.Replace('+', '-').Replace('/', '_');
            return encodedStr;
        }

        public static string Encode(byte[] bs)
        {
            if (bs == null || bs.Length == 0) return "";
            string encodedStr = Convert.ToBase64String(bs);
            encodedStr = encodedStr.Replace('+', '-').Replace('/', '_');
            return encodedStr;
        }
    }
}
