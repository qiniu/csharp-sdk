using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     获取空间文件列表(list操作)的返回消息
    /// </summary>
    public class ListResult : HttpResult
    {
        /// <summary>
        ///     文件列表信息
        /// </summary>
        public ListInfo Result
        {
            get
            {
                ListInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<ListInfo>(Text);
                }

                return info;
            }
        }

        /// <summary>
        ///     转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"code: {Code}");

            if (Result != null)
            {
                if (Result.CommonPrefixes != null)
                {
                    sb.Append("commonPrefixes:");
                    foreach (var p in Result.CommonPrefixes) sb.Append($"{p} ");
                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(Result.Marker))
                {
                    sb.AppendLine($"marker: {Result.Marker}");
                }

                if (Result.Items != null)
                {
                    sb.AppendLine("items:");
                    int i = 0, n = Result.Items.Count;
                    foreach (var item in Result.Items)
                    {
                        sb.AppendLine($"#{++i}/{n}:Key={item.Key}, Size={item.Fsize}, Mime={item.MimeType}, Hash={item.Hash}, Time={item.PutTime}, Type={item.FileType}");
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

            sb.AppendLine($"ref-code: {RefCode}");

            if (!string.IsNullOrEmpty(RefText))
            {
                sb.AppendLine("ref-text:");
                sb.AppendLine(RefText);
            }

            if (RefInfo != null)
            {
                sb.AppendLine("ref-info:");
                foreach (var d in RefInfo) sb.AppendLine($"{d.Key}: {d.Value}");
            }

            return sb.ToString();
        }
    }
}
