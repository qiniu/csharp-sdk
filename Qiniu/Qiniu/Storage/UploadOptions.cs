using System.Collections.Generic;
using System.Diagnostics;

namespace Qiniu.Storage
{
    /// <summary>
    /// 上传可选设置
    /// </summary>
    public class UploadOptions
    {
        //扩展变量,名词必须以x:开头，另外值不能为空
        private Dictionary<string, string> extraParams;
        //上传数据或文件的mimeType
        public string MimeType { set; get; }
        //是否对上传文件或数据做crc32校验
        public bool CheckCrc32 { set; get; }
        //上传进度处理器
        public UpProgressHandler ProgressHandler { set; get; }
        //上传取消信号
        public UpCancellationSignal CancellationSignal { set; get; }

        /// <summary>
        /// 过滤掉所有不符合规则的扩展参数
        /// </summary>
        public Dictionary<string, string> ExtraParams
        {
            get
            {
                return filterParams(extraParams);
            }
            set
            {
                this.extraParams = value;
            }
        }

        /// <summary>
        /// 构建上传可选设置对象
        /// 
        /// 在构造函数内部会设置默认的值来回避对象null检测
        /// </summary>
        /// <param name="extraParams">扩展参数</param>
        /// <param name="mimeType">文件MimeType</param>
        /// <param name="checkCrc32">检查crc32开关</param>
        /// <param name="upProgressHandler">上传进度处理器</param>
        /// <param name="upCancellationSignal">上传取消信号</param>
        public UploadOptions(Dictionary<string, string> extraParams, string mimeType, bool checkCrc32,
            UpProgressHandler upProgressHandler, UpCancellationSignal upCancellationSignal)
        {
            this.ExtraParams = extraParams;
            this.MimeType = mime(mimeType);
            this.CheckCrc32 = checkCrc32;
            this.CancellationSignal = (upCancellationSignal != null) ? upCancellationSignal : new UpCancellationSignal(delegate()
            {
                return false;
            });
            this.ProgressHandler = (upProgressHandler != null) ? upProgressHandler : new UpProgressHandler(delegate(string key, double percent)
            {
                Debug.WriteLine("qiniu up progress " + percent);
            });
        }

        /// <summary>
        /// 默认的上传可选设置对象
        /// </summary>
        /// <returns>上传可选设置对象</returns>
        public static UploadOptions defaultOptions()
        {
            return new UploadOptions(null, null, false, null, null);
        }

        /// <summary>
        /// 过滤掉所有非x:开头的或者值为空的扩展变量
        /// </summary>
        /// <param name="extraParamsToFilter">待过滤参数</param>
        /// <returns>已过滤参数</returns>
        private Dictionary<string, string> filterParams(Dictionary<string, string> extraParamsToFilter)
        {
            Dictionary<string, string> filtered = new Dictionary<string, string>();
            if (extraParamsToFilter != null)
            {
                foreach (KeyValuePair<string, string> kvp in extraParamsToFilter)
                {
                    if (kvp.Key.StartsWith("x:") && kvp.Value != null && kvp.Value.Trim().Length > 0)
                    {
                        filtered.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            return filtered;
        }

        /// <summary>
        /// 检测MimeType
        /// </summary>
        /// <param name="mimeType">mimeType参数</param>
        /// <returns>mimeType值</returns>
        private string mime(string mimeType)
        {
            if (mimeType == null || mimeType.Trim().Length == 0)
            {
                return "application/octet-stream";
            }
            return mimeType;
        }
    }
}