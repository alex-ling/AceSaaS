using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Services
{
    public class InstanceService : Service<WF_Instance>, IInstanceService
    {
        public WfResult GetInstance(WfRunner runner)
        {
            return Session.QueryMultiple<WfResult>(
                new RequestContext("wf", "get_instance_by_id")
                .SetParam(new
                {
                    runner.AppInstanceId,
                    runner.TaskId
                }),
                reader =>
                {
                    var result = new WfResult();
                    result.Process = reader.Read<WF_Process>(true).FirstOrDefault();
                    result.Task = reader.Read<WF_Task>(true).FirstOrDefault();
                    result.Instance = reader.Read<WF_Instance>(true).FirstOrDefault();
                    result.InstanceTask = reader.Read<WF_InstanceTask>(true).FirstOrDefault();
                    return result;
                }
            );
        }
    }
}
