using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Qiniu.Util
{
    /// <summary>
    /// 字符串处理工具
    /// </summary>
    public class StringUtils
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

        public static string jsonEncode(object obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T jsonDecode<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

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

        public static byte[] sha1(byte[] data)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(data);
        }

        public static string urlencode(string from)
        {
            return Uri.EscapeDataString(from);
        }

        public static string urlValuesEncode(Dictionary<string, string[]> urlValues)
        {
            StringBuilder urlValuesBuilder = new StringBuilder();
           
            foreach (KeyValuePair<string, string[]> kvp in urlValues)
            {
                foreach (string urlVal in kvp.Value)
                {
                    urlValuesBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(urlVal));
                }
            }
            string encodedStr=urlValuesBuilder.ToString();
            return encodedStr.Substring(0, encodedStr.Length - 1);
        }

        /// <summary>
        /// md5 hash in hex string
        /// </summary>
        /// <param name="str">str to hash</param>
        /// <returns>str hashed and format in hex string</returns>
        public static string md5Hash(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] hashData = md5.ComputeHash(data);
            StringBuilder sb = new StringBuilder(hashData.Length * 2);
            foreach (byte b in hashData)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}