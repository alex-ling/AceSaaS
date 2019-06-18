using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Runtime
{
    public interface IRuntimeStartup
    {
        WfResult Execute(WfRunner runner);
    }

    public class WfRuntimeStartup : WfRuntime, IRuntimeStartup
    {
        private readonly ITaskService taskService;
        private readonly IRouteService routeService;
        private readonly IInstanceService instanceService;
        private readonly IInstanceTaskService iTaskService;

        public WfRuntimeStartup(
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

        public WfResult Execute(WfRunner runner)
        { 
            var result = instanceService.GetInstance(runner);
            if (result.Instance == null)
            {
                // create instance
                var instance = new WF_Instance();
                instance.InitializeId();
                instance.DCreate = DateTime.Now;
                instance.AppInstanceId = runner.AppInstanceId;
                instance.CreateUser_Id = runner.AC.User.Id;
                instance.CreateUser_Name = runner.AC.User.UserName ?? runner.AC.User.NickName;
                instance.Process_Id = result.Process.Id;
                instance.Process_Name = result.Process.Name;
                instance.StartTask_Id = result.Task.Id;
                instance.Status = WfProcessStatus.Running;
                instanceService.Insert(instance);

                var iTask = new WF_InstanceTask();
                iTask.InitializeId();
                iTask.DCreate = DateTime.Now;
                iTask.AppInstanceId = runner.AppInstanceId;
                iTask.Process_Id = result.Process.Id;
                iTask.Process_Name = result.Process.Name;
                iTask.Task_Id = result.Task.Id;
                iTask.Task_Name = result.Task.Name;
                iTask.DFetch = DateTime.Now;
                iTask.Status = WfTaskStatus.Dealing;
                iTask.Audit = WfAuditState.UnSend;
                iTask.Action = WfActionType.Begin;
                iTask.CreateUser_Id = runner.AC.User.Id;
                iTask.User_Id = instance.CreateUser_Id;
                iTask.User_Name = instance.CreateUser_Name;
                iTaskService.Insert(iTask);

                result.Instance = instance;
                result.InstanceTask = iTask;
            }

            if (runner.LoadPrevInstanceTask && result.InstanceTask.Prev_Id.HasValue)
            {
                result.PrevInstanceTask = iTaskService.Get(result.InstanceTask.Prev_Id.Value);
            }

            if (runner.LoadBackInstanceTask)
            {
                result.BackInstanceTask = GetBackInstanceTask(result);
            }

            if (runner.LoadNextTasks)
            {
                result.NextTasks = GetNextTasks(result);
            }

            if (runner.LoadCanWithdraw)
            {
                if (result.InstanceTask.Status == WfTaskStatus.Processed)
                {
                    result.CanWithdraw = CanWithdraw(runner, result);
                }
                else
                {
                    result.CanWithdraw = false;
                }
            }

            return result;
        }

        private bool CanWithdraw(WfRunner runner, WfResult result)
        {
            if (result.InstanceTask.User_Id != runner.AC.User.Id)
            {
                result.Message = "你无权处理该件，操作人不符！";
                return false;
            }
            else if (result.Instance.Status == WfProcessStatus.Stopped)
            {
                result.Message = "流程已结束";
                return false;
            }
            return iTaskService.CanWithdraw(result);
        }

        private WF_InstanceTask GetBackInstanceTask(WfResult result)
        {
            return iTaskService.GetBack(result);
        }

        private IList<WF_Task> GetNextTasks(WfResult result)
        {
            var tasks = new List<WF_Task>();
            var task = result.Task;
            var routes = routeService.GetFromRoutes(task.Process_Id, task.TaskNo);

            if (routes.Count > 1)
            {
                routes.Each(route =>
                {
                    if (task.RouteOut != WfRouteType.And)
                    {
                        if (routeService.IsRoutePassed(route, result))
                        {
                            // add route passwd
                            tasks.Add(taskService.Get(task.Process_Id, route.ToTask));
                        }
                    }
                    else
                    {
                        // and route every
                        tasks.Add(taskService.Get(task.Process_Id, route.ToTask));
                    }
                });
            }
            else
            {
                // and route one
                tasks.Add(taskService.Get(task.Process_Id, routes.First().ToTask));
            }

            return tasks;
        }
    }
}
