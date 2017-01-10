using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Qiniu.Util
{
    /// <summary>
    /// 字符串处理工具
    /// 特别注意，不同平台使用的Cryptography可能略有不同，使用中如有遇到问题，请反馈
    /// 提交您的issue到 https://github.com/fengyhack/csharp-sdk-shared
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 字符串连接
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="sep">连接符</param>
        /// <returns>连接后字符串</returns>
        public static string join(string[] array, string sep)
        {
            if (array == null || sep == null)
            {
                return null;
            }
            StringBuilder joined = new StringBuilder();
            int arrayLength = array.Length;
            for (int i = 0; i < arrayLength; i++)
            {
                joined.Append(array[i]);
                if (i < arrayLength - 1)
                {
                    joined.Append(sep);
                }
            }
            return joined.ToString();
        }

        /// <summary>
        /// 以json格式连接字符串
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <returns>连接后字符串</returns>
        public static string jsonJoin(string[] array)
        {
            if (array == null)
            {
                return null;
            }
            StringBuilder joined = new StringBuilder();
            int arrayLength = array.Length;
            for (int i = 0; i < arrayLength; i++)
            {
                joined.Append("\"").Append(array[i]).Append("\"");
                if (i < arrayLength - 1)
                {
                    joined.Append(",");
                }
            }
            return joined.ToString();
        }

        /// <summary>
        /// 获取字符串Url安全Base64编码值
        /// </summary>
        /// <param name="from">源字符串</param>
        /// <returns>已编码字符串</returns>
        public static string urlSafeBase64Encode(string from)
        {
            return urlSafeBase64Encode(Encoding.UTF8.GetBytes(from));
        }

        /// <summary>
        /// URL安全的base64编码
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static string urlSafeBase64Encode(byte[] from)
        {
            return Convert.ToBase64String(from).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// 解码Url安全的Base64编码值
        /// </summary>
        /// <param name="from">编码字符串</param>
        /// <returns>已解码字符串</returns>
        public static byte[] urlsafeBase64Decode(string from)
        {
            return Convert.FromBase64String(from.Replace('-', '+').Replace('_', '/'));
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <returns>JSON编码字符串</returns>
        public static string jsonEncode(object obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, setting);
        }

        /// <summary>
        /// JSON解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="jsonData">JSON字符串</param>
        /// <returns>JSON解码结果</returns>
        public static T jsonDecode<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /// <summary>
        /// bucket:key 编码
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>编码</returns>
        public static string encodedEntry(string bucket, string key)
        {
            if (key == null)
            {
                return urlSafeBase64Encode(bucket);
            }
            else
            {
                return urlSafeBase64Encode(bucket + ":" + key);
            }
        }

        /// <summary>
        /// 计算SHA1
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <returns>SHA1</returns>
        public static byte[] calcSHA1(byte[] data)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(data);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="from">源字符串</param>
        /// <returns>URL编码字符串</returns>
        public static string urlEncode(string from)
        {
            return Uri.EscapeDataString(from);
        }

        /// <summary>
        /// URL键值对编码
        /// </summary>
        /// <param name="values">键值对</param>
        /// <returns>URL表单编码的键值对数据</returns>
        public static string urlValuesEncode(Dictionary<string, string> values)
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
        /// md5 hash in hex string
        /// </summary>
        /// <param name="str">待计算的字符串</param>
        /// <returns>MD5结果</returns>
        public static string calcMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] hashData = md5.ComputeHash(data);
            StringBuilder sb = new StringBuilder(hashData.Length * 2);
            foreach (byte b in hashData)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 从UNIX时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">时间戳字符串</param>
        /// <returns>日期</returns>
        public static DateTime fromUnixTimestamp(string timestamp)
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
        public static string toUnixTimestamp(DateTime stopAt)
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
        public static string calcUnixTimestamp(long secondsAfterNow)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            DateTime dt1 = DateTime.Now.AddSeconds(secondsAfterNow);
            TimeSpan tsx = dt1.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            return sts.Substring(0, sts.Length - 7);
        }

    }
}