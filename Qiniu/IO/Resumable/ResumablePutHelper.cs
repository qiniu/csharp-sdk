using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using Qiniu.IO.Resumable;
namespace Qiniu
{
	public static class ResumbalePutHelper
	{
        public static Dictionary<int,BlkputRet> GetHistory(string filename)
        {
            return ParseHistory(filename);
        }

        /// <summary>
        /// File Format:
        /// i,offset,ctx/n
        /// i,offset,ctx/n
        /// </summary>
        /// <param name="tempFile"></param>
        public static Dictionary<int,BlkputRet> ParseHistory(string tempFile) 
        {
            if (!File.Exists(tempFile)) 
            {
                return null;
            }
            string[] lines = File.ReadAllLines(tempFile);
            Dictionary<int, BlkputRet> result = new Dictionary<int, BlkputRet>();
            foreach (string line in lines) 
            {
                string[] fields = line.Split(',');
                BlkputRet ret = new BlkputRet();
                ret.offset = ulong.Parse(fields[1]);
                ret.ctx = fields[2];
                int idx = int.Parse(fields[0]);
                result.Add(idx, ret);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempFileName"></param>
        /// <param name="idx"></param>
        /// <param name="ret"></param>
        public static void Append(string tempFileName, int idx, BlkputRet ret)
        {
            string content = idx + "," + ret.offset + "," + ret.ctx + "\n";
            File.AppendAllText(tempFileName, content);
        }

        /// <summary>
        /// 获取文件的SHA1值
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>base64编码的sha1值</returns>
        public static string GetFileBase64Sha1(string filename) 
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            using (Stream reader = System.IO.File.OpenRead(filename))
            {
                byte[] result = sha1.ComputeHash(reader);
                return BitConverter.ToString(result);
            }
        }

        /// <summary>
        /// 获取临时文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetTempleFile(string filename)
        {
            string tempPath = Path.GetTempPath();
            return tempPath + filename;
        } 

        public static string GetPutHistroryFile(string filename) { 

            string f = GetFileBase64Sha1(filename);
            return GetTempleFile(f);
        }
	}
}

