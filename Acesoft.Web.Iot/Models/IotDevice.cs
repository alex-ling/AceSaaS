using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.Cloud;

namespace Acesoft.Web.IoT.Models
{
    public class IotDevice
    {
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

        public IDictionary<string, IotParam> Params { get; set; }
        public IDictionary<string, IotCmd> Cmds { get; set; }
    }
}
