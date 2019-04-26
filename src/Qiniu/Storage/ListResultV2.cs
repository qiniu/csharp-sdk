using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;
using System;
using System.Text.RegularExpressions;

namespace Qiniu.Storage
{
    /// <summary>
    /// 获取空间文件列表(list操作)的返回消息
    /// </summary>
    public class ListResultV2 : HttpResult
    {
        /// <summary>
        /// 文件列表信息
        /// </summary>
        public ListInfoV2[] Result
        {
            get
            {
                string[] mm=null;
                ListInfoV2[] info =null;
                if ((Code == (int)HttpCode.OK) && (!string.IsNullOrEmpty(Text)))
                {
                    mm = Regex.Split(Text, "\\s+", RegexOptions.IgnoreCase);
                    info = new ListInfoV2[mm.Length-1];
                    for (int i = 0; i < mm.Length- 1; i++)
                    {
                        info[i] = JsonConvert.DeserializeObject<ListInfoV2>(mm[i]);
                    }
                }
                return info;
            }
        }

        /// <summary>
        /// 转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code: {0}\n", Code);

            if (Result != null)
            {
                if (Result.Length != 0)
                {
                    sb.AppendLine("items:");
                    int i = 0, n = Result.Length;
                    foreach (var item in Result)
                    {
                        sb.AppendFormat("#{0}/{1}:Key={2}, Size={3}, Mime={4}, Hash={5}, Time={6}, Type={7}, Status={8}\n",
                           ++i, n, item.Item.Key, item.Item.Fsize, item.Item.MimeType, item.Item.Hash, item.Item.PutTime, item.Item.FileType,item.Item.Status);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    sb.AppendLine("text:");
                    sb.AppendLine(Text);
                }
            }
            sb.AppendLine();

            sb.AppendFormat("ref-code: {0}\n", RefCode);

            if (!string.IsNullOrEmpty(RefText))
            {
                sb.AppendLine("ref-text:");
                sb.AppendLine(RefText);
            }

            if (RefInfo != null)
            {
                sb.AppendFormat("ref-info:\n");
                foreach (var d in RefInfo)
                {
                    sb.AppendLine(string.Format("{0}: {1}", d.Key, d.Value));
                }
            }

            return sb.ToString();
        }
    }
}
