using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

using Acesoft.Data;
using Acesoft.Workflow.Entity;
using Acesoft.Util;

namespace Acesoft.Workflow.Services
{
    public class ProcessService : Service<WF_Process>, IProcessService
    {
        private readonly ITaskService taskService;
        private readonly IRouteService routeService;

        public ProcessService(ITaskService taskService,
            IRouteService routeService)
        {
            this.taskService = taskService;
            this.routeService = routeService;
        }

        public void Save(long id, string xml)
        {
            var xmlDoc = new XmlDocument();
            xml = HttpUtility.UrlDecode(xml);
            xmlDoc.LoadXml(xml);

            // 定义根元素
            var root = xmlDoc.DocumentElement;

            Session.BeginTransaction();
            try
            {
                // 获得当前流程并保存流程图
                var process = Get(id);
                process.Xml = xml;
                Update(process);

                // 清除该流程之前配置的跳帧
                routeService.Delete(process.Id);

                // 保存工作流的节点（活动
                var taskIds = new List<long>();
                var tasks = root.SelectNodes("//Task");
                foreach (XmlNode node in tasks)
                {
                    var taskNo = int.Parse(node.Attributes["id"].Value);
                    var task = taskService.Get(process.Id, taskNo);
                    var isNew = false;
                    if (task == null)
                    {
                        isNew = true;
                        task = new WF_Task();
                        task.InitializeId();
                        task.DCreate = DateTime.Now;
                        task.Process_Id = process.Id;
                        task.TaskNo = taskNo;
                    }

                    task.Name = node.Attributes.GetValue("label", "");
                    task.Url = node.Attributes.GetValue("url", "");
                    task.TaskType = node.Attributes.GetValue("type", WfTaskType.Default);
                    task.RouteIn = node.Attributes.GetValue("in", WfRouteType.Default);
                    task.RouteOut = node.Attributes.GetValue("out", WfRouteType.Default);
                    task.AutoFetch = node.Attributes.GetValue("fetch", 1) > 0;

                    // 保存
                    if (isNew)
                    {
                        taskService.Insert(task);
                    }
                    else
                    {
                        taskService.Update(task);
                    }
                    taskIds.Add(task.Id);

                    // 保存工作流单元权限配置
                    //foreach (XmlElement roleNode in node.SelectNodes("//role"))
                    //{
                    //    var taskRole = new WF_TaskRole();
                    //    taskRole.InitializeId();
                    //    taskRole.Task_Id = task.Id;
                    //    taskRole.Role_Id = roleNode.Attributes.GetValue<long>("role");
                    //    taskRoleService.Insert(taskRole);
                    //}
                }

                // 保存工作流的步骤
                var lines = root.SelectNodes("//Line");
                foreach (XmlNode node in lines)
                {
                    // 创建
                    var route = new WF_Route();
                    route.InitializeId();
                    route.DCreate = DateTime.Now;
                    route.Process_Id = process.Id;
                    route.FromTask = node.FirstChild.Attributes.GetValue<int>("source");
                    route.ToTask = node.FirstChild.Attributes.GetValue<int>("target");
                    route.RuleSql = node.Attributes.GetValue("rule", "");
                    routeService.Insert(route);
                }

                // 清除要删除的节点
                taskService.Delete(process.Id, taskIds);

                Session.Commit();
            }
            catch
            {
                Session.Rollback();
                throw;
            }
        }
    }
}
