using System.Collections.Generic;
using System.Threading;
using Qiniu.Http;

namespace Qiniu.Storage
{
    internal class ResumeBlocker
    {
        public Dictionary<long, HttpResult> BlockMakeResults;

        public ResumeBlocker(
            ManualResetEvent doneEvent,
            byte[] blockBuffer,
            long blockIndex,
            string uploadToken,
            PutExtra putExtra,
            ResumeInfo resumeInfo,
            Dictionary<long, HttpResult> blockMakeResults,
            object progressLock,
            Dictionary<string, long> uploadedBytesDict,
            long fileSize)
        {
            DoneEvent = doneEvent;
            BlockBuffer = blockBuffer;
            BlockIndex = blockIndex;
            UploadToken = uploadToken;
            PutExtra = putExtra;
            ResumeInfo = resumeInfo;
            BlockMakeResults = blockMakeResults;
            ProgressLock = progressLock;
            UploadedBytesDict = uploadedBytesDict;
            FileSize = fileSize;
        }

        public ManualResetEvent DoneEvent { set; get; }
        public byte[] BlockBuffer { set; get; }
        public long BlockIndex { set; get; }
        public string UploadToken { set; get; }
        public PutExtra PutExtra { set; get; }
        public ResumeInfo ResumeInfo { set; get; }
        public object ProgressLock { set; get; }
        public Dictionary<string, long> UploadedBytesDict { set; get; }
        public long FileSize { set; get; }
    }
}
