using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     批量处理结果
    /// </summary>
    public class BatchResult : HttpResult
    {
        /// <summary>
        ///     错误消息
        /// </summary>
        public string Error
        {
            get
            {
                string ex = null;
                if (Code != (int)HttpCode.OK && Code != (int)HttpCode.PARTLY_OK)
                {
                    var ret = JsonConvert.DeserializeObject<Dictionary<string, string>>(Text);
                    if (ret.ContainsKey("error"))
                    {
                        ex = ret["error"];
                    }
                }

                return ex;
            }
        }

        /// <summary>
        ///     获取批量处理结果
        /// </summary>
        public List<BatchInfo> Result
        {
            get
            {
                List<BatchInfo> info = null;
                if ((Code == (int)HttpCode.OK || Code == (int)HttpCode.PARTLY_OK) &&
                    !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<List<BatchInfo>>(Text);
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
                sb.AppendLine("result:");
                int i = 0, n = Result.Count;
                foreach (var v in Result)
                {
                    sb.AppendLine($"#{++i}/{n}");
                    sb.AppendLine($"code: {v.Code}");
                    sb.AppendLine($"data:\n{v.Data}\n");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Error))
                {
                    sb.AppendLine($"Error: {Error}");
                }
                else if (!string.IsNullOrEmpty(Text))
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
