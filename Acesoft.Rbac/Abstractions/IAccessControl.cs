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
        HttpContext Context { get; }

        bool Logined { get; }
        bool IsRoot { get; }
        bool IsAdmin { get; }
        Rbac_User User { get; }
        IList<long> Roles { get; }
        string InRoles { get; }
        IDictionary<string, object> Params { get; }
        IDictionary<string, Rbac_Auth> Auths { get; }

        Task<Token> GetToken(string userName, string password);
        Task Login(string userName, string password, bool persistent);
        Task Login(Rbac_User user, bool persistent);
        void Logout();
        bool IsInRole(long roleId);
        bool CheckAccess(long refId);
        string Replace(string str, bool reapceQuery = true);

        long GetDefaultScaleId();
    }
}
