using System;
using System.Collections.Generic;
using System.Text;

namespace Qiniu.Util
{
    /// <summary>
    /// 字符串处理工具
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 字符串连接
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="sep">连接符</param>
        /// <returns>连接后字符串</returns>
        public static string Join(IList<string> array, string sep)
        {
            if (array == null || sep == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            int arrayLength = array.Count;
            for (int i = 0; i < arrayLength; i++)
            {
                sb.Append(array[i]);
                sb.Append(sep);
            }
            return sb.ToString(0, sb.Length - sep.Length);
        }

        /// <summary>
        /// 以json格式连接字符串
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <returns>连接后字符串</returns>
        public static string JsonJoin(IList<string> array)
        {
            if (array == null || array.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            int arrayLength = array.Count;

            for (int i = 0; i < arrayLength; i++)
            {
                sb.AppendFormat("\"{0}\",", array[i]);
            }
            return sb.ToString(0, sb.Length - 1);
        }               

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <returns>URL编码字符串</returns>
        public static string UrlEncode(string text)
        {
            return Uri.EscapeDataString(text);
        }

        /// <summary>
        /// URL键值对编码
        /// </summary>
        /// <param name="values">键值对</param>
        /// <returns>URL编码的键值对数据</returns>
        public static string UrlFormEncode(Dictionary<string, string> values)
        {
            StringBuilder urlValuesBuilder = new StringBuilder();
           
            foreach (KeyValuePair<string, string> kvp in values)
            {
                urlValuesBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value));
            }
            string encodedStr=urlValuesBuilder.ToString();
            return encodedStr.Substring(0, encodedStr.Length - 1);
        }

        /// <summary>
        /// 从UNIX时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">时间戳字符串</param>
        /// <returns>日期</returns>
        public static DateTime ConvertToDateTime(string timestamp)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            long ticks = long.Parse(timestamp + "0000000");
            TimeSpan tsx = new TimeSpan(ticks);
            return dt0.Add(tsx);
        }

        /// <summary>
        /// 指定时间点转换为UNIX时间戳
        /// </summary>
        /// <param name="stopAt">绝对时间点</param>
        /// <returns>时间戳字符串</returns>
        public static string ConvertToTimestamp(DateTime stopAt)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            TimeSpan tsx = stopAt.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            return sts.Substring(0, sts.Length - 7);
        }

        /// <summary>
        /// 从现在(调用此函数时刻)起若干秒以后那个时间点的时间戳
        /// </summary>
        /// <param name="secondsAfterNow">从现在起多少秒以后</param>
        /// <returns>时间戳字符串</returns>
        public static string CalcUnixTimestamp(long secondsAfterNow)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            DateTime dt1 = DateTime.Now.AddSeconds(secondsAfterNow);
            TimeSpan tsx = dt1.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            return sts.Substring(0, sts.Length - 7);
        }

    }
}