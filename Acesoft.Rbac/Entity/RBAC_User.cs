using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Rbac.Entity
{
    [Table("rbac_user")]
    public class Rbac_User : EntityBase
    {
        [NonSerialized]
        internal IList<Rbac_UA> Rbac_UAs;
        [NonSerialized]
        internal IList<Rbac_Param> Rbac_Params;
        [NonSerialized]
        internal IList<Rbac_Auth> Rbac_Auths;

        public long Scale_Id { get; set; }
        public long? Client_Id { get; set; }

        public string LoginName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Creator { get; set; }
        public string RefCode { get; set; }
        public UserType UserType { get; set; }
        public RegType RegType { get; set; }
        public bool Enabled { get; set; }

        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Mail { get; set; }
        public string Photo { get; set; }
        public int? Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }

        public DateTime? DLogin { get; set; }
        public string LoginIP { get; set; }

        public bool? RstPwd { get; set; }
        public DateTime? DRstPwd { get; set; }
        public int TryTimes { get; set; }

        public string Remark { get; set; }

        // ADD.BGN.2019-08-25 for wechat unionid.
        public string UnionId { get; set; }
    }
}
