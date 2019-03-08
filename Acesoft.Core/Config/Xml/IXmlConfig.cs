using System;
using System.Collections.Generic;
using System.Xml;

namespace Acesoft.Config.Xml
{
    public interface IXmlConfig : IXmlConfigData
    {
        string ConfigFile { get; }

        void LoadConfig(string configFile);
    }
}
