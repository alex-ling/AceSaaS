using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_instancetask")]
    public class WF_InstanceTask : EntityBase
    {
        public long? Prev_Id { get; set; }
        public long AppInstanceId { get; set; }
        public long Process_Id { get; set; }
        public string Process_Name { get; set; }
        public long Task_Id { get; set; }
        public string Task_Name { get; set; }
        public long CreateUser_Id { get; set; }
        public long? UpdateUser_Id { get; set; }
        public long User_Id { get; set; }
        public string User_Name { get; set; }
        public WfTaskStatus Status { get; set; }
        public WfAuditState Audit { get; set; }
        public WfActionType Action { get; set; }
        //public bool Current { get; set; }
        public string Opinion { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DFetch { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
