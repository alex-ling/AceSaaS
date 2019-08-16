using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Rbac;
using Acesoft.Workflow.Runtime;

namespace Acesoft.Workflow
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IRuntimeStartup runtimeStartup;
        private readonly IRuntimeFetch runtimeFetch;
        private readonly IRuntimeForward runtimeForward;
        private readonly IRuntimeBackward runtimeBackward;
        private readonly IRuntimeWithdraw runtimeWithdraw;

        public WorkflowService(
            IRuntimeStartup runtimeStartup,
            IRuntimeFetch runtimeFetch,
            IRuntimeWithdraw runtimeWithdraw,
            IRuntimeForward runtimeForward,
            IRuntimeBackward runtimeBackward)
        {
            this.runtimeStartup = runtimeStartup;
            this.runtimeFetch = runtimeFetch;
            this.runtimeWithdraw = runtimeWithdraw;
            this.runtimeForward = runtimeForward;
            this.runtimeBackward = runtimeBackward;
        }

        public WfResult Start(WfRunner runner)
        {
            var result = runtimeStartup.Execute(runner);
            if (result.InstanceTask.Status == WfTaskStatus.Pending && result.Task.AutoFetch)
            {
                // 自动取件
                runtimeFetch.Execute(runner, result);
            }
            return result;
        }

        public WfResult Fetch(WfRunner runner)
        {
            var result = Start(runner);
            runtimeFetch.Execute(runner, result);
            return result;
        }

        public WfResult Forward(WfRunner runner)
        {
            var result = Start(runner);
            var iTask = result.InstanceTask;
            var task = result.Task;

            if (iTask.Status == WfTaskStatus.Dealing)
            {
                runtimeForward.Execute(runner, result);
            }
            else if (iTask.Status == WfTaskStatus.Pending)
            {
                throw new AceException("流程当前节点未取件！");
            }
            else
            {
                throw new AceException("流程当前节点已处理！");
            }

            return result;
        }

        public WfResult Backward(WfRunner runner)
        {
            var result = Start(runner);
            runtimeBackward.Execute(runner, result);
            return result;
        }

        public WfResult Withdraw(WfRunner runner)
        {
            var result = Start(runner);
            runtimeWithdraw.Execute(runner, result);
            return result;
        }
    }
}
