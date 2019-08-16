using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_process")]
    public class WF_Process : EntityBase
    {
        public string WfNo { get; set; }
        public string Name { get; set; }
        public string Xml { get; set; }

        public int OrderNo { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}