using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.Storage.Model;
using Qiniu.Util;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Qiniu.Storage
{
    public class BucketManager
    {
        private Mac mac;
        private HttpManager mHttpManager;

        public BucketManager(Mac mac)
        {
            this.mac = mac;
            this.mHttpManager = new HttpManager();
        }

        public StatResult stat(string bucket, string key)
        {
            StatResult statResult = null;
            string statUrl = string.Format("{0}{1}", Config.ZONE.RsHost, statOp(bucket, key));
            string accessToken = Auth.createManageToken(statUrl, null, this.mac);

            Dictionary<string, string> statHeaders = new Dictionary<string, string>();
            statHeaders.Add("Authorization", accessToken);
            CompletionHandler statCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    statResult = StringUtils.jsonDecode<StatResult>(response);
                }
                else
                {
                    statResult = new StatResult();
                }
                statResult.Response = response;
                statResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.get(statUrl, statHeaders, statCompletionHandler);
            return statResult;
        }

        public HttpResult delete(string bucket, string key)
        {
            HttpResult deleteResult = null;
            string deleteUrl = string.Format("{0}{1}", Config.ZONE.RsHost, deleteOp(bucket, key));
            string accessToken = Auth.createManageToken(deleteUrl, null, this.mac);
            Dictionary<string, string> deleteHeaders = new Dictionary<string, string>();
            deleteHeaders.Add("Authorization", accessToken);
            CompletionHandler deleteCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                deleteResult = new HttpResult();
                deleteResult.Response = response;
                deleteResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(deleteUrl, deleteHeaders, null, deleteCompletionHandler);
            return deleteResult;
        }

        public HttpResult copy(string srcBucket, string srcKey, string destBucket, string destKey)
        {
            HttpResult copyResult = null;
            string copyUrl = string.Format("{0}{1}", Config.ZONE.RsHost, copyOp(srcBucket, srcKey, destBucket, destKey));
            string accessToken = Auth.createManageToken(copyUrl, null, this.mac);
            Dictionary<string, string> copyHeaders = new Dictionary<string, string>();
            copyHeaders.Add("Authorization", accessToken);
            CompletionHandler copyCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                copyResult = new HttpResult();
                copyResult.Response = response;
                copyResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(copyUrl, copyHeaders, null, copyCompletionHandler);
            return copyResult;
        }

        // ADD 'force' param
        // 2016-08-22 14:58 @fengyh
        public HttpResult copy(string srcBucket, string srcKey, string destBucket, string destKey, bool force)
        {
            HttpResult copyResult = null;
            string copyUrl = string.Format("{0}{1}", Config.ZONE.RsHost, copyOp(srcBucket, srcKey, destBucket, destKey, force));
            string accessToken = Auth.createManageToken(copyUrl, null, this.mac);
            Dictionary<string, string> copyHeaders = new Dictionary<string, string>();
            copyHeaders.Add("Authorization", accessToken);
            CompletionHandler copyCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                copyResult = new HttpResult();
                copyResult.Response = response;
                copyResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(copyUrl, copyHeaders, null, copyCompletionHandler);
            return copyResult;
        }

        public HttpResult move(string srcBucket, string srcKey, string destBucket, string destKey)
        {
            HttpResult moveResult = null;
            string moveUrl = string.Format("{0}{1}", Config.ZONE.RsHost, moveOp(srcBucket, srcKey, destBucket, destKey));
            string accessToken = Auth.createManageToken(moveUrl, null, this.mac);
            Dictionary<string, string> moveHeaders = new Dictionary<string, string>();
            moveHeaders.Add("Authorization", accessToken);
            CompletionHandler moveCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                moveResult = new HttpResult();
                moveResult.Response = response;
                moveResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(moveUrl, moveHeaders, null, moveCompletionHandler);
            return moveResult;
        }

        // ADD 'force' param
        // 2016-08-22 14:58 @fengyh
        public HttpResult move(string srcBucket, string srcKey, string destBucket, string destKey, bool force)
        {
            HttpResult moveResult = null;
            string moveUrl = string.Format("{0}{1}", Config.ZONE.RsHost, moveOp(srcBucket, srcKey, destBucket, destKey, force));
            string accessToken = Auth.createManageToken(moveUrl, null, this.mac);
            Dictionary<string, string> moveHeaders = new Dictionary<string, string>();
            moveHeaders.Add("Authorization", accessToken);
            CompletionHandler moveCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                moveResult = new HttpResult();
                moveResult.Response = response;
                moveResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(moveUrl, moveHeaders, null, moveCompletionHandler);
            return moveResult;
        }

        public HttpResult chgm(string bucket, string key, string mimeType)
        {
            HttpResult chgmResult = null;
            string chgmUrl = string.Format("{0}{1}", Config.ZONE.RsHost, chgmOp(bucket, key, mimeType));
            string accessToken = Auth.createManageToken(chgmUrl, null, this.mac);
            Dictionary<string, string> chgmHeaders = new Dictionary<string, string>();
            chgmHeaders.Add("Authorization", accessToken);
            CompletionHandler chgmCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                chgmResult = new HttpResult();
                chgmResult.Response = response;
                chgmResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(chgmUrl, chgmHeaders, null, chgmCompletionHandler);
            return chgmResult;
        }

        public HttpResult batch(string ops)
        {
            HttpResult batchResult = null;
            string batchUrl = string.Format("{0}{1}", Config.ZONE.RsHost, "/batch");
            string accessToken = Auth.createManageToken(batchUrl, Encoding.UTF8.GetBytes(ops), this.mac);
            Dictionary<string, string> batchHeaders = new Dictionary<string, string>();
            batchHeaders.Add("Authorization", accessToken);
            CompletionHandler batchCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                batchResult = new FetchResult();
                batchResult.Response = response;
                batchResult.ResponseInfo = respInfo;
            });


            this.mHttpManager.postData(batchUrl, batchHeaders, Encoding.UTF8.GetBytes(ops),
                HttpManager.FORM_MIME_URLENCODED, batchCompletionHandler);
            return batchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ops"></param>
        /// <returns></returns>
        public HttpResult batch(string[] ops)
        {
            HttpResult batchResult = null;
            string batchUrl = string.Format("{0}{1}", Config.ZONE.RsHost, "/batch");

            StringBuilder opsBuilder = new StringBuilder();
            for (int i = 0; i < ops.Length; i++)
            {
                opsBuilder.AppendFormat("op={0}", ops[i]);
                if (i < ops.Length - 1)
                {
                    opsBuilder.Append("&");
                }
            }
            string accessToken = Auth.createManageToken(batchUrl, Encoding.UTF8.GetBytes(opsBuilder.ToString()), this.mac);
            Dictionary<string, string> batchHeaders = new Dictionary<string, string>();
            batchHeaders.Add("Authorization", accessToken);
            CompletionHandler batchCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                batchResult = new FetchResult();
                batchResult.Response = response;
                batchResult.ResponseInfo = respInfo;
            });

            Dictionary<string, string[]> batchParams = new Dictionary<string, string[]>();
            batchParams.Add("op", ops);
            this.mHttpManager.postForm(batchUrl, batchHeaders, batchParams, batchCompletionHandler);
            return batchResult;
        }

        public FetchResult fetch(string remoteResUrl, string bucket, string key)
        {
            FetchResult fetchResult = null;
            string fetchUrl = string.Format("{0}{1}", Config.ZONE.IovipHost, fetchOp(remoteResUrl, bucket, key));
            string accessToken = Auth.createManageToken(fetchUrl, null, this.mac);
            Dictionary<string, string> fetchHeaders = new Dictionary<string, string>();
            fetchHeaders.Add("Authorization", accessToken);
            CompletionHandler fetchCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    fetchResult = StringUtils.jsonDecode<FetchResult>(response);
                }
                else
                {
                    fetchResult = new FetchResult();
                }
                fetchResult.Response = response;
                fetchResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(fetchUrl, fetchHeaders, null, fetchCompletionHandler);
            return fetchResult;
        }

        public HttpResult prefetch(string bucket, string key)
        {
            HttpResult prefetchResult = null;
            string prefetchUrl = string.Format("{0}{1}", Config.ZONE.IovipHost, prefetchOp(bucket, key));
            string accessToken = Auth.createManageToken(prefetchUrl, null, this.mac);
            Dictionary<string, string> prefetchHeaders = new Dictionary<string, string>();
            prefetchHeaders.Add("Authorization", accessToken);
            CompletionHandler prefetchCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                prefetchResult = new FetchResult();
                prefetchResult.Response = response;
                prefetchResult.ResponseInfo = respInfo;
            });

            this.mHttpManager.postForm(prefetchUrl, prefetchHeaders, null, prefetchCompletionHandler);
            return prefetchResult;
        }

        public BucketsResult buckets()
        {
            BucketsResult bucketsResult = null;
            List<string> buckets = new List<string>();
            string bucketsUrl = string.Format("{0}/buckets", Config.ZONE.RsHost);
            string accessToken = Auth.createManageToken(bucketsUrl, null, this.mac);
            Dictionary<string, string> bucketsHeaders = new Dictionary<string, string>();
            bucketsHeaders.Add("Authorization", accessToken);
            CompletionHandler bucketsCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                bucketsResult = new BucketsResult();
                bucketsResult.Response = response;
                bucketsResult.ResponseInfo = respInfo;
                if (respInfo.isOk())
                {
                    buckets = JsonConvert.DeserializeObject<List<string>>(response);
                    bucketsResult.Buckets = buckets;
                }
            });

            this.mHttpManager.get(bucketsUrl, bucketsHeaders, bucketsCompletionHandler);
            return bucketsResult;
        }

        public DomainsResult domains(string bucket)
        {
            DomainsResult domainsResult = null;
            List<string> domains = new List<string>();
            string domainsUrl = string.Format("{0}/v6/domain/list", Config.ZONE.ApiHost);
            string postBody = string.Format("tbl={0}", bucket);
            string accessToken = Auth.createManageToken(domainsUrl, Encoding.UTF8.GetBytes(postBody), this.mac);

            Dictionary<string, string> domainsHeaders = new Dictionary<string, string>();
            domainsHeaders.Add("Authorization", accessToken);

            CompletionHandler domainsCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                domainsResult = new DomainsResult();
                domainsResult.Response = response;
                domainsResult.ResponseInfo = respInfo;
                if (respInfo.isOk())
                {
                    domains = JsonConvert.DeserializeObject<List<string>>(response);
                    domainsResult.Domains = domains;
                }
            });

            Dictionary<string, string[]> postParams = new Dictionary<string, string[]>();
            postParams.Add("tbl", new string[] { bucket });
            this.mHttpManager.postForm(domainsUrl, domainsHeaders, postParams, domainsCompletionHandler);
            return domainsResult;
        }

        public CdnRefreshResult cdnRefresh(List<string> urls, List<string> dirs)
        {
            CdnRefreshResult result = null;
            if (urls == null && dirs == null)
            {
                return result;
            }

            int urlsCount = 0;
            int dirsCount = 0;
            if (urls != null)
            {
                urlsCount = urls.Count;
            }
            if (dirs != null)
            {
                dirsCount = dirs.Count;
            }

            if (urlsCount == 0 && dirsCount == 0)
            {
                return result;
            }

            CdnRefreshRequest req = new CdnRefreshRequest();
            if (urls != null && urls.Count > 0)
            {
                req.Urls = urls;
            }
            if (dirs != null && dirs.Count > 0)
            {
                req.Dirs = dirs;
            }

            string postData = JsonConvert.SerializeObject(req);
            string refreshUrl = string.Format("{0}/refresh", Config.FUSION_API_HOST);
            string accessToken = Auth.createManageToken(refreshUrl, null, this.mac);
            Dictionary<string, string> refreshHeaders = new Dictionary<string, string>();
            refreshHeaders.Add("Authorization", accessToken);

            CompletionHandler refreshCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                result = new CdnRefreshResult();
                result.Response = response;
                result.ResponseInfo = respInfo;
                if (respInfo.isOk())
                {
                    result = JsonConvert.DeserializeObject<CdnRefreshResult>(response);
                }
            });

            this.mHttpManager.postData(refreshUrl, refreshHeaders, Encoding.UTF8.GetBytes(postData),
                HttpManager.FORM_MIME_JSON, refreshCompletionHandler);
            return result;
        }

        public string statOp(string bucket, string key)
        {
            return string.Format("/stat/{0}", StringUtils.encodedEntry(bucket, key));
        }

        public string deleteOp(string bucket, string key)
        {
            return string.Format("/delete/{0}", StringUtils.encodedEntry(bucket, key));
        }

        public string copyOp(string srcBucket, string srcKey, string destBucket, string destKey)
        {
            return string.Format("/copy/{0}/{1}", StringUtils.encodedEntry(srcBucket, srcKey),
                StringUtils.encodedEntry(destBucket, destKey));
        }

        // ADD 'force' param
        // 2016-08-22 14:58 @fengyh
        public string copyOp(string srcBucket, string srcKey, string destBucket, string destKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/copy/{0}/{1}/{2}", StringUtils.encodedEntry(srcBucket, srcKey),
                StringUtils.encodedEntry(destBucket, destKey), fx);
        }

        public string moveOp(string srcBucket, string srcKey, string destBucket, string destKey)
        {
            return string.Format("/move/{0}/{1}", StringUtils.encodedEntry(srcBucket, srcKey),
                StringUtils.encodedEntry(destBucket, destKey));
        }

        // ADD 'force' param
        // 2016-08-22 14:58 @fengyh
        public string moveOp(string srcBucket, string srcKey, string destBucket, string destKey, bool force)
        {
            string fx = force ? "force/true" : "force/false";
            return string.Format("/move/{0}/{1}/{2}", StringUtils.encodedEntry(srcBucket, srcKey),
                StringUtils.encodedEntry(destBucket, destKey), fx);
        }
        public string chgmOp(string bucket, string key, string mimeType)
        {
            return string.Format("/chgm/{0}/mime/{1}", StringUtils.encodedEntry(bucket, key),
                StringUtils.urlSafeBase64Encode(mimeType));
        }

        public string fetchOp(string url, string bucket, string key)
        {
            return string.Format("/fetch/{0}/to/{1}", StringUtils.urlSafeBase64Encode(url),
                StringUtils.encodedEntry(bucket, key));
        }

        public string prefetchOp(string bucket, string key)
        {
            return string.Format("/prefetch/{0}", StringUtils.encodedEntry(bucket, key));
        }
    }
}
