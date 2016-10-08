using Qiniu.Common;
using Qiniu.Http;
using Qiniu.Util;
using System.Collections.Generic;
using System.Text;

namespace Qiniu.Processing
{
    public class Pfop
    {
        private HttpManager mHttpManager;
        public Mac Mac { set; get; }

        public Pfop(Mac mac)
        {
            this.mHttpManager = new HttpManager();
            this.Mac = mac;
        }

        public PfopResult pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force)
        {
            PfopResult pfopResult = null;

            Dictionary<string, string[]> pfopParams = new Dictionary<string, string[]>();
            pfopParams.Add("bucket", new string[] { bucket });
            pfopParams.Add("key", new string[] { key });
            pfopParams.Add("fops", new string[] { fops });
            if (!string.IsNullOrEmpty(notifyUrl))
            {
                pfopParams.Add("notifyURL", new string[] { notifyUrl });
            }
            if (force)
            {
                pfopParams.Add("force", new string[] { "1" });
            }
            if (!string.IsNullOrEmpty(pipeline))
            {
                pfopParams.Add("pipeline", new string[] { pipeline });
            }

            string pfopUrl = Config.ZONE.ApiHost + "/pfop/";

            string accessToken = Auth.createManageToken(pfopUrl, Encoding.UTF8.GetBytes(StringUtils.urlValuesEncode(pfopParams)), this.Mac);
            Dictionary<string, string> pfopHeaders = new Dictionary<string, string>();
            pfopHeaders.Add("Authorization", accessToken);

            CompletionHandler pfopCompletionHandler = new CompletionHandler(delegate (ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    pfopResult = StringUtils.jsonDecode<PfopResult>(response);
                }
                else
                {
                    pfopResult = new PfopResult();
                }
                pfopResult.ResponseInfo = respInfo;
                pfopResult.Response = response;
            });

            this.mHttpManager.postForm(pfopUrl, pfopHeaders, pfopParams, pfopCompletionHandler);
            return pfopResult;
        }
        public PfopResult pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force)
        {
            string newFops = string.Join(";", fops);
            return pfop(bucket, key, newFops, pipeline, notifyUrl, force);
        }
    }
}