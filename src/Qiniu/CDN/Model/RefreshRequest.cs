using System.Collections.Generic;
using System.Text;

namespace Qiniu.CDN.Model
{
    /// <summary>
    /// 缓存刷新-请求
    /// </summary>
    public class RefreshRequest
    {
        /// <summary>
        /// 要预取的单个url列表，总数不超过100条
        /// 单个url，即一个具体的url，例如：http://bar.foo.com/test.zip
        /// 注意：
        /// 请输入资源 url 完整的绝对路径，由 http:// 或 https:// 开始
        /// 资源 url 不支持通配符，例如：不支持 http://www.test.com/abc/*.*
        /// 带参数的 url 刷新，根据其域名缓存配置是否忽略参数缓存决定刷新结果。
        /// 如果配置了时间戳防盗链的资源 url 提交时刷新需要去掉 e 和 token 参数
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        /// 要刷新的目录url列表，总数不超过10条；目录dir，即表示一个目录级的url，需要以 / 结尾
        /// 例如：http://bar.foo.com/dir/，
        /// 也支持在尾部使用通配符，例如：http://bar.foo.com/dir/*
        /// </summary>
        public List<string> Dirs { get; set; }

        /// <summary>
        /// 初始化(所有成员为空，需要后续赋值)
        /// </summary>
        public RefreshRequest()
        {
            Urls = new List<string>();
            Dirs = new List<string>();
        }

        /// <summary>
        /// 初始化URL列表
        /// </summary>
        /// <param name="urls">URL列表</param>
        /// <param name="dirs">URL目录列表</param>
        public RefreshRequest(IEnumerable<string> urls,IEnumerable<string> dirs)
        {
            Urls = new List<string>(urls);
            Dirs = new List<string>(dirs);
        }

        /// <summary>
        /// 添加URL列表
        /// </summary>
        /// <param name="urls">URL列表</param>
        public void AddUrls(IEnumerable<string> urls)
        {
            Urls.AddRange(urls);
        }

        /// <summary>
        /// 添加URL目录列表
        /// </summary>
        /// <param name="dirs">URL目录列表</param>
        public void AddDirs(IEnumerable<string> dirs)
        {
            Dirs.AddRange(dirs);
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
            sb.Append("], ");

            sb.Append("\"dirs\":[");
            for (int i = 0; i < Dirs.Count; ++i)
            {
                if (i < Dirs.Count - 1)
                {
                    sb.Append(string.Format("\"{0}\",", Dirs[i]));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\"", Dirs[i]));
                }
            }
            sb.Append("] }");

            return sb.ToString();
        }

    }
}
