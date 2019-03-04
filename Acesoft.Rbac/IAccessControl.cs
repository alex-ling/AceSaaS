using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IAccessControl
    {
        HttpContext HttpContext { get; }

        bool Logined { get; }
        bool IsRoot { get; }
        bool IsAdmin { get; }
        Rbac_User User { get; }
        IEnumerable<long> Roles { get; }
        string InRoles { get; }
        IDictionary<string, string> Params { get; }
        IDictionary<string, string> Auths { get; }

        Task Login(string userName, string password, bool persistent);
        Task Login(long appId, string authId, bool persistent);
        Task Login(Rbac_User user, bool persistent);
        void UpdateUser(long appId, string authId, string authType, bool needSaveUser);
        Task Logout();
        bool IsInRole(long roleId);
        bool CheckAccess(long refId);
    }
}
