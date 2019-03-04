using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class ObjectService : Service<Rbac_Object>, IObjectService
    {
        public Rbac_Object GetByUrl(string url)
        {
            return Session.QuerySingle<Rbac_Object>(
                new RequestContext("rbac", "get_object_by_url")
                .SetParam(new
                {
                    url
                })
            );
        }

        public IList<Rbac_Object> Gets(IList<long> roleIds, int type, string user)
        {
            return Session.Query<Rbac_Object>(
                new RequestContext("rbac", "get_objects_by_roles")
                .SetParam(new
                {
                    type,
                    user,
                    roleIds
                })
            ).ToList();
        }

        public int Delete(long id)
        {
            return Session.Execute(
                new RequestContext("rbac", "object")
                .SetParam(new
                {
                    id,
                    ids = new long[] { id }
                })
            );
        }

        public int Delete(string ids)
        {
            return Session.Execute(
                new RequestContext("rbac", "object")
                .SetParam(new
                {
                    ids = ids.Split<long>()
                })
            );
        }
    }
}
