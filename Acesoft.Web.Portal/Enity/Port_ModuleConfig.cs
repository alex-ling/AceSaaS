using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Portal.Entity
{
    [Table("port_moduleconfig")]
    public class Port_ModuleConfig : EntityBase
    {
        public long Module_Id { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
