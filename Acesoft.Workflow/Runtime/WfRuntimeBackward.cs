using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Runtime
{
    public interface IRuntimeBackward
    {
        void Execute(WfRunner runner, WfResult result);
    }

    public class WfRuntimeBackward : WfRuntime, IRuntimeBackward
    {
        private readonly ITaskService taskService;
        private readonly IRouteService routeService;
        private readonly IInstanceService instanceService;
        private readonly IInstanceTaskService iTaskService;

        public WfRuntimeBackward(
            ITaskService taskService,
            IRouteService routeService,
            IInstanceService instanceService,
            IInstanceTaskService iTaskService)
        {
            this.taskService = taskService;
            this.routeService = routeService;
            this.instanceService = instanceService;
            this.iTaskService = iTaskService;
        }

        public void Execute(WfRunner runner, WfResult result)
        {
            var task = result.Task;
            var iTask = result.InstanceTask;

            // 检查办理人是否一致
            if (iTask.User_Id != runner.AC.User.Id)
            {
                throw new AceException("你无权处理该件，操作人不符！");
            }

            // 本步结束
            SetTaskFinish(runner, result);

            // 返回上一步
            var backTask = taskService.Get(result.BackInstanceTask.Task_Id);
            CreateNextInstanceTask(runner, result, backTask);
        }

        private void CreateNextInstanceTask(WfRunner runner, WfResult result, WF_Task backTask)
        {
            var iTask = new WF_InstanceTask();
            iTask.InitializeId();
            iTask.DCreate = DateTime.Now;
            iTask.Prev_Id = result.InstanceTask.Id;
            iTask.AppInstanceId = runner.AppInstanceId;
            iTask.Process_Id = result.Process.Id;
            iTask.Process_Name = result.Process.Name;
            iTask.Task_Id = backTask.Id;
            iTask.Task_Name = backTask.Name;
            iTask.Status = WfTaskStatus.Dealing;
            iTask.DFetch = DateTime.Now;
            iTask.Audit = result.Instance.StartTask_Id == backTask.Id ? WfAuditState.UnSend : WfAuditState.Pending;
            iTask.Action = WfActionType.Backward;
            iTask.CreateUser_Id = runner.AC.User.Id;
            iTask.User_Id = result.BackInstanceTask.User_Id;
            iTask.User_Name = result.BackInstanceTask.User_Name;
            iTaskService.Insert(iTask);
        }

        private void SetTaskFinish(WfRunner runner, WfResult result)
        {
            var iTask = result.InstanceTask;
            iTask.DUpdate = DateTime.Now;
            iTask.UpdateUser_Id = runner.AC.User.Id;
            iTask.Status = WfTaskStatus.Processed;
            iTask.Audit = WfAuditState.Back;
            iTask.Opinion = runner.Opinion;
            iTaskService.Update(iTask);
        }
    }
}
