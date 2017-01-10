using System.Collections.Generic;
using System.Text;

namespace Qiniu.CDN.Model
{
    /// <summary>
    /// 文件预取-请求
    /// </summary>
    public class PrefetchRequest
    {
        /// <summary>
        /// 要预取的单个url列表，总数不超过100条
        /// 单个url，即一个具体的url，例如：http://bar.foo.com/test.zip
        /// 注意：
        /// 请输入资源 url 完整的绝对路径，由 http:// 或 https:// 开始
        /// 资源 url 不支持通配符，例如：不支持 http://www.test.com/abc/*.*
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        /// 初始化(URL列表为空，需要后续赋值)
        /// </summary>
        public PrefetchRequest()
        {
            Urls = new List<string>();
        }

        /// <summary>
        /// 初始化(URL列表)
        /// </summary>
        /// <param name="urls">URL列表</param>
        public PrefetchRequest(IEnumerable<string> urls)
        {
            Urls = new List<string>(urls);
        }

        /// <summary>
        /// 添加要查询的URL
        /// </summary>
        /// <param name="urls">URL列表</param>
        public void AddUrls(IEnumerable<string> urls)
        {
            Urls.AddRange(urls);
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <returns>请求内容的JSON字符串</returns>
        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");

            sb.Append("\"urls\":[");
            for (int i = 0; i < Urls.Count; ++i)
            {
                if (i < Urls.Count - 1)
                {
                    sb.Append(string.Format("\"{0}\",", Urls[i]));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\"", Urls[i]));
                }
            }
            sb.Append("] }");

            return sb.ToString();
        }
    }
}
