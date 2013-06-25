using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox.RS
{
    public class Scope
    {
        string bucket;
        string key;
        public Scope(string bucket, string key)
        {
            this.bucket = bucket;
            this.key = key;
        }
        public string URI { get { return bucket + ":" + key; } }
    }

    /// <summary>
    /// 二元操作路径
    /// </summary>
    public class EntryPathPair
    {
        private string bucketSrc;
        private string bucketDest;
        private string keySrc;
        private string keyDest;
        Scope src;
        Scope dest;
        private void _entryPathPair(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            this.src = new Scope(bucketSrc, keySrc);
            this.dest = new Scope(bucketDest, keyDest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketSrc"></param>
        /// <param name="keySrc"></param>
        /// <param name="bucketDest"></param>
        /// <param name="keyDest"></param>
        public EntryPathPair(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            _entryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="keySrc"></param>
        /// <param name="keyDest"></param>
        public EntryPathPair(string bucket, string keySrc, string keyDest)
        {
            _entryPathPair(bucket, keySrc, bucket, keyDest);
        }
        /// <summary>
        /// bucketSrc+":"+keySrc
        /// </summary>
        public string URISrc { get { return src.URI; } }
        /// <summary>
        /// bucketDest+":"+keyDest
        /// </summary>
        public string URIDest { get { return dest.URI; } }
    }
}
