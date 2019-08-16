using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acesoft.Util
{
    public static class XmlExtensions
    {
        public static T GetValue<T>(this XmlAttributeCollection attributes, string name)
        {
            if (attributes[name] != null && attributes[name].Value.HasValue())
            {
                return attributes[name].Value.ToObject<T>();
            }

            throw new AceException($"不包含名为{name}的属性！");
        }

        public static T GetValue<T>(this XmlAttributeCollection attributes, string name, T defaultValue)
        {
            if (attributes[name] != null && attributes[name].Value.HasValue())
            {
                return attributes[name].Value.ToObject<T>();
            }

            return defaultValue;
        }
    }
}
