using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     获取bucket信息-结果
    /// </summary>
    public class BucketResult : HttpResult
    {
        /// <summary>
        ///     bucket信息
        /// </summary>
        public BucketInfo Result
        {
            get
            {
                BucketInfo info = null;

                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<BucketInfo>(Text);
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
                sb.AppendLine("bucket-info:");
                sb.AppendLine($"tbl={Result.Tbl}");
                sb.AppendLine($"zone={Result.Zone}");
                sb.AppendLine($"region={Result.Region}");
                sb.AppendLine($"isGlobal={Result.Global}");
                sb.AppendLine($"isLine={Result.Line}");
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
