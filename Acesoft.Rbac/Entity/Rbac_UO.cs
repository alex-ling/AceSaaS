using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_uo")]
    public class Rbac_UO : EntityBase
    {
        public long User_Id { get; set; }
        public long Ref_Id { get; set; }
        public string Type { get; set; }
        public DateTime DCreate { get; set; }
    }
}