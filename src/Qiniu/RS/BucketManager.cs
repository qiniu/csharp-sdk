using System;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RS.Model;
using Qiniu.Util;

namespace Qiniu.RS
{
    /// <summary>
    /// 空间(资源)管理/操作
    /// </summary>
    public class BucketManager
    {
        private Signature signature;
        private HttpManager httpManager;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mac">账户访问控制(密钥)</param>
        public BucketManager(Mac mac)
        {
            signature = new Signature(mac);
            httpManager = new HttpManager();
        }

        /// <summary>
        /// 生成管理凭证
        /// 有关管理凭证请参阅
        /// http://developer.qiniu.com/article/developer/security/access-token.html
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>生成的管理凭证</returns>
        public string createManageToken(string url)
        {
            return createManageToken(url, null);
        }

        /// <summary>
        /// 生成管理凭证
        /// 有关管理凭证请参阅
        /// http://developer.qiniu.com/article/developer/security/access-token.html
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="body">请求的主体内容</param>
        /// <returns>生成的管理凭证</returns>
        public string createManageToken(string url, byte[] body)
        {
            return string.Format("QBox {0}", signature.signRequest(url, body));
        }

        /// <summary>
        /// 获取空间文件信息
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>文件信息获取结果</returns>
        public StatResult stat(string bucket, string key)
        {
            StatResult result = new StatResult();

            try
            {
                string statUrl = Config.ZONE.RsHost + statOp(bucket, key);
                string token = createManageToken(statUrl);

                HttpResult hr = httpManager.get(statUrl, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Stat] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取空间(bucket)列表
        /// </summary>
        /// <returns>空间列表获取结果</returns>
        public BucketsResult buckets()
        {
            BucketsResult result = new BucketsResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/buckets";
                string token = createManageToken(bucketsUrl);

                HttpResult hr = httpManager.get(bucketsUrl, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Buckets] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 查询指定bucket的信息
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <returns></returns>
        public BucketResult bucket(string bucketName)
        {
            BucketResult result = new BucketResult();

            try
            {
                string bucketsUrl = Config.ZONE.RsHost + "/bucket/" + bucketName;
                string token = createManageToken(bucketsUrl);

                HttpResult hr = httpManager.get(bucketsUrl, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Bucket] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult delete(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string deleteUrl = Config.ZONE.RsHost + deleteOp(bucket, key);
                string token = createManageToken(deleteUrl);

                result = httpManager.post(deleteUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Delete] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult copy(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + copyOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = createManageToken(copyUrl);

                result = httpManager.post(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Copy] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string copyUrl = Config.ZONE.RsHost + copyOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = createManageToken(copyUrl);

                result = httpManager.post(copyUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Copy] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult move(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + moveOp(srcBucket, srcKey, dstBucket, dstKey);
                string token = createManageToken(moveUrl);

                result = httpManager.post(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Move] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            try
            {
                string moveUrl = Config.ZONE.RsHost + moveOp(srcBucket, srcKey, dstBucket, dstKey, force);
                string token = createManageToken(moveUrl);

                result = httpManager.post(moveUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Move] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult rename(string bucket, string oldKey, string newKey)
        {
            return move(bucket, oldKey, bucket, newKey);
        }

        /// <summary>
        /// 修改文件MimeType
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后的MIME Type</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult chgm(string bucket, string key, string mimeType)
        {
            HttpResult result = new HttpResult();

            try
            {
                string chgmUrl = Config.ZONE.RsHost + chgmOp(bucket, key, mimeType);
                string token = createManageToken(chgmUrl);

                result = httpManager.post(chgmUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Chgm] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="batchOps">批量操作的操作字符串</param>
        /// <returns>状态码为200时表示OK</returns>
        public BatchResult batch(string batchOps)
        {
            BatchResult result = new BatchResult();

            try
            {
                string batchUrl = Config.ZONE.RsHost + "/batch";
                byte[] data = Encoding.UTF8.GetBytes(batchOps);
                string token = createManageToken(batchUrl, data);

                HttpResult hr = httpManager.postForm(batchUrl, data, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Batch] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批处理，字符串数组拼接后与另一形式等价
        /// </summary>
        /// <param name="ops">批量操作的操作字符串数组</param>
        /// <returns>状态码为200时表示OK</returns>
        public BatchResult batch(string[] ops)
        {
            StringBuilder opsb = new StringBuilder();
            opsb.AppendFormat("op={0}", ops[0]);
            for (int i = 1; i < ops.Length; ++i)
            {
                opsb.AppendFormat("&op={0}", ops[i]);
            }

            return batch(opsb.ToString());
        }

        /// <summary>
        /// 批处理-stat
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public BatchResult batchStat(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = statOp(bucket, keys[i]);
            }

            return batch(ops);
        }

        /// <summary>
        /// 批处理 - delete
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="keys">文件key列表</param>
        /// <returns>结果列表</returns>
        public BatchResult batchDelete(string bucket, string[] keys)
        {
            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = deleteOp(bucket, keys[i]);
            }

            return batch(ops);
        }
        
        /// <summary>
        /// 抓取文件
        /// </summary>
        /// <param name="resUrl">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public HttpResult fetch(string resUrl, string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string fetchUrl = Config.ZONE.IovipHost + fetchOp(resUrl, bucket, key);
                string token = createManageToken(fetchUrl);

                result = httpManager.post(fetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Fetch] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult prefetch(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            try
            {
                string prefetchUrl = Config.ZONE.IovipHost + prefetchOp(bucket, key);
                string token = createManageToken(prefetchUrl);

                result = httpManager.post(prefetchUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Prefetch] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取空间的域名
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <returns>空间对应的域名</returns>
        public DomainsResult domains(string bucket)
        {
            DomainsResult result = new DomainsResult();

            try
            {
                string domainsUrl = Config.ZONE.ApiHost + "/v6/domain/list";
                string body = string.Format("tbl={0}", bucket);
                byte[] data = Encoding.UTF8.GetBytes(body);
                string token = createManageToken(domainsUrl, data);

                HttpResult hr = httpManager.postForm(domainsUrl, data, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Domains] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public ListResult listFiles(string bucket, string prefix, string marker, int limit, string delimiter)
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
                string token = createManageToken(listUrl);

                HttpResult hr = httpManager.post(listUrl, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[List-files] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
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
        public HttpResult updateLifecycle(string bucket, string key, int deleteAfterDays)
        {
            HttpResult result = new HttpResult();

            try
            {
                string updateUrl = Config.ZONE.RsHost + updateLifecycleOp(bucket, key, deleteAfterDays);

                string token = createManageToken(updateUrl);

                result = httpManager.post(updateUrl, token);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("[Update-lifecycle] Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = HttpHelper.STATUS_CODE_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 生成stat操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>stat操作字符串</returns>
        public string statOp(string bucket, string key)
        {
            return string.Format("/stat/{0}", StringHelper.encodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成delete操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>delete操作字符串</returns>
        public string deleteOp(string bucket, string key)
        {
            return string.Format("/delete/{0}", StringHelper.encodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成copy操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>copy操作字符串</returns>
        public string copyOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return string.Format("/copy/{0}/{1}",
                StringHelper.encodedEntry(srcBucket, srcKey),
                StringHelper.encodedEntry(dstBucket, dstKey));
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
        public string copyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/copy/{0}/{1}/{2}",
                StringHelper.encodedEntry(srcBucket, srcKey),
                StringHelper.encodedEntry(dstBucket, dstKey), fx);
        }

        /// <summary>
        /// 生成move操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>move操作字符串</returns>
        public string moveOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return string.Format("/move/{0}/{1}",
                StringHelper.encodedEntry(srcBucket, srcKey),
                StringHelper.encodedEntry(dstBucket, dstKey));
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
        public string moveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/move/{0}/{1}/{2}",
                StringHelper.encodedEntry(srcBucket, srcKey),
                StringHelper.encodedEntry(dstBucket, dstKey), fx);
        }

        /// <summary>
        /// 生成chgm操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后MIME Type</param>
        /// <returns>chgm操作字符串</returns>
        public string chgmOp(string bucket, string key, string mimeType)
        {
            return string.Format("/chgm/{0}/mime/{1}",
                StringHelper.encodedEntry(bucket, key),
                StringHelper.urlSafeBase64Encode(mimeType));
        }

        /// <summary>
        /// 生成fetch操作字符串
        /// </summary>
        /// <param name="url">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>fetch操作字符串</returns>
        public string fetchOp(string url, string bucket, string key)
        {
            return string.Format("/fetch/{0}/to/{1}",
                StringHelper.urlSafeBase64Encode(url), 
                StringHelper.encodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成prefetch操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>prefetch操作字符串</returns>
        public string prefetchOp(string bucket, string key)
        {
            return string.Format("/prefetch/{0}", 
                StringHelper.encodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成updateLifecycle操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除(设为0表示取消)</param>
        /// <returns>updateLifecycle操作字符串</returns>
        public string updateLifecycleOp(string bucket,string key,int deleteAfterDays)
        {
            return string.Format("/deleteAfterDays/{0}/{1}",
                StringHelper.encodedEntry(bucket, key), deleteAfterDays);
        }

    }
}
