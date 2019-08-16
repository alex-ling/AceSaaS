using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public interface IProcessService : IService<WF_Process>
    {
        void Save(long id, string xml);
    }
}
