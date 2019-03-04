using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_auth")]
    public class Rbac_Auth : EntityBase
    {
        public long User_Id { get; set; }
        public string AuthType { get; set; }
        public string AuthId { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
