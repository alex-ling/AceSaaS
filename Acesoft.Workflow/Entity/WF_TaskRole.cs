using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_taskrole")]
    public class WF_TaskRole : EntityBase
    {
        public long Role_Id { get; set; }
        public long Task_Id { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
