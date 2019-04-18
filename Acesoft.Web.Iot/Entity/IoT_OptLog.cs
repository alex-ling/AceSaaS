using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.IoT.Entity
{
    [Table("iot_optlog")]
    public class IoT_OptLog : EntityBase
	{
        public long User_Id { get; set; }
        public string Mac { get; set; }
        public string Sbno { get; set; }
        public string Name { get; set; }
        public string Cmd { get; set; }
        public string Body { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
