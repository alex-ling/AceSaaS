using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acesoft.Config.Xml
{
    public interface IXmlConfigData
    {
        XmlElement Config { get; }

        void Load(XmlElement config);
    }
}
