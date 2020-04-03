using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IUserService : IService<Rbac_User>
    {
        Rbac_User QueryById(long id, string authId);
        Rbac_User QueryByUserName(string userName, string authId = "none");
        Rbac_User QueryByAuth(long appId, string authId);

        Rbac_User GetByLoginName(string loginName);
        Rbac_User GetByMobile(string mobile);
        Rbac_User GetByMail(string mail);
        Rbac_User GetByRefCode(string refcode);
        Rbac_User GetByAuth(long appId, string authId);

        Rbac_User CheckUser(string userName, string password);
        void UpdateLogin(Rbac_User user);
        void UpdateAuth(Rbac_User user, long appId, string authId, string authType);

        int Delete(long id);
        void Delete(string userIds);
    }
}
