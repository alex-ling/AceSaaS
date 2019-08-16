using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.IotNet
{
    public class IotData
    {
        public bool Online { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastCollectTime { get; set; }
        public int Hour { get; set; }
        public bool Write { get; set; }
        public AqiResult Weather { get; set; }
        public IDictionary<string, object> Values { get; private set; }
    }
}
