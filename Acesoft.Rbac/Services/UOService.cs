using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.Services
{
    public class UOService : Service<Rbac_UO>, IUOService
    {
        public int DeleteByUser(long userId, string type)
        {
            return Session.Execute(
                new RequestContext("rbac", "delete_uo_by_user")
                .SetParam(new
                {
                    userId,
                    type
                })
            );
        }

        public void Save(long userId, string type, string refIds)
        {
            Session.BeginTransaction();

            try
            {
                DeleteByUser(userId, type);

                refIds.Split<long>().Each(refId =>
                {
                    var ua = new Rbac_UO
                    {
                        User_Id = userId,
                        Ref_Id = refId,
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
