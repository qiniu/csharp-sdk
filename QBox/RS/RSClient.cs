using System.IO;
using System.Collections.Generic;

using System.Text;
using QBox.Util;
using QBox.RPC;
using QBox.Auth;
using QBox.Conf;

namespace QBox.RS
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public class FileHandle
    {
        /// <summary>
        /// 查看
        /// </summary>
        public const string STAT = "stat";
        /// <summary>
        /// 移动
        /// </summary>
        public const string MOVE = "move";
        /// <summary>
        /// 复制
        /// </summary>
        public const string COPY = "copy";
        /// <summary>
        /// 删除
        /// </summary>
        public const string DELETE = "delete";
    }
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

        public CallRet Move(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            string entryURISrc = bucketSrc + ":" + keySrc;
            string entryURIDest = bucketDest + ":" + keyDest;
            string url = Config.RS_HOST + "/move/" +
                Base64URLSafe.Encode(entryURISrc) + "/" + Base64URLSafe.Encode(entryURIDest);
            return Conn.Call(url);
        }

        public CallRet Copy(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            string entryURISrc = bucketSrc + ":" + keySrc;
            string entryURIDest = bucketDest + ":" + keyDest;
            string url = Config.RS_HOST + "/copy/" +
                Base64URLSafe.Encode(entryURISrc) + "/" + Base64URLSafe.Encode(entryURIDest);
            return Conn.Call(url);
        }

        private string getBatchOp_1(string opName, string bucketName, string[] keys)
        {
            if (keys.Length < 1)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Length - 1; i++)
            {
                string item = string.Format("op=/{0}/{1}&", opName, Base64URLSafe.Encode(string.Format("{0}:{1}", bucketName, keys[i])));
                sb.Append(item);
            }
            string litem = string.Format("op=/{0}/{1}", opName, Base64URLSafe.Encode(string.Format("{0}:{1}", bucketName, keys[keys.Length - 1])));
            return sb.Append(litem).ToString();
        }

        public List<Entry> BatchStat(string bucket, string[] keys)
        {
            string requestBody = getBatchOp_1(FileHandle.STAT, bucket, keys);
            CallRet ret = Conn.CallWithBinary(Conf.Config.RS_HOST + "/batch", "application/x-www-form-urlencoded", requestBody.ToStream(), requestBody.Length);
            return null;
        }

    }
}
