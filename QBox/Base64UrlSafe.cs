using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox
{
    public static class Base64UrlSafe
    {
        public static string Encode(string text)
        {
            if (String.IsNullOrEmpty(text)) return "";
            byte[] bs = Encoding.ASCII.GetBytes(text);
            string encoded = Convert.ToBase64String(bs);
            encoded = encoded.Replace('+', '-').Replace('/', '_');
            return encoded;
        }

        public static string Encode(byte[] bs)
        {
            if (bs == null || bs.Length == 0) return "";
            string encoded = Convert.ToBase64String(bs);
            encoded = encoded.Replace('+', '-').Replace('/', '_');
            return encoded;
        }
    }
}
