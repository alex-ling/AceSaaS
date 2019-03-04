using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_seed")]
    public class Sys_Seed : EntityBase
	{
		public string Name { get; set; }
		public string Value { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime DUpdate { get; set; }
    }
}
