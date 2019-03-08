using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_object")]
    public class Rbac_Object : EntityBase
    {
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public ObjectType Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string OpNames { get; set; }
        public bool Visible { get; set; }
        public int OrderNo { get; set; }
        public DateTime DCreate { get; set; }
    }
}
