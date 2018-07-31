using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Qiniu.Http
{
    /// <summary>
    ///     HTTP请求(GET,POST等)的返回消息
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        ///     非法上传凭证错误
        /// </summary>
        public static readonly HttpResult InvalidToken = new HttpResult
        {
            Code = (int)HttpCode.INVALID_TOKEN,
            Text = "invalid uptoken"
        };

        /// <summary>
        ///     非法文件错误
        /// </summary>
        public static readonly HttpResult InvalidFile = new HttpResult
        {
            Code = (int)HttpCode.INVALID_FILE,
            Text = "invalid file"
        };

        /// <summary>
        ///     初始化(所有成员默认值，需要后续赋值)
        /// </summary>
        public HttpResult()
        {
            Code = (int)HttpCode.USER_UNDEF;
            Text = null;
            Data = null;

            RefCode = (int)HttpCode.USER_UNDEF;
            RefInfo = null;
        }

        /// <summary>
        ///     状态码 (200表示OK)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     消息或错误文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     消息或错误(二进制格式)
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        ///     参考代码(用户自定义)
        /// </summary>
        public int RefCode { get; set; }

        /// <summary>
        ///     附加信息(用户自定义,如Exception内容)
        /// </summary>
        public string RefText { get; set; }

        /// <summary>
        ///     参考信息(从返回消息WebResponse的头部获取)
        /// </summary>
        public Dictionary<string, string> RefInfo { get; set; }

        /// <summary>
        ///     对象复制
        /// </summary>
        /// <param name="source">要复制其内容的来源</param>
        public void Shadow(HttpResult source)
        {
            Code = source.Code;
            Text = source.Text;
            Data = source.Data;
            RefCode = source.RefCode;
            RefText += source.RefText;
            RefInfo = source.RefInfo;
        }

        /// <summary>
        ///     读取 <see cref="HttpResponseMessage" /> 数据
        /// </summary>
        /// <param name="response">Http响应</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <exception cref="HttpRequestException"></exception>
        internal async Task ReadAsync(HttpResponseMessage response, bool binaryMode)
        {
            ReadInfo(response);

            if (binaryMode)
            {
                Data = await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                Text = await response.Content.ReadAsStringAsync();
            }
        }

        internal async Task ReadErrorAsync(Exception exception, HttpResponseMessage response)
        {
            var sb = new StringBuilder();
            var e = exception;
            while (e != null)
            {
                sb.Append(e.Message + " ");
                e = e.InnerException;
            }

            sb.AppendLine();
            RefText += sb.ToString();
            RefCode = (int)HttpCode.USER_UNDEF;

            if (response != null)
            {
                ReadInfo(response);
                Text = await response.Content.ReadAsStringAsync();
            }
        }

        private void ReadInfo(HttpResponseMessage response)
        {
            Code = (int)response.StatusCode;
            RefCode = (int)response.StatusCode;

            if (RefInfo == null)
            {
                RefInfo = new Dictionary<string, string>();
            }

            RefInfo.Add("ProtocolVersion", response.Version.ToString());

            if (!string.IsNullOrEmpty(response.Content.Headers.ContentType.CharSet))
            {
                RefInfo.Add("Characterset", response.Content.Headers.ContentType.CharSet);
            }

            if (!response.Content.Headers.ContentEncoding.Any())
            {
                RefInfo.Add("ContentEncoding", string.Join("; ", response.Content.Headers.ContentEncoding));
            }

            RefInfo.Add("ContentType", response.Content.Headers.ContentType.ToString());

            RefInfo.Add("ContentLength", response.Content.Headers.ContentLength.ToString());

            foreach (var header in response.Headers) RefInfo.Add(header.Key, string.Join("; ", header.Value));
        }

        /// <summary>
        ///     转换为易读或便于打印的字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"code:{Code}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(Text))
            {
                sb.AppendLine("text:");
                sb.AppendLine(Text);
            }

            if (Data != null)
            {
                sb.AppendLine("data:");
                const int n = 1024;
                if (Data.Length <= n)
                {
                    sb.AppendLine(Encoding.UTF8.GetString(Data));
                }
                else
                {
                    sb.AppendLine(Encoding.UTF8.GetString(Data, 0, n));
                    sb.Append($"<--- TOO-LARGE-TO-DISPLAY --- TOTAL {Data.Length} BYTES --->");
                    sb.AppendLine();
                }
            }

            sb.AppendLine();

            sb.Append($"ref-code:{RefCode}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(RefText))
            {
                sb.AppendLine("ref-text:");
                sb.AppendLine(RefText);
            }

            if (RefInfo != null)
            {
                sb.AppendLine("ref-info:");
                foreach (var d in RefInfo) sb.AppendLine($"{d.Key}:{d.Value}");
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}
