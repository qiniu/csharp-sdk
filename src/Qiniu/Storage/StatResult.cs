using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     获取空间文件信息(stat操作)的返回消息
    /// </summary>
    public class StatResult : HttpResult
    {
        /// <summary>
        ///     stat信息列表
        /// </summary>
        public FileInfo Result
        {
            get
            {
                FileInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<FileInfo>(Text);
                }

                return info;
            }
        }

        /// <summary>
        ///     转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"code: {Code}");

            if (Result != null)
            {
                sb.AppendLine($"Size={Result.Fsize}, Type={Result.MimeType}, Hash={Result.Hash}, Time={Result.PutTime}");
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
