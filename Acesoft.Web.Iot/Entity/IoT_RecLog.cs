using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.IoT.Entity
{
    [Table("iot_reclog")]
    public class IoT_RecLog : EntityBase
	{
        public string Mac { get; set; }
        public string Sbno { get; set; }
        public string Name { get; set; }
        public string Cmd { get; set; }
        public string Body { get; set; }
        public string Json { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
