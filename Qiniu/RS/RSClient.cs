using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Auth.digest;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.Util;

namespace Qiniu.RS
{
    /// <summary>
    /// 文件管理操作
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

    /// <summary>
    /// 资源存储客户端，提供对文件的查看（stat），移动(move)，复制（copy）,删除（delete）操作
    /// 以及与这些操作对应的批量操作
    /// </summary>
    public class RSClient :QiniuAuthClient
    {


        public RSClient(Mac mac=null)
            : base(mac)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private CallRet op(string op, EntryPath scope)
        {
            string url = string.Format("{0}/{1}/{2}",
                Config.RS_HOST,
                op,
                Base64URLSafe.Encode(scope.URI));
            return Call(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        private CallRet op2(string op, EntryPathPair pair)
        {
            string url = string.Format("{0}/{1}/{2}/{3}",
                Config.RS_HOST,
                op,
                Base64URLSafe.Encode(pair.URISrc),
                Base64URLSafe.Encode(pair.URIDest));
            return Call(url);
        }
        /// <summary>
        /// 文件信息查看
        /// </summary>
        /// <param name="scope"></param>
        /// <returns>文件的基本信息，见<see cref="Entry">Entry</see></returns>
        public Entry Stat(EntryPath scope)
        {
            CallRet callRet = op(FileHandle.STAT, scope);
            return new Entry(callRet);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucket">七牛云存储空间名称</param>
        /// <param name="key">需要删除的文件key</param>
        /// <returns></returns>
        public CallRet Delete(EntryPath scope)
        {
            CallRet callRet = op(FileHandle.DELETE, scope);
            return new Entry(callRet);
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="bucketSrc">文件所属的源空间名称</param>
        /// <param name="keySrc">源key</param>
        /// <param name="bucketDest">目标空间名称</param>
        /// <param name="keyDest">目标key</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public CallRet Move(EntryPathPair pathPair)
        {
            return op2(FileHandle.MOVE, pathPair);
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="bucketSrc">文件所属的空间名称</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">复制至目标空间</param>
        /// <param name="keyDest">复制的副本文件key</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public CallRet Copy(EntryPathPair pathPair)
        {
            return op2(FileHandle.COPY, pathPair);
        }
        /// <summary>
        /// 获取一元批操作http request Body
        /// </summary>
        /// <param name="opName">操作名</param>
        /// <param name="keys">操作对象keys</param>
        /// <returns>Request Body</returns>
        private string getBatchOp_1(string opName, EntryPath[] keys)
        {
            if (keys.Length < 1)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Length - 1; i++)
            {
                string item = string.Format("op=/{0}/{1}&",
                    opName, 
                    Base64URLSafe.Encode(keys[i].URI));
                sb.Append(item);
            }
            string litem = string.Format("op=/{0}/{1}", opName, Base64URLSafe.Encode(keys[keys.Length - 1].URI));
            return sb.Append(litem).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private string getBatchOp_2(string opName, EntryPathPair[] keys)
        {
            if (keys.Length < 1)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Length - 1; i++)
            {
                string item = string.Format("op=/{0}/{1}/{2}&", opName,
                    Base64URLSafe.Encode(keys[i].URISrc),
                    Base64URLSafe.Encode(keys[i].URIDest));
                sb.Append(item);
            }
            string litem = string.Format("op=/{0}/{1}/{2}", opName,
                    Base64URLSafe.Encode(keys[keys.Length - 1].URISrc),
                    Base64URLSafe.Encode(keys[keys.Length - 1].URIDest));
            return sb.Append(litem).ToString();
        }
        private CallRet batch(string requestBody)
        {
            return CallWithBinary(Conf.Config.RS_HOST + "/batch", "application/x-www-form-urlencoded", requestBody.ToStream(), requestBody.Length);
        }
        /// <summary>
        /// 批操作：文件信息查看
        /// <example>
        /// <code>
        /// public static void BatchStat(string bucket, string[] keys)
        ///{
        ///    RSClient client = new RSClient();
        ///    List<Scope> scopes= new List<Scope>();
        ///    foreach(string key in keys)
        ///    {
        ///        Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
        ///        scopes.Add(new Scope(bucket,key));
        ///    }
        ///    client.BatchStat(scopes.ToArray()); 
        ///}
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="keys">文件bucket+key,see<see cref="Scope"/></param>
        /// <returns></returns>
        public List<BatchRetItem> BatchStat(EntryPath[] keys)
        {
            string requestBody = getBatchOp_1(FileHandle.STAT, keys);
            CallRet ret = batch(requestBody);
            if (ret.OK)
            {
                List<BatchRetItem> items = JsonConvert.DeserializeObject<List<BatchRetItem>>(ret.Response);
                return items;
            }
            return null;
        }
        /// <summary>
        /// 批操作：文件移动
        /// </summary>
        /// <param name="entryPathPair"><see cref="">EntryPathPair</see></param>
        public CallRet BatchMove(EntryPathPair[] entryPathPairs)
        {
            string requestBody = getBatchOp_2(FileHandle.MOVE, entryPathPairs);
            return batch(requestBody);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="entryPathPari"></param>
        public CallRet BatchCopy(EntryPathPair[] entryPathPari)
        {
            string requestBody = getBatchOp_2(FileHandle.COPY, entryPathPari);
            return batch(requestBody);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public CallRet BatchDelete(EntryPath[] keys)
        {
            string requestBody = getBatchOp_1(FileHandle.DELETE, keys);
            return batch(requestBody);
        }
    }
}
