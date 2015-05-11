using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZY.Storage.QiniuConfig
{
    public class ConfigurationSectionHandler : System.Configuration.IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        object System.Configuration.IConfigurationSectionHandler.Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return QiniuConfiguration.FromAppConfig(section);
        }

        #endregion
    }
}
