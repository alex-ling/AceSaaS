using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_schema")]
	public class Sys_Schema : EntityBase
	{
		public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public bool System { get; set; }
        public DateTime DCreate { get; set; }
    }
}
