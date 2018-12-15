using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acesoft.NetCore.Config
{
    public interface IXmlConfigData
    {
        XmlElement Config { get; }

        void Load(XmlElement config);
    }

    public abstract class XmlConfigData : IXmlConfigData
    {
        public XmlElement Config { get; private set; }

        public virtual void Load(XmlElement config)
        {
            this.Config = config;
        }
    }
}
