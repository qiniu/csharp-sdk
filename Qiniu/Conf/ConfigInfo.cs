using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZY.Storage.QiniuConfig
{
    public class ConfigInfo : IConfigurationSectionHandler
    {
        public string USER_AGENT { get; set; }
        public string ACCESS_KEY { get; set; }
        public string SECRET_KEY { get; set; }
        public string RS_HOST { get; set; }
        public string UP_HOST { get; set; }
        public string RSF_HOST { get; set; }
        public string PREFETCH_HOST { get; set; }
        public string DN_HOST { get; set; }
        public object Create(object parent, object configContext, XmlNode section)
        {

            this.USER_AGENT = section.SelectSingleNode("/qiniu/useragent").InnerText;

            this.ACCESS_KEY = section.SelectSingleNode("/qiniu/accesskey").InnerText;

            this.SECRET_KEY = section.SelectSingleNode("/qiniu/secretkey").InnerText;

            this.RS_HOST = section.SelectSingleNode("/qiniu/rshost").InnerText;

            this.UP_HOST = section.SelectSingleNode("/qiniu/uphost").InnerText;

            this.RSF_HOST = section.SelectSingleNode("/qiniu/rsfhost").InnerText;

            this.PREFETCH_HOST = section.SelectSingleNode("/qiniu/prefetchhost").InnerText;

            this.DN_HOST = section.SelectSingleNode("/qiniu/dnhost").InnerText;

            return this;
        }

    }
}
