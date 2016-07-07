using Qiniu.Auth;
using System;
using Qiniu.RS;
using Qiniu.RPC;
using Qiniu.Conf;
using Qiniu.Util;
using Newtonsoft.Json;
#if ABOVE45
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif

namespace Qiniu.PFOP
{
    public class Mkzip
    {
#if !ABOVE45
        /// <summary>
        /// 多文件压缩存储为用户提供了批量文件的压缩存储功能
        /// POST /pfop/ HTTP/1.1
        /// Host: api.qiniu.com  
        /// Content-Type: application/x-www-form-urlencoded  
        /// Authorization: <AccessToken>  
        /// bucket = <bucket>
        /// mkzip/<mode>
        /// /url/<Base64EncodedURL>
        /// /alias/<Base64EncodedAlias>
        /// /url/<Base64EncodedURL>
        /// ...  
        /// </summary>
        public String doMkzip(String bucket, String existKey, String newFileName, String[] urls, string pipeline)
        {
            if (bucket == null || string.IsNullOrEmpty(existKey) || string.IsNullOrEmpty(newFileName) || urls.Length < 0 || pipeline == null)
            {
                throw new Exception("params error");
            }
            String entryURI = bucket + ":" + newFileName;
            String urlString = "";
            for (int i = 0; i < urls.Length; i++)
            {
                String urlEntry = "/url/" + Qiniu.Util.Base64URLSafe.ToBase64URLSafe(urls[i]);
                urlString += urlEntry;
            }

            String fop = System.Web.HttpUtility.UrlEncode("mkzip/1" + urlString + "|saveas/" + Qiniu.Util.Base64URLSafe.ToBase64URLSafe(entryURI));

            string body = string.Format("bucket={0}&key={1}&fops={2}&pipeline={3}", bucket, existKey, fop, pipeline);

            System.Text.Encoding curEncoding = System.Text.Encoding.UTF8;

            QiniuAuthClient authClient = new QiniuAuthClient();
            CallRet ret = authClient.CallWithBinary(Config.API_HOST + "/pfop/", "application/x-www-form-urlencoded", StreamEx.ToStream(body), body.Length);
            if (ret.OK)
            {
                try
                {
                    PersistentId pid = JsonConvert.DeserializeObject<PersistentId>(ret.Response);
                    return pid.persistentId;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }
#else
        public async Task<String> doMkzipAsync(String bucket, String existKey, String newFileName, String[] urls, string pipeline)
        {
            if (bucket == null || string.IsNullOrEmpty(existKey) || string.IsNullOrEmpty(newFileName) || urls.Length < 0 || pipeline == null)
            {
                throw new Exception("params error");
            }
            String entryURI = bucket + ":" + newFileName;
            String urlString = "";
            for (int i = 0; i < urls.Length; i++)
            {
                String urlEntry = "/url/" + Qiniu.Util.Base64URLSafe.ToBase64URLSafe(urls[i]);
                urlString += urlEntry;
            }

            String fop = System.Net.WebUtility.UrlEncode("mkzip/1" + urlString + "|saveas/" + Qiniu.Util.Base64URLSafe.ToBase64URLSafe(entryURI));

            string body = string.Format("bucket={0}&key={1}&fops={2}&pipeline={3}", bucket, existKey, fop, pipeline);

            System.Text.Encoding curEncoding = System.Text.Encoding.UTF8;

            QiniuAuthClient authClient = new QiniuAuthClient();

            var content = new StringContent(body);
            // StringContent 的 ContentType 默认是 text/plain
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            CallRet ret = await authClient.CallWithBinaryAsync(Config.API_HOST + "/pfop/", content);
            if (ret.OK)
            {
                try
                {
                    PersistentId pid = JsonConvert.DeserializeObject<PersistentId>(ret.Response);
                    return pid.persistentId;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }
#endif
    }
}
