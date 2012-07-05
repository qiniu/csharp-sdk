using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QBox
{
    public class FileParameter
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }

        public FileParameter(string fname, string mimeType)
        {
            FileName = fname;
            MimeType = mimeType;
        }
    }
}
