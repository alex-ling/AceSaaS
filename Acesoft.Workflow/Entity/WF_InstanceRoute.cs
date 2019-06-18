using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Workflow.Entity
{
    [Table("wf_instanceroute")]
    public class WF_InstanceRoute : EntityBase
    {
        public Guid ProcessGUID { get; set; }
        public Guid FromTaskInstanceId { get; set; }
        public Guid ToTaskInstanceId { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long CreatedUserId { get; set; }
        public long UpdatedUserId { get; set; }
    }
}