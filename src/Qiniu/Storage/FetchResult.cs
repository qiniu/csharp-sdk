using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     文件抓取返回的消息
    /// </summary>
    public class FetchResult : HttpResult
    {
        /// <summary>
        ///     Fetch信息列表
        /// </summary>
        public FetchInfo Result
        {
            get
            {
                FetchInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<FetchInfo>(Text);
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
                sb.AppendLine($"Key={Result.Key}, Size={Result.Fsize}, Type={Result.MimeType}, Hash={Result.Hash}");
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
