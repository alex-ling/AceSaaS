using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class RoleService : Service<Rbac_Role>, IRoleService
    {
        private readonly IUAService uaService;
        private readonly IPAService paService;

        public RoleService(IUAService uaService, IPAService paService)
        {
            this.uaService = uaService;
            this.paService = paService;
        }

        public int Delete(long id)
        {
            return Session.Execute(new RequestContext("rbac", "role")
                .SetCmdType(CmdType.delete)
                .SetParam(new
                {
                    id,
                    ids = new long[] { id }
                })
            );
        }

        public void Delete(string roleIds)
        {
            try
            {
                Session.BeginTransaction();

                roleIds.Split<long>().Each(roleId =>
                {
                    uaService.DeleteByRole(roleId);
                    paService.DeleteByRole(roleId);
                    Delete(roleId);
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
