using System;

namespace Acesoft.Workflow
{
    public interface IWorkflowService
    {
        /// <summary>
        /// 流程启动
        /// </summary>
        WfResult Start(WfRunner runner);
        /// <summary>
        /// 流程取件
        /// </summary>
        WfResult Fetch(WfRunner runner);
        /// <summary>
        /// 流程前进
        /// </summary>
        WfResult Forward(WfRunner runner);
        /// <summary>
        /// 流程后退
        /// </summary>
        WfResult Backward(WfRunner runner);
        /// <summary>
        /// 流程回收
        /// </summary>
        WfResult Withdraw(WfRunner runner);
    }
}