using System;

namespace QBox.RS
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
