using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_instance")]
    public class WF_Instance : EntityBase
    {
        public long? ParentId { get; set; }
        public long AppInstanceId { get; set; }
        public long Process_Id { get; set; }
        public string Process_Name { get; set; }
        public long StartTask_Id { get; set; }
        public long CreateUser_Id { get; set; }
        public string CreateUser_Name { get; set; }
        public long? UpdateUser_Id { get; set; }
        public WfProcessStatus Status { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}