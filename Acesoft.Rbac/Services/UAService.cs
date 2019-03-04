using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class UAService : Service<Rbac_UA>, IUAService
    {
        public int DeleteByUser(long userId)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_ua_by_user")
                .SetParam(new
                {
                    userId
                })
            );
        }

        public int DeleteByRole(long roleId)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_ua_by_role")
                .SetParam(new
                {
                    roleId
                })
            );
        }

        public void Save(long userId, string roleIds)
        {
            Session.BeginTransaction();

            try
            {
                DeleteByUser(userId);

                roleIds.Split<long>().Each(roleId =>
                {
                    var ua = new Rbac_UA
                    {
                        User_Id = userId,
                        Role_Id = roleId,
                        DCreate = DateTime.Now
                    };
                    ua.InitializeId();
                    Insert(ua);
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
