using QBox.Util;
using QBox.RPC;
using QBox.Auth;
using QBox.Conf;

namespace QBox.RS
{
    public class RSClient
    {
        public Client Conn { get; private set; }

        public RSClient()
        {
            Conn = new QBoxAuthClient();
        }

        public Entry Stat(string bucket, string key)
        {
            string entryURI = bucket + ":" + key;
            string url = Config.RS_HOST + "/stat/" + Base64URLSafe.Encode(entryURI);
            CallRet callRet = Conn.Call(url);
            return new Entry(callRet);
        }

        public CallRet Delete(string bucket, string key)
        {
            string entryURI = bucket + ":" + key;
            string url = Config.RS_HOST + "/delete/" + Base64URLSafe.Encode(entryURI);
            return Conn.Call(url);
        }
    }
}
