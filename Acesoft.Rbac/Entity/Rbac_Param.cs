using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_param")]
    public class Rbac_Param : EntityBase
    {
        public long User_Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime DCreate { get; set; }
    }
}
