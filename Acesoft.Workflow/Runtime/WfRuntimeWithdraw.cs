using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Runtime
{
    public interface IRuntimeWithdraw
    {
        void Execute(WfRunner runner, WfResult result);
    }

    public class WfRuntimeWithdraw : WfRuntime, IRuntimeWithdraw
    {
        private readonly IInstanceTaskService iTaskService;

        public WfRuntimeWithdraw(
            IInstanceTaskService iTaskService)
        {
            this.iTaskService = iTaskService;
        }

        public void Execute(WfRunner runner, WfResult result)
        {
            if (result.CanWithdraw)
            {
                // 删除后续实例任务
                iTaskService.Delete(result.InstanceTask.Id);

                // 修改本部为处理中
                var iTask = result.InstanceTask;
                iTask.Status = WfTaskStatus.Dealing;
                iTask.Opinion = null;
                iTask.DUpdate = null;
                iTask.Audit = result.IsStartTask ? WfAuditState.UnSend : WfAuditState.Pending;
                iTaskService.Update(iTask);
            }
            else
            {
                throw new AceException("该件已回收或后置节点已处理！");
            }
        }
    }
}
