using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Portal.Entity
{
    [Table("port_page")]
    public class Port_Page : EntityBase
    {
        public long ParentId { get; set; }

        public string Name { get; set; }
        public string Layout { get; set; }
        public string Url { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public string Remark { get; set; }
        public int OrderNo { get; set; }
    }
}
