using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public class WfResult
    {
        public WF_Process Process { get; set; }
        public WF_Task Task { get; set; }
        public WF_Instance Instance { get; set; }
        public WF_InstanceTask InstanceTask { get; set; }
        public WF_InstanceTask PrevInstanceTask { get; set; }
        public WF_InstanceTask BackInstanceTask { get; set; }
        public IList<WF_Task> NextTasks { get; set; }

        public string BackTaskName => BackInstanceTask?.Task_Name;
        public string NextTasksName => NextTasks?.Join(t => t.Name);
        public bool CanWithdraw { get; set; }
        public bool IsStartTask => Task.Id == Instance.StartTask_Id;

        public string Message { get; set; }
    }
}
