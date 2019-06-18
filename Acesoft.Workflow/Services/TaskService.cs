using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Services
{
    public class TaskService : Service<WF_Task>, ITaskService
    {
        public WF_Task Get(long processId, int taskNo)
        {
            return Session.QueryFirst<WF_Task>(
                new RequestContext("wf", "get_task_by_taskno")
                .SetParam(new
                {
                    processId,
                    taskNo
                })
            );
        }

        public int Delete(long processId, IList<long> taskIds)
        {
            return Session.Execute(
                new RequestContext("wf", "delete_task_by_process")
                .SetParam(new
                {
                    processId,
                    taskIds
                })
            );
        }
    }
}