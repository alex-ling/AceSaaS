using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class ScaleService : Service<Rbac_Scale>, IScaleService
    {
        public Rbac_Scale GetByRef(string refId)
        {
            return Session.QuerySingle<Rbac_Scale>(
                new RequestContext("rbac", "get_scale_by_refid")
                .SetParam(new
                {
                    refId
                })
            );
        }

        public int Delete(long id)
        {
            return Session.Execute(
                new RequestContext("rbac", "scale")
                .SetCmdType(CmdType.delete)
                .SetParam(new
                {
                    id,
                    ids = new long[] { id }
                })
            );
        }

        public int DeleteByUser(long userId)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_scale_by_user")
                .SetParam(new
                {
                    userId
                })
            );
        }
    }
}
