using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public interface IInstanceTaskService : IService<WF_InstanceTask>
    {
        bool CanWithdraw(WfResult result);
        WF_InstanceTask GetBack(WfResult result);

        int Delete(long prevId);
    }
}