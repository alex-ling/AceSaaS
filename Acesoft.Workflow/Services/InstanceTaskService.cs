using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Services
{
    public class InstanceTaskService : Service<WF_InstanceTask>, IInstanceTaskService
    {
        public WF_InstanceTask GetBack(WfResult result)
        {
            if (result.InstanceTask.Action == WfActionType.Forward)
            {
                if (result.PrevInstanceTask == null)
                {
                    result.PrevInstanceTask = Get(result.InstanceTask.Prev_Id.Value);
                }
                return result.PrevInstanceTask;
            }

            return Session.QueryFirst<WF_InstanceTask>(
                new RequestContext("wf", "get_instancetask_by_taskid")
                .SetParam(new
                {
                    taskid = result.InstanceTask.Task_Id
                })
            );
        }

        public bool CanWithdraw(WfResult result)
        {
            return Session.ExecuteScalar<int>(
                new RequestContext("wf", "exe_instancetask_count")
                .SetParam(new
                {
                    prevId = result.InstanceTask.Id
                })
            ) <= 0;
        }

        public int Delete(long prevId)
        {
            return Session.Execute(
                new RequestContext("wf", "delete_instancetask_by_prevId")
                .SetParam(new
                {
                    prevId
                })
            );
        }
    }
}
