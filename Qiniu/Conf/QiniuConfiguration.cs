using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ZY.Storage.QiniuConfig
{
    public class QiniuConfiguration
    {
        internal static ConfigInfo FromAppConfig(XmlNode node)
        {
            XmlTextReader reader = new XmlTextReader(node.OuterXml, XmlNodeType.Document, null);
            return (ConfigInfo)(new System.Xml.Serialization.XmlSerializer(typeof(ConfigInfo)).Deserialize(reader));
        }
    }
}
