using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    public static class StringEx
    {
        public static string ToUrlEncode(this string value)
        {
            return System.Web.HttpUtility.UrlEncode(value);
        }
    }
}
