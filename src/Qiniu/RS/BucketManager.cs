using System;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RS.Model;
using Qiniu.Util;

#if Net45 || Net46 || NetCore || WINDOWS_UWP
using System.Threading.Tasks;
#endif

namespace Qiniu.RS
{
    /// <summary>
    /// 空间(资源)管理/操作
    /// </summary>
    public class BucketManager
    {
        private Auth auth;
        private HttpManager httpManager;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mac">账号(密钥)</param>
        public BucketManager(Mac mac)
        {
            auth = new Auth(mac);
            httpManager = new HttpManager();
        }

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        #region NET-NORMAL

        /// <summary>
        /// 获取空间文件信息
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>文件信息获取结果</returns>
        public StatResult Stat(string bucket, string key)
        {
            StatResult result = new StatResult();

            try
            {
                string statUrl = Config.ZONE.RsHost + StatOp(bucket, key);
                string token = auth.CreateManageToken(statUrl);

                HttpResult hr = httpManager.Get(statUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [stat] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取空间(bucket)列表
        /// </summary>
        /// <returns>空间列表获取结果</returns>
        public BucketsResult Buckets()
        {
            BucketsResult result = new BucketsResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/buckets";
                string token = auth.CreateManageToken(bucketsUrl);

                HttpResult hr = httpManager.Get(bucketsUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [buckets] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 查询指定bucket的信息
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <returns></returns>
        public BucketResult Bucket(string bucketName)
        {
            BucketResult result = new BucketResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/bucket/" + bucketName;
                string token = auth.CreateManageToken(bucketsUrl);

                HttpResult hr = httpManager.Get(bucketsUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [bucket] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Delete(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string deleteUrl = Config.ZONE.RsHost + DeleteOp(bucket, key);
                string token = auth.CreateManageToken(deleteUrl);

                result = httpManager.Post(deleteUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [delete] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = auth.CreateManageToken(copyUrl);

                result = httpManager.Post(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [copy] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 复制文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = auth.CreateManageToken(copyUrl);

                result = httpManager.Post(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [copy] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = auth.CreateManageToken(moveUrl);

                result = httpManager.Post(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [move] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 移动文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = auth.CreateManageToken(moveUrl);

                result = httpManager.Post(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [move] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 修改文件名(key)
        /// </summary>
        /// <param name="bucket">文件所在空间</param>
        /// <param name="oldKey">旧的文件名</param>
        /// <param name="newKey">新的文件名</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Rename(string bucket, string oldKey, string newKey)
        {
            return Move(bucket, oldKey, bucket, newKey);
        }

        /// <summary>
        /// 修改文件MimeType
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后的MIME Type</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Chgm(string bucket, string key, string mimeType)
        {
            HttpResult result = new HttpResult();

            try
            {
                string chgmUrl = Config.ZONE.RsHost + ChgmOp(bucket, key, mimeType);
                string token = auth.CreateManageToken(chgmUrl);

                result = httpManager.Post(chgmUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [chgm] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="batchOps">批量操作的操作字符串</param>
        /// <returns>状态码为200时表示OK</returns>
        public BatchResult Batch(string batchOps)
        {
            BatchResult result = new BatchResult();

            try
            {
                string batchUrl = Config.ZONE.RsHost + "/batch";
                byte[] data = Encoding.UTF8.GetBytes(batchOps);
                string token = auth.CreateManageToken(batchUrl, data);

                HttpResult hr = httpManager.PostForm(batchUrl, data, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [batch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批处理，字符串数组拼接后与另一形式等价
        /// </summary>
        /// <param name="ops">批量操作的操作字符串数组</param>
        /// <returns>状态码为200时表示OK</returns>
        public BatchResult Batch(string[] ops)
        {
            StringBuilder opsb = new StringBuilder();
            opsb.AppendFormat("op={0}", ops[0]);
            for (int i = 1; i < ops.Length; ++i)
            {
                opsb.AppendFormat("&op={0}", ops[i]);
            }

            return Batch(opsb.ToString());
        }

        /// <summary>
        /// 批处理-stat
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public BatchResult BatchStat(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = StatOp(bucket, keys[i]);
            }

            return Batch(ops);
        }

        /// <summary>
        /// 批处理 - delete
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public BatchResult BatchDelete(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = DeleteOp(bucket, keys[i]);
            }

            return Batch(ops);
        }

        /// <summary>
        /// 抓取文件
        /// </summary>
        /// <param name="resUrl">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Fetch(string resUrl, string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string fetchUrl = Config.ZONE.IovipHost + FetchOp(resUrl, bucket, key);
                string token = auth.CreateManageToken(fetchUrl);

                result = httpManager.Post(fetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [fetch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 更新文件，适用于"镜像源站"设置的空间
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult Prefetch(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string prefetchUrl = Config.ZONE.IovipHost + PrefetchOp(bucket, key);
                string token = auth.CreateManageToken(prefetchUrl);

                result = httpManager.Post(prefetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [prefetch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取空间的域名
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <returns>空间对应的域名</returns>
        public DomainsResult Domains(string bucket)
        {
            DomainsResult result = new DomainsResult();

            try
            {
                string domainsUrl = Config.ZONE.ApiHost + "/v6/domain/list";
                string body = string.Format("tbl={0}", bucket);
                byte[] data = Encoding.UTF8.GetBytes(body);
                string token = auth.CreateManageToken(domainsUrl, data);

                HttpResult hr = httpManager.PostForm(domainsUrl, data, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [domains] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 
        /// 获取空间文件列表 
        /// listFiles(bucket, prefix, marker, limit, delimiter)
        /// 
        /// bucket:    目标空间名称
        /// 
        /// prefix:    返回指定文件名前缀的文件列表(prefix可设为null)
        /// 
        /// marker:    考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作)
        ///            执行listFiles操作时使用marker标记来追加新的结果
        ///            特别注意首次执行listFiles操作时marker为null
        ///            
        /// limit:     每次返回结果所包含的文件总数限制(limit最大值1000，建议值100)
        /// 
        /// delimiter: 分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)
        ///            假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt
        ///            现设置分隔符delimiter = / 得到返回结果items =[]，commonPrefixes = [fakepath/]
        ///            然后调整prefix = fakepath/ delimiter = null 得到所需结果items = [1.txt,2.txt]
        ///            于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件
        ///            
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="prefix">前缀</param>
        /// <param name="marker">标记</param>
        /// <param name="limit">数量限制</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns>文件列表获取结果</returns>
        public ListResult ListFiles(string bucket, string prefix, string marker, int limit, string delimiter)
        {
            ListResult result = new ListResult();

            try
            {
                StringBuilder sb = new StringBuilder("/list?bucket=" + bucket);

                if (!string.IsNullOrEmpty(marker))
                {
                    sb.Append("&marker=" + marker);
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    sb.Append("&prefix=" + prefix);
                }

                if (!string.IsNullOrEmpty(delimiter))
                {
                    sb.Append("&delimiter=" + delimiter);
                }

                if (limit > 1000 || limit < 1)
                {
                    sb.Append("&limit=1000");
                }
                else
                {
                    sb.Append("&limit=" + limit);
                }

                string listUrl = Config.ZONE.RsfHost + sb.ToString();
                string token = auth.CreateManageToken(listUrl);

                HttpResult hr = httpManager.Post(listUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [listFiles] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 更新文件生命周期
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult UpdateLifecycle(string bucket, string key, int deleteAfterDays)
        {
            HttpResult result = new HttpResult();

            try
            {
                string updateUrl = Config.ZONE.RsHost + UpdateLifecycleOp(bucket, key, deleteAfterDays);
                string token = auth.CreateManageToken(updateUrl);

                result = httpManager.Post(updateUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [deleteAfterDays] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }


        #endregion NORMAL

#endif

#if Net45 || Net46 || NetCore || WINDOWS_UWP

        #region NETUWP-ASYNC

        /// <summary>
        /// [异步async]获取空间文件信息
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>文件信息获取结果</returns>
        public async Task<StatResult> StatAsync(string bucket, string key)
        {
            StatResult result = new StatResult();

            try
            {
                string statUrl = Config.ZONE.RsHost + StatOp(bucket, key);
                string token = auth.CreateManageToken(statUrl);

                HttpResult hr = await httpManager.GetAsync(statUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] stat Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]获取空间(bucket)列表
        /// </summary>
        /// <returns>空间列表获取结果</returns>
        public async Task<BucketsResult> BucketsAsync()
        {
            BucketsResult result = new BucketsResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/buckets";
                string token = auth.CreateManageToken(bucketsUrl);

                HttpResult hr = await httpManager.GetAsync(bucketsUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] buckets Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]查询指定bucket的信息
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <returns></returns>
        public async Task<BucketResult> BucketAsync(string bucketName)
        {
            BucketResult result = new BucketResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/bucket/" + bucketName;
                string token = auth.CreateManageToken(bucketsUrl);

                HttpResult hr = await httpManager.GetAsync(bucketsUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] bucket Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]删除文件
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> DeleteAsync(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string deleteUrl = Config.ZONE.RsHost + DeleteOp(bucket, key);
                string token = auth.CreateManageToken(deleteUrl);

                result = await httpManager.PostAsync(deleteUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] delete Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]复制文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> CopyAsync(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = auth.CreateManageToken(copyUrl);

                result = await httpManager.PostAsync(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] copy Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]复制文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> CopyAsync(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = auth.CreateManageToken(copyUrl);

                result = await httpManager.PostAsync(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] copy Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]移动文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> MoveAsync(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = auth.CreateManageToken(moveUrl);

                result = await httpManager.PostAsync(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] move Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]移动文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> MoveAsync(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = auth.CreateManageToken(moveUrl);

                result = await httpManager.PostAsync(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] move Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]修改文件名(key)
        /// </summary>
        /// <param name="bucket">文件所在空间</param>
        /// <param name="oldKey">旧的文件名</param>
        /// <param name="newKey">新的文件名</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> RenameAsync(string bucket, string oldKey, string newKey)
        {
            return await MoveAsync(bucket, oldKey, bucket, newKey);
        }

        /// <summary>
        /// [异步async]修改文件MimeType
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后的MIME Type</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> ChgmAsync(string bucket, string key, string mimeType)
        {
            HttpResult result = new HttpResult();

            try
            {
                string chgmUrl = Config.ZONE.RsHost + ChgmOp(bucket, key, mimeType);
                string token = auth.CreateManageToken(chgmUrl);

                result = await httpManager.PostAsync(chgmUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] chgm Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]批处理
        /// </summary>
        /// <param name="batchOps">批量操作的操作字符串</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<BatchResult> BatchAsync(string batchOps)
        {
            BatchResult result = new BatchResult();

            try
            {
                string batchUrl = Config.ZONE.RsHost + "/batch";
                byte[] data = Encoding.UTF8.GetBytes(batchOps);
                string token = auth.CreateManageToken(batchUrl, data);

                HttpResult hr = await httpManager.PostFormAsync(batchUrl, data, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] batch Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]批处理，字符串数组拼接后与另一形式等价
        /// </summary>
        /// <param name="ops">批量操作的操作字符串数组</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<BatchResult> BatchAsync(string[] ops)
        {
            StringBuilder opsb = new StringBuilder();
            opsb.AppendFormat("op={0}", ops[0]);
            for (int i = 1; i < ops.Length; ++i)
            {
                opsb.AppendFormat("&op={0}", ops[i]);
            }

            return await BatchAsync(opsb.ToString());
        }

        /// <summary>
        /// [异步async]批处理-stat
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public async Task<BatchResult> BatchStatAsync(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = StatOp(bucket, keys[i]);
            }

            return await BatchAsync(ops);
        }

        /// <summary>
        /// [异步async]批处理 - delete
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public async Task<BatchResult> BatchDeleteAsync(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = DeleteOp(bucket, keys[i]);
            }

            return await BatchAsync(ops);
        }

        /// <summary>
        /// [异步async]抓取文件
        /// </summary>
        /// <param name="resUrl">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> FetchAsync(string resUrl, string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string fetchUrl = Config.ZONE.IovipHost + FetchOp(resUrl, bucket, key);
                string token = auth.CreateManageToken(fetchUrl);

                result = await httpManager.PostAsync(fetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] fetch Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]更新文件，适用于"镜像源站"设置的空间
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> PrefetchAsync(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string prefetchUrl = Config.ZONE.IovipHost + PrefetchOp(bucket, key);
                string token = auth.CreateManageToken(prefetchUrl);

                result = await httpManager.PostAsync(prefetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] prefetch Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]获取空间的域名
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <returns>空间对应的域名</returns>
        public async Task<DomainsResult> DomainsAsync(string bucket)
        {
            DomainsResult result = new DomainsResult();

            try
            {
                string domainsUrl = Config.ZONE.ApiHost + "/v6/domain/list";
                string body = string.Format("tbl={0}", bucket);
                byte[] data = Encoding.UTF8.GetBytes(body);
                string token = auth.CreateManageToken(domainsUrl, data);

                HttpResult hr = await httpManager.PostFormAsync(domainsUrl, data, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] domains Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 
        /// [异步async]获取空间文件列表 
        /// listFiles(bucket, prefix, marker, limit, delimiter)
        /// 
        /// bucket:    目标空间名称
        /// 
        /// prefix:    返回指定文件名前缀的文件列表(prefix可设为null)
        /// 
        /// marker:    考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作)
        ///            执行listFiles操作时使用marker标记来追加新的结果
        ///            特别注意首次执行listFiles操作时marker为null
        ///            
        /// limit:     每次返回结果所包含的文件总数限制(limit最大值1000，建议值100)
        /// 
        /// delimiter: 分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)
        ///            假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt
        ///            现设置分隔符delimiter = / 得到返回结果items =[]，commonPrefixes = [fakepath/]
        ///            然后调整prefix = fakepath/ delimiter = null 得到所需结果items = [1.txt,2.txt]
        ///            于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件
        ///            
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="prefix">前缀</param>
        /// <param name="marker">标记</param>
        /// <param name="limit">数量限制</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns>文件列表获取结果</returns>
        public async Task<ListResult> ListAsync(string bucket, string prefix, string marker, int limit, string delimiter)
        {
            ListResult result = new ListResult();

            try
            {
                StringBuilder sb = new StringBuilder("/list?bucket=" + bucket);

                if (!string.IsNullOrEmpty(marker))
                {
                    sb.Append("&marker=" + marker);
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    sb.Append("&prefix=" + prefix);
                }

                if (!string.IsNullOrEmpty(delimiter))
                {
                    sb.Append("&delimiter=" + delimiter);
                }

                if (limit > 1000 || limit < 1)
                {
                    sb.Append("&limit=1000");
                }
                else
                {
                    sb.Append("&limit=" + limit);
                }

                string listUrl = Config.ZONE.RsfHost + sb.ToString();
                string token = auth.CreateManageToken(listUrl);

                HttpResult hr = await httpManager.PostAsync(listUrl, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] list Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]更新文件生命周期
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> UpdateLifecycleAsync(string bucket, string key, int deleteAfterDays)
        {
            HttpResult result = new HttpResult();

            try
            {
                string updateUrl = Config.ZONE.RsHost + UpdateLifecycleOp(bucket, key, deleteAfterDays);
                string token = auth.CreateManageToken(updateUrl);

                result = await httpManager.PostAsync(updateUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] deleteAfterDays Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        #endregion NETUWP-ASYNC

#endif


        /// <summary>
        /// 生成stat操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>stat操作字符串</returns>
        public string StatOp(string bucket, string key)
        {
            return string.Format("/stat/{0}", Base64.UrlSafeBase64Encode(bucket, key));
        }

        /// <summary>
        /// 生成delete操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>delete操作字符串</returns>
        public string DeleteOp(string bucket, string key)
        {
            return string.Format("/delete/{0}", Base64.UrlSafeBase64Encode(bucket, key));
        }

        /// <summary>
        /// 生成copy操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>copy操作字符串</returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return string.Format("/copy/{0}/{1}",
                Base64.UrlSafeBase64Encode(srcBucket, srcKey),
                Base64.UrlSafeBase64Encode(dstBucket, dstKey));
        }

        /// <summary>
        /// 生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>copy操作字符串</returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/copy/{0}/{1}/{2}",
                Base64.UrlSafeBase64Encode(srcBucket, srcKey),
                Base64.UrlSafeBase64Encode(dstBucket, dstKey), fx);
        }

        /// <summary>
        /// 生成move操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>move操作字符串</returns>
        public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return string.Format("/move/{0}/{1}",
                Base64.UrlSafeBase64Encode(srcBucket, srcKey),
                Base64.UrlSafeBase64Encode(dstBucket, dstKey));
        }

        /// <summary>
        /// 生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>move操作字符串</returns>
        public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/move/{0}/{1}/{2}",
                Base64.UrlSafeBase64Encode(srcBucket, srcKey),
                Base64.UrlSafeBase64Encode(dstBucket, dstKey), fx);
        }

        /// <summary>
        /// 生成chgm操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后MIME Type</param>
        /// <returns>chgm操作字符串</returns>
        public string ChgmOp(string bucket, string key, string mimeType)
        {
            return string.Format("/chgm/{0}/mime/{1}",
                Base64.UrlSafeBase64Encode(bucket, key),
                Base64.UrlSafeBase64Encode(mimeType));
        }

        /// <summary>
        /// 生成fetch操作字符串
        /// </summary>
        /// <param name="url">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>fetch操作字符串</returns>
        public string FetchOp(string url, string bucket, string key)
        {
            return string.Format("/fetch/{0}/to/{1}",
                Base64.UrlSafeBase64Encode(url),
                Base64.UrlSafeBase64Encode(bucket, key));
        }

        /// <summary>
        /// 生成prefetch操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>prefetch操作字符串</returns>
        public string PrefetchOp(string bucket, string key)
        {
            return string.Format("/prefetch/{0}",
                Base64.UrlSafeBase64Encode(bucket, key));
        }

        /// <summary>
        /// 生成updateLifecycle操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除(设为0表示取消)</param>
        /// <returns>updateLifecycle操作字符串</returns>
        public string UpdateLifecycleOp(string bucket,string key,int deleteAfterDays)
        {
            return string.Format("/deleteAfterDays/{0}/{1}",
                Base64.UrlSafeBase64Encode(bucket, key), deleteAfterDays);
        }

    }
}
