using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class PAService : Service<Rbac_PA>, IPAService
    {
        public IList<Rbac_PA> GetsByRoles(IList<long> roleIds)
        {
            return Session.Query<Rbac_PA>(
                new RequestContext("rbac", "get_pas_by_roles")
                .SetParam(new
                {
                    roleIds
                })
            ).ToList();
        }

        public void Save(long roleId, string refIds)
        {
            Session.BeginTransaction();

            try
            {
                DeleteByRole(roleId);

                refIds.Split<string>().Each(item =>
                {
                    if (item.HasValue())
                    {
                        var pa = new Rbac_PA
                        {
                            Role_Id = roleId,
                            Ref_Id = item.Split('_')[0].ToObject<long>(),
                            OpValue = item.Split('_')[1].ToObject<int>()
                        };
                        pa.InitializeId();
                        Insert(pa);
                    }
                });

                Session.Commit();
            }
            catch (Exception ex)
            {
                Session.Rollback();

                throw new AceException(ex.GetMessage());
            }
        }

        public int DeleteByRole(long roleId)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_pa_by_role")
                .SetParam(new
                {
                    roleId
                })
            );
        }

        public int DeleteByRefId(long refId)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_pa_by_ref")
                .SetParam(new
                {
                    refId
                })
            );
        }
    }
}
