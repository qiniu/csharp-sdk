using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox.IO
{
    public class PutExtra
    {
        public string CallbackParams { get; set; }
        public string Bucket { get; set; }
        public string CustomMeta { get; set; }
        public string MimeType { get; set; }
        public Int64 Crc32 { get; set; }

        public PutExtra()
        {
            Crc32 = -1;
        }

        public PutExtra(string bucket, string mimeType, string callbackParams)
        {
            Bucket = bucket;
            MimeType = mimeType;
            CallbackParams = callbackParams;
            Crc32 = -1;
        }
    }
}
