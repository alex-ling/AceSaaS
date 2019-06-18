using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public interface ITaskService : IService<WF_Task>
    {
        WF_Task Get(long processId, int taskNo);

        int Delete(long processId, IList<long> exceptIds);
    }
}
