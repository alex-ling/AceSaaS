using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Runtime
{
    public interface IRuntimeFetch
    {
        void Execute(WfRunner runner, WfResult result);
    }

    public class WfRuntimeFetch : WfRuntime, IRuntimeFetch
    {
        private readonly IInstanceTaskService iTaskService;

        public WfRuntimeFetch(
            IInstanceTaskService iTaskService)
        {
            this.iTaskService = iTaskService;
        }

        public void Execute(WfRunner runner, WfResult result)
        {
            var iTask = result.InstanceTask;
            iTask.Status = WfTaskStatus.Dealing;
            iTask.User_Id = runner.AC.User.Id;
            iTask.User_Name = runner.AC.User.UserName ?? runner.AC.User.NickName;
            iTask.DFetch = DateTime.Now;
            iTaskService.Update(iTask);
        }
    }
}
