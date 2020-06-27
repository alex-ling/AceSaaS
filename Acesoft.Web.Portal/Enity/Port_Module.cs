using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Portal.Entity
{
    [Table("port_module")]
    public class Port_Module : EntityBase
    {
        public long Page_Id { get; set; }
        public long Widget_Id { get; set; }

        public string Title { get; set; }
        public string Icon { get; set; }
        public string Template { get; set; }
        public string DockName { get; set; }
        public int DockOrder { get; set; }
        public int Cache { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public string Remark { get; set; }

        [NonSerialized]
        public IDictionary<string, object> Configs;
        [NonSerialized]
        public Port_Widget Widget;

        public IDictionary<string, object> Props()
        {
            return Configs.Merge(new Dictionary<string, object>
            {
                { "mod_title", Title },
                { "mod_icon", Icon },
                { "mod_template", Template },
                { "mod_cache", Cache },
                { "mod_remark", Remark }
            });
        }
    }
}
