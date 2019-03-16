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
        private readonly IPAService paService;

        public ObjectService(IPAService pAService)
        {
            this.paService = pAService;
        }

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

        public IList<Rbac_Object> Gets(IList<long> roleIds, ObjectType type, string loginname)
        {
            return Session.Query<Rbac_Object>(
                new RequestContext("rbac", "get_objects_by_roles")
                .SetParam(new
                {
                    type,
                    loginname,
                    roleIds
                })
            ).ToList();
        }

        public int Delete(long id)
        {
            return Session.Execute(
                new RequestContext("rbac", "object")
                .SetCmdType(CmdType.delete)
                .SetParam(new
                {
                    id,
                    ids = new long[] { id }
                })
            );
        }

        public void Delete(string objectIds)
        {
            try
            {
                Session.BeginTransaction();

                objectIds.Split<long>().Each(objectId =>
                {
                    // delete ua
                    paService.DeleteByRefId(objectId);

                    // delete user
                    Delete(objectId);
                });

                Session.Commit();
            }
            catch (Exception ex)
            {
                Session.Rollback();

                throw new AceException(ex.GetMessage());
            }
        }
    }
}
