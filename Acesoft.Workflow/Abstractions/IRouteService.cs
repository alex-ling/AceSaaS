using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow
{
    public interface IRouteService : IService<WF_Route>
    {
        IList<WF_Route> GetFromRoutes(long processId, int taskNo);
        IList<WF_Route> GetToRoutes(long processId, int taskNo);
        bool IsRoutePassed(WF_Route route, WfResult result);

        int Delete(long processId);
    }
}
