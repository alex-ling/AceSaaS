using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_media")]
    public class Sys_Media : EntityBase
	{
		public string Url { get; set; }
		public MediaType Type { get; set; }
        public DateTime DCreate { get; set; }
        public bool Sync { get; set; }
    }
}
