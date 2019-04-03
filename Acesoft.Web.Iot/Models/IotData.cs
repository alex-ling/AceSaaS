using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Acesoft.Cache;
using Acesoft.Web.Cloud;

namespace Acesoft.Web.IoT.Models
{
    public class IotData
    {
        public bool Online { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastCollectTime { get; set; }
        public IDictionary<string, object> Values { get; private set; }
        public AqiResult Weather { get; set; }

        [JsonIgnore]
        public IotDevice Device { get; set; }

        public IotData()
        {
            this.Values = new Dictionary<string, object>();
        }

        public void Save()
        {
            App.Cache.Set($"iot_data_{Device.Mac}", this);
        }
    }
}
