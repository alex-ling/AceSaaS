using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_scale")]
    public class Rbac_Scale : EntityBase
    {
        public long? ParentId { get; set; }
        public string Ref_Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool System { get; set; }
        public DateTime DCreate { get; set; }
    }
}
