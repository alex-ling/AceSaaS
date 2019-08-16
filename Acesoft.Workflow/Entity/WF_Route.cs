using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_route")]
    public class WF_Route : EntityBase
    {
        public long Process_Id { get; set; }
        public int FromTask { get; set; }
        public int ToTask { get; set; }
        public string RuleSql { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}