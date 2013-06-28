using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QBox.IO.Resumable
{
    
    [JsonObject(MemberSerialization.OptIn)]
    public class BlkputRet
    {
        [JsonProperty("ctx")]
        public string ctx;
        [JsonProperty("checksum")]
        public string checkSum;
        [JsonProperty("crc32")]
        public UInt32 crc32;
        [JsonProperty("offset")]
        public UInt32 offset;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PutNotifyEvent:EventArgs
    {
        int blkIdx;
        int blkSize;
        BlkputRet ret;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PutNotifyErrorEvent:EventArgs
    {
        int blIdx;
        int blkSize;
        string error;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PutExtra
    {
        public Dictionary<string, string> Params { get; set; }
        public string MimeType;
        public string chunkSize;
        public string tryTimes;
        public List<BlkputRet> Progresses;
        public event EventHandler<PutNotifyEvent> Notify;
        public event EventHandler<PutNotifyErrorEvent> NotifyErr;
    }
}
