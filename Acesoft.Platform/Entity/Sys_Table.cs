using System;
using System.Collections.Generic;
using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_table")]
	public class Sys_Table : EntityBase
    {
        [NonSerialized]
        public IList<Sys_Field> Fields;

        public long Schema_Id { get; set; }
		public string Table { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int OrderNo { get; set; }
        public bool Created { get; set; }
        public bool System { get; set; }
        public DateTime DCreate { get; set; }
    }
}
