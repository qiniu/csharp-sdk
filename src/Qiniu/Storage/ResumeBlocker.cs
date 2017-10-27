using Qiniu.Http;
using System.Collections.Generic;
using System.Threading;

namespace Qiniu.Storage
{
    class ResumeBlocker
    {
        public ManualResetEvent DoneEvent { set; get; }
        public byte[] BlockBuffer { set; get; }
        public int BlockIndex { set; get; }
        public string UploadToken { set; get; }
        public PutExtra PutExtra { set; get; }
        public ResumeInfo ResumeInfo { set; get; }
        public Dictionary<int, HttpResult> BlockMakeResults;
        public object ProgressLock { set; get; }
        public Dictionary<string, long> UploadedBytesDict { set; get; }
        public long FileSize { set; get; }

        public ResumeBlocker(ManualResetEvent doneEvent, byte[] blockBuffer, int blockIndex, string uploadToken,
            PutExtra putExtra, ResumeInfo resumeInfo, Dictionary<int, HttpResult> blockMakeResults,
            object progressLock, Dictionary<string, long> uploadedBytesDict, long fileSize)
        {
            this.DoneEvent = doneEvent;
            this.BlockBuffer = blockBuffer;
            this.BlockIndex = blockIndex;
            this.UploadToken = uploadToken;
            this.PutExtra = putExtra;
            this.ResumeInfo = resumeInfo;
            this.BlockMakeResults = blockMakeResults;
            this.ProgressLock = progressLock;
            this.UploadedBytesDict = uploadedBytesDict;
            this.FileSize = fileSize;
        }
    }
}
