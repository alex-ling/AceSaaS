using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Rbac;

namespace Acesoft.Workflow
{
    public class WfRunner
    {
        public IAccessControl AC { get; }

        public WfRunner(IAccessControl ac)
        {
            this.AC = ac;
        }

        public long TaskId { get; set; }
        public long AppInstanceId { get; set; }
        public WfAuditState Audit { get; set; }
        public string Opinion { get; set; }

        public bool LoadNextTasks { get; set; }
        public bool LoadBackInstanceTask { get; set; }
        public bool LoadPrevInstanceTask { get; set; }
        public bool LoadCanWithdraw { get; set; }
    }
}
