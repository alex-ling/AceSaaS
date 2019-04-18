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
        public int Hour { get; set; }
        public bool Write { get; set; }
        public IotDevice Device { get; set; }
        public AqiResult Weather { get; set; }
        public IDictionary<string, object> Values { get; private set; }

        public IotData()
        {
            this.Values = new Dictionary<string, object>();
        }

        public bool IsNeedSave()
        {
            int hour = DateTime.Now.Hour;
            if (hour <= Hour)
            {
                if (hour == 0)
                {
                    return Hour == 23;
                }
                return false;
            }
            return true;
        }

        public IDictionary<string, object> GetSaveParams()
        {
            Hour = DateTime.Now.Hour;
            var param = new Dictionary<string, object>
            {
                {
                    "id",
                    App.IdWorker.NextId()
                },
                {
                    "mac",
                    Device.Mac
                },
                {
                    "sbno",
                    Device.Sbno
                },
                {
                    "_temp",
                    Weather?.condition.temp
                },
                {
                    "_hum",
                    Weather?.condition.humidity
                },
                {
                    "_pm25",
                    Weather?.aqi.pm25
                }
            };

            foreach (var value in Values)
            {
                string text;
                if ((text = (value.Value as string)) != null)
                {
                    param.Add(value.Key, (text == "异常") ? null : text);
                }
                else
                {
                    param.Add(value.Key, value.Value);
                }
            }
            return param;
        }

        public void Save()
        {
            App.Cache.Set($"iot_data_{Device.Mac}", new
            {
                Online,
                LastLoginTime,
                LastCollectTime,
                Hour,
                Write,
                Weather,
                Values
            });
        }
    }
}
