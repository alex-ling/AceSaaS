using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public interface IInstanceService : IService<WF_Instance>
    {
        WfResult GetInstance(WfRunner runner);
    }
}