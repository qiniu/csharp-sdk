using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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
        private HttpClient client;

        public BucketManager(Mac mac)
        {
            signature = new Signature(mac);
            client = new HttpClient();
        }

        /// <summary>
        /// 生成管理凭证
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string CreateManageToken(string url)
        {
            return CreateManageToken(url, null);
        }

        /// <summary>
        /// 生成管理凭证
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public string CreateManageToken(string url, byte[] body)
        {
            return string.Format("QBox {0}", signature.SignRequest(url, body));
        }

        /// <summary>
        /// 获取空间文件信息
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件名称</param>
        /// <returns></returns>
        public StatResult Stat(string bucket,string key)
        {
            StatResult result = new StatResult();

            string statUrl = Config.ZONE.RsHost + StatOp(bucket, key);
            string token = CreateManageToken(statUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, statUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.StatInfo = JsonConvert.DeserializeObject<StatInfo>(ret.Result);                
            }
            catch(Exception ex)
            {
                result.Message = "[stat] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取空间(bucket)列表
        /// </summary>
        /// <returns></returns>
        public BucketsResult Buckets()
        {
            BucketsResult result = new BucketsResult();

            string bucketsUrl = Config.ZONE.RsHost + "/buckets";
            string token = CreateManageToken(bucketsUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, bucketsUrl);
            req.Headers.Add("Authorization", token);

            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Buckets = JsonConvert.DeserializeObject<List<string>>(ret.Result);
            }
            catch(Exception ex)
            {
                result.Message = "[buckets] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public HttpResult Delete(string bucket,string key)
        {
            HttpResult result = new HttpResult();

            string deleteUrl = Config.ZONE.RsHost + DeleteOp(bucket, key);
            string token = CreateManageToken(deleteUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, deleteUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[delete] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <returns></returns>
        public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey);
            string token = CreateManageToken(copyUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, copyUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[copy] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 复制文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            string copyUrl = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey, force);
            string token = CreateManageToken(copyUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, copyUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[copy] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <returns></returns>
        public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            HttpResult result = new HttpResult();

            string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey);
            string token = CreateManageToken(moveUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, moveUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[move] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 移动文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            HttpResult result = new HttpResult();

            string moveUrl = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey, force);
            string token = CreateManageToken(moveUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, moveUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[move] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改文件MimeType
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public HttpResult Chgm(string bucket, string key,string mimeType)
        {
            HttpResult result = new HttpResult();

            string chgmUrl = Config.ZONE.RsHost +ChgmOp(bucket, key,mimeType);
            string token = CreateManageToken(chgmUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, chgmUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[chgm] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="batchOps"></param>
        /// <returns></returns>
        public HttpResult Batch(string batchOps)
        {
            HttpResult result = new HttpResult();

            string batchUrl = Config.ZONE.RsHost + "/batch";
            byte[] content = Encoding.UTF8.GetBytes(batchOps);
            string token = CreateManageToken(batchUrl, content);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, batchUrl);
            req.Headers.Add("Authorization", token);
            req.Content = new ByteArrayContent(content);
            req.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[batch] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="ops"></param>
        /// <returns></returns>
        public HttpResult Batch(string[] ops)
        {
            StringBuilder opsb = new StringBuilder();
            opsb.AppendFormat("op={0}", ops[0]);
            for(int i=1;i<ops.Length;++i)
            {
                opsb.AppendFormat("&op={0}", ops[i]);
            }

            return Batch(opsb.ToString());
        }

        /// <summary>
        /// 批处理-stat
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public StatResult[] BatchStat(string bucket, string[] keys)
        {
            StatResult[] results = new StatResult[keys.Length];

            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = StatOp(bucket, keys[i]);
                results[i] = new StatResult();
            }

            HttpResult batchResult = Batch(ops);
            List<BatchInfo> infos = JsonConvert.DeserializeObject<List<BatchInfo>>(batchResult.Message);
            for (int i = 0; i < keys.Length; ++i)
            {
                results[i].StatusCode = infos[i].Code;
                results[i].StatInfo = JsonConvert.DeserializeObject<StatInfo>(infos[i].Data.ToString());
                results[i].Message = infos[i].Data.ToString();
            }
            return results;
        }

        /// <summary>
        /// 批处理 - delete
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public HttpResult[] BatchDelete(string bucket, string[] keys)
        {
            HttpResult[] results = new HttpResult[keys.Length];

            string[] ops = new string[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                ops[i] = DeleteOp(bucket, keys[i]);
                results[i] = new HttpResult();
            }

            HttpResult deleteResult = Batch(ops);
            List<BatchInfo> infos = JsonConvert.DeserializeObject<List<BatchInfo>>(deleteResult.Message);
            for (int i = 0; i < keys.Length; ++i)
            {
                results[i].StatusCode = infos[i].Code;
                if (infos[i].Data != null)
                {
                    results[i].Message = infos[i].Data.ToString();
                }
            }
            return results;
        }

        /// <summary>
        /// 抓取文件
        /// </summary>
        /// <param name="resUrl"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public HttpResult Fetch(string resUrl, string bucket, string key)
        {
            HttpResult result = new HttpResult();

            string fetchUrl = Config.ZONE.IovipHost + FetchOp(resUrl, bucket, key);
            string token = CreateManageToken(fetchUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, fetchUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[fetch] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新文件，适用于"镜像源站"设置的空间
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public HttpResult Prefetch(string bucket, string key)
        {
            HttpResult result = new HttpResult();

            string prefetchUrl = Config.ZONE.IovipHost + PrefetchOp(bucket, key);
            string token = CreateManageToken(prefetchUrl);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, prefetchUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[prefetch] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取空间的域名
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public DomainsResult Domains(string bucket)
        {
            DomainsResult result = new DomainsResult();

            string domainsUrl = Config.ZONE.ApiHost + "/v6/domain/list";
            string body = string.Format("tbl={0}", bucket);
            byte[] content = Encoding.UTF8.GetBytes(body);
            string token = CreateManageToken(domainsUrl, content);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, domainsUrl);
            req.Headers.Add("Authorization", token);
            req.Content = new ByteArrayContent(content);
            req.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Domains = JsonConvert.DeserializeObject<List<string>>(ret.Result);
            }
            catch (Exception ex)
            {
                result.Message = "[domains] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 
        /// 获取空间文件列表 
        /// list(bucket, prefix, marker, limit, delimiter)
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
        public ListResult List(string bucket, string prefix, string marker, int limit, string delimiter)
        {
            ListResult result = new ListResult();

            StringBuilder sb = new StringBuilder("bucket=" + bucket);

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

            string listUrl = Config.ZONE.RsfHost + "/list?" + sb.ToString();
            string token = CreateManageToken(listUrl);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, listUrl);
            req.Headers.Add("Authorization", token);
            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.ListInfo = JsonConvert.DeserializeObject<ListInfo>(ret.Result);
            }
            catch (Exception ex)
            {
                result.Message = "[list] Error: "+ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 生成stat操作字符串
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string StatOp(string bucket, string key)
        {
            return string.Format("/stat/{0}", StringHelper.EncodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成delete操作字符串
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DeleteOp(string bucket, string key)
        {
            return string.Format("/delete/{0}", StringHelper.EncodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成copy操作字符串
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <returns></returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return string.Format("/copy/{0}/{1}", 
                StringHelper.EncodedEntry(srcBucket, srcKey),
                StringHelper.EncodedEntry(dstBucket, dstKey));
        }

        /// <summary>
        /// 生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="dstBucket"></param>
        /// <param name="dstKey"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/copy/{0}/{1}/{2}", 
                StringHelper.EncodedEntry(srcBucket, srcKey),
                StringHelper.EncodedEntry(dstBucket, dstKey), fx);
        }

        /// <summary>
        /// 生成move操作字符串
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="destBucket"></param>
        /// <param name="destKey"></param>
        /// <returns></returns>
        public string MoveOp(string srcBucket, string srcKey, string destBucket, string destKey)
        {
            return string.Format("/move/{0}/{1}", 
                StringHelper.EncodedEntry(srcBucket, srcKey),
                StringHelper.EncodedEntry(destBucket, destKey));
        }

        /// <summary>
        /// 生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket"></param>
        /// <param name="srcKey"></param>
        /// <param name="destBucket"></param>
        /// <param name="destKey"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public string MoveOp(string srcBucket, string srcKey, string destBucket, string destKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/move/{0}/{1}/{2}", 
                StringHelper.EncodedEntry(srcBucket, srcKey),
                StringHelper.EncodedEntry(destBucket, destKey), fx);
        }

        /// <summary>
        /// 生成chgm操作字符串
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public string ChgmOp(string bucket, string key, string mimeType)
        {
            return string.Format("/chgm/{0}/mime/{1}", 
                StringHelper.EncodedEntry(bucket, key),
                StringHelper.UrlSafeBase64Encode(mimeType));
        }

        /// <summary>
        /// 生成fetch操作字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string FetchOp(string url, string bucket, string key)
        {
            return string.Format("/fetch/{0}/to/{1}", 
                StringHelper.UrlSafeBase64Encode(url),
                StringHelper.EncodedEntry(bucket, key));
        }

        /// <summary>
        /// 生成prefetch操作字符串
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string PrefetchOp(string bucket, string key)
        {
            return string.Format("/prefetch/{0}", StringHelper.EncodedEntry(bucket, key));
        }

    }
}
