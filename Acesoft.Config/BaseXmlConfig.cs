using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acesoft.NetCore.Config
{
    public abstract class BaseXmlConfig : XmlConfigData, IXmlConfig
    {
        public string ConfigFile { get; private set; }

        public void LoadConfig(string configFile)
        {
            this.ConfigFile = configFile;

            var doc = new XmlDocument();
            doc.Load(configFile);
            this.Load(doc.DocumentElement);
        }
    }
}
