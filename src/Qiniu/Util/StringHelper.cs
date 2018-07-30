using System;
using System.Collections.Generic;
using System.Linq;

namespace Qiniu.Util
{
    /// <summary>
    ///     字符串处理工具
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        ///     URL编码
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <returns>URL编码字符串</returns>
        public static string UrlEncode(string text)
        {
            return Uri.EscapeDataString(text);
        }

        /// <summary>
        ///     URL键值对编码
        /// </summary>
        /// <param name="values">键值对</param>
        /// <returns>URL编码的键值对数据</returns>
        public static string UrlFormEncode(Dictionary<string, string> values)
        {
            return string.Join("&", values.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        }
    }
}
