using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Http
{
    public enum HttpFileType
    {
        FILE_PATH,
        FILE_STREAM,
        DATA_BYTES,
        DATA_SLICE
    }
}