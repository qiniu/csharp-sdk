using Qiniu.Common;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.Processing
{
    public class Prefop
    {
        private HttpManager mHttpManager;
        public string PersistentId { set; get; }
        public Prefop(string persistentId)
        {
            this.mHttpManager = new HttpManager();
            this.PersistentId = persistentId;
        }

        public PrefopResult prefop()
        {
            PrefopResult prefopResult = null;

            CompletionHandler prefopCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    prefopResult = StringUtils.jsonDecode<PrefopResult>(response);
                }
                else
                {
                    prefopResult = new PrefopResult();
                }
                prefopResult.ResponseInfo = respInfo;
                prefopResult.Response = response;
            });
            string qUrl = string.Format(Config.ZONE.ApiHost + "/status/get/prefop?id={0}", this.PersistentId);
            this.mHttpManager.get(qUrl, null, prefopCompletionHandler);
            return prefopResult;
        }
    }
}
