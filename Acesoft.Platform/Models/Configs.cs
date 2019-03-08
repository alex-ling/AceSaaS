using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Platform.Models
{
    public class Configs
    {
        private readonly IDictionary<string, ConfigItem> values;

        public Configs(IDictionary<string, ConfigItem> values)
        {
            this.values = values;
        }

        public string this[string key]
        {
            get
            {
                return GetValue(key, "");
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            var item = values.GetValue(key, null);
            if (item != null)
            {
                return item.Value.ToObject(defaultValue);
            }
            return defaultValue;
        }
    }
}
