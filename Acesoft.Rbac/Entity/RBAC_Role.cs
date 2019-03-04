using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_role")]
    public class Rbac_Role : EntityBase
    {
        public long Scale_Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool System { get; set; }
        public DateTime DCreate { get; set; }
    }
}
