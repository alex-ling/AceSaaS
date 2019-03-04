using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_ua")]
    public class Rbac_UA : EntityBase
    {
        public long Role_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime? DStart { get; set; }
        public DateTime? DEnd { get; set; }
        public DateTime DCreate { get; set; }
    }
}
