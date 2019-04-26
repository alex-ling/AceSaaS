using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IUserService : IService<Rbac_User>
    {
        /// <summary>
        /// Query user with the Id(long:18)
        /// </summary>
        Rbac_User QueryById(long id, string authId);
        /// <summary>
        /// Query user with the UserName(perhaps as LoginName, Mobile, Mail)
        /// </summary>
        Rbac_User QueryByUserName(string userName, string authId = "none");
        /// <summary>
        /// Query user with the 3th AuthId, such as Wechat's OpenId
        /// </summary>
        Rbac_User QueryByAuth(long appId, string authId);

        /// <summary>
        /// Get user with the LoginName
        /// </summary>
        Rbac_User GetByLoginName(string loginName);
        /// <summary>
        /// Get user with the Mobile
        /// </summary>
        Rbac_User GetByMobile(string mobile);
        /// <summary>
        /// Get user with the Mail
        /// </summary>
        Rbac_User GetByMail(string mail);
        /// <summary>
        /// Check user with pwd, roles
        /// </summary>
        Rbac_User CheckUser(string userName, string password);
        /// <summary>
        /// Update user login information, such as IP, Time
        /// </summary>
        void UpdateLogin(Rbac_User user);
        /// <summary>
        /// Update user auth information, such as Login with WeChat
        /// </summary>
        void UpdateAuth(Rbac_User user, long appId, string authId, string authType);

        /// <summary>
        /// Delete user
        /// </summary>
        int Delete(long id);
        /// <summary>
        /// Delete multi-users
        /// </summary>
        void Delete(string userIds);
    }
}
