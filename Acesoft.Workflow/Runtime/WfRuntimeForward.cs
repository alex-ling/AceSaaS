using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Runtime
{
    public interface IRuntimeForward
    {
        void Execute(WfRunner runner, WfResult result);
    }

    public class WfRuntimeForward : WfRuntime, IRuntimeForward
    {
        private readonly ITaskService taskService;
        private readonly IInstanceService instanceService;
        private readonly IInstanceTaskService iTaskService;

        public WfRuntimeForward(
            ITaskService taskService,
            IInstanceService instanceService,
            IInstanceTaskService iTaskService)
        {
            this.taskService = taskService;
            this.instanceService = instanceService;
            this.iTaskService = iTaskService;
        }

        public void Execute(WfRunner runner, WfResult result)
        {
            var task = result.Task;
            var iTask = result.InstanceTask;

            if (task.TaskType == WfTaskType.Default)
            {
                // 检查办理人是否一致
                if (iTask.User_Id != runner.AC.User.Id)
                {
                    throw new AceException("你无权处理该件，操作人不符！");
                }

                // 本步结束
                SetTaskFinish(runner, result);

                // 执行下一步
                var nextTasks = result.NextTasks;
                Check.Require(nextTasks.Count > 0, $"未获取到流程后续节点！");
                if (nextTasks.First().TaskType == WfTaskType.Stop)
                {
                    // 后续无业务时结束流程
                    SetInstanceFinish(runner, result);
                }
                else
                {                
                    // 生成后续的实例任务
                    nextTasks.Each(nextTask =>
                    {
                        CreateNextInstanceTask(runner, result, nextTask);
                    });
                }
            }
            else if (task.TaskType == WfTaskType.Looping)
            {
                // 选择下一人办理，重复当前节点
            }
            else if (task.TaskType == WfTaskType.Multiple)
            {
                // 按给定顺序执行会签，串行和并行
            }
            else if (task.TaskType == WfTaskType.SubProcess)
            {
                // 启动相应的子流程
            }
        }

        private void CreateNextInstanceTask(WfRunner runner, WfResult result, WF_Task nextTask)
        {
            var iTask = new WF_InstanceTask();
            iTask.InitializeId();
            iTask.DCreate = DateTime.Now;
            iTask.Prev_Id = result.InstanceTask.Id;
            iTask.AppInstanceId = runner.AppInstanceId;
            iTask.Process_Id = result.Process.Id;
            iTask.Process_Name = result.Process.Name;
            iTask.Task_Id = nextTask.Id;
            iTask.Task_Name = nextTask.Name;
            iTask.Status = WfTaskStatus.Pending;
            iTask.Audit = WfAuditState.Pending;
            iTask.Action = WfActionType.Forward;
            iTask.CreateUser_Id = runner.AC.User.Id;
            iTaskService.Insert(iTask);
        }

        private void SetInstanceFinish(WfRunner runner, WfResult result)
        {
            var instance = result.Instance;
            instance.DUpdate = DateTime.Now;
            instance.UpdateUser_Id = runner.AC.User.Id;
            instance.Status = WfProcessStatus.Stopped;
            instanceService.Update(instance);
        }

        private void SetTaskFinish(WfRunner runner, WfResult result)
        {
            var iTask = result.InstanceTask;
            iTask.DUpdate = DateTime.Now;
            iTask.UpdateUser_Id = runner.AC.User.Id;
            iTask.Status = WfTaskStatus.Processed;
            iTask.Audit = result.IsStartTask ? WfAuditState.Sended : runner.Audit;
            iTask.Opinion = runner.Opinion;
            iTaskService.Update(iTask);
        }
    }
}
