using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Portal.Entity
{
    [Table("port_widget")]
    public class Port_Widget : EntityBase
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public string Remark { get; set; }
    }
}
