using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.Cloud;
using Newtonsoft.Json;

namespace Acesoft.Web.IoT.Models
{
    public class IotDevice
    {
        public long Id { get; set; }
        public string Sbno { get; set; }
        public string Mac { get; set; }
        public string Alias { get; set; }
        public bool NetAlert { get; set; }
        public long? Owner_id { get; set; }
        public string Owner_phone { get; set; }
        public string Cpno { get; set; }
        public string Name { get; set; }
        public string Cpxh { get; set; }
        public Location Location { get; set; }

        [JsonIgnore]
        public IDictionary<string, IotParam> Params { get; set; }
        [JsonIgnore]
        public IDictionary<string, IotCmd> Cmds { get; set; }

        internal string GetInsertSql()
        {
            var sbIns = new StringBuilder().Append($"insert into iot_data_{Cpno}(id,mac,sbno,_temp,_hum,_pm25");
            var sbVal = new StringBuilder().Append("values(@id,@mac,@sbno,@_temp,@_hum,@_pm25");
            foreach (var param in Params)
            {
                sbIns.Append($",[{param.Key}]");
                sbVal.Append($",@{param.Key}");
            }
            sbIns.Append(")");
            sbVal.Append(")");
            return $"{sbIns}{sbVal}";
        }
    }
}
