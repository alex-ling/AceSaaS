using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_field")]
    public class Sys_Field : EntityBase
	{
		public long Table_Id { get; set; }
		public string Field { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public FieldType Type { get; set; }
        public int? Length { get; set; }
        public bool IsNull { get; set; }
        public bool Unique { get; set; }
        public string Default { get; set; }
        public string Ref { get; set; }
        public int OrderNo { get; set; }
        public bool Created { get; set; }
        public bool System { get; set; }
        public DateTime DCreate { get; set; }
    }
}
