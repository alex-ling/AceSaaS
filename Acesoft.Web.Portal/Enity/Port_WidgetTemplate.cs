using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Portal.Entity
{
    [Table("port_widgettemplate")]
    public class Port_WidgetTemplate : EntityBase
    {
        public long Widget_id { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Remark { get; set; }
    }
}
