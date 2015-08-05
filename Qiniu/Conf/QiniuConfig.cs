using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZY.Storage.QiniuConfig
{
    public class QiniuStorageConfig
    {
        private static ConfigInfo _info = null;
        public static ConfigInfo Info
        {
            get
            {
                if (_info == null)
                    _info = System.Configuration.ConfigurationManager.GetSection("qiniu") as ConfigInfo;
                return _info;
            }
            set { _info = value; }
        }
    }
}
