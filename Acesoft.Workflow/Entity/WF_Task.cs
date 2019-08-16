using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_task")]
    public class WF_Task : EntityBase
    {
        public long Process_Id { get; set; }
        public string Name { get; set; }
        public int TaskNo { get; set; }
        public WfRouteType RouteIn { get; set; }
        public WfRouteType RouteOut { get; set; }
        public WfTaskType TaskType { get; set; }
        public bool AutoFetch { get; set; }
        public string Url { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
