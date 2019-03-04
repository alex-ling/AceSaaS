using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_pa")]
    public class Rbac_PA : EntityBase
    {
        public long Role_Id { get; set; }
        public long Ref_Id { get; set; }
        public int OpValue { get; set; }
    }
}
