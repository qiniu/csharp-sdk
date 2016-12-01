using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;

namespace Qiniu.Processing
{
    public class Dfop
    {
        private HttpManager mHttpManager;
        public Mac mac { set; get; }

        private const string API_HOST = "http://api.qiniu.com";

        public Dfop(Mac mac)
        {
            this.mHttpManager = new HttpManager();
            this.mac = mac;
        }

        public DfopResult dfop(string fop,string url)
        {
            DfopResult dfopResult = new DfopResult();

            string encodedUrl = StringUtils.urlencode(url);

            string dfopUrl = string.Format("{0}/dfop?fop={1}&url={2}", API_HOST, fop, encodedUrl);
            string token = Auth.createManageToken(dfopUrl, null, mac);

            Dictionary<string, string> dfopHeaders = new Dictionary<string, string>();
            dfopHeaders.Add("Authorization", token);

            CompletionHandler dfopCompletionHandler = new CompletionHandler(delegate (ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    dfopResult = StringUtils.jsonDecode<DfopResult>(response);
                }

                dfopResult.ResponseInfo = respInfo;
                dfopResult.Response = response;
            });

            mHttpManager.postForm(dfopUrl, dfopHeaders, null, dfopCompletionHandler);

            return dfopResult;
        }

        public DfopResult dfop(string fop,byte[] data)
        {
            DfopResult dfopResult = new DfopResult();
            HttpFormFile dfopData = HttpFormFile.NewFileFromBytes("netFx.png", "application/octet-stream", data);

            string dfopUrl = string.Format("{0}/dfop?fop={1}", API_HOST, fop);
            string token = Auth.createManageToken(dfopUrl, null, mac);

            Dictionary<string, string> dfopHeaders = new Dictionary<string, string>();
            dfopHeaders.Add("Authorization", token);            

            CompletionHandler dfopCompletionHandler = new CompletionHandler(delegate (ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    dfopResult = StringUtils.jsonDecode<DfopResult>(response);
                }

                dfopResult.ResponseInfo = respInfo;
                dfopResult.Response = response;
            });

            mHttpManager.postMultipartDataRaw(dfopUrl, dfopHeaders, dfopData, null, dfopCompletionHandler);

            return dfopResult;
        }

    }
}
