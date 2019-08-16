using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Workflow.Entity;

namespace Acesoft.Workflow.Services
{
    public class RouteService : Service<WF_Route>, IRouteService
    {
        public IList<WF_Route> GetFromRoutes(long processId, int taskNo)
        {
            return Session.Query<WF_Route>(
                new RequestContext("wf", "gets_route_by_fromtask")
                .SetParam(new
                {
                    processId,
                    taskNo
                })
            ).ToList();
        }

        public bool IsRoutePassed(WF_Route route, WfResult result)
        {
            if (route.RuleSql.HasValue())
            {
                return Session.ExecuteScalar<int>(route.RuleSql, new
                {
                    result.Instance.AppInstanceId
                }) > 0;
            }
            return true;
        }

        public IList<WF_Route> GetToRoutes(long processId, int taskNo)
        {
            return Session.Query<WF_Route>(
                new RequestContext("wf", "gets_route_by_totask")
                .SetParam(new
                {
                    processId,
                    taskNo
                })
            ).ToList();
        }

        public int Delete(long processId)
        {
            return Session.Execute(
                new RequestContext("wf", "delete_route_by_process")
                .SetParam(new
                {
                    processId
                })
            );
        }
    }
}