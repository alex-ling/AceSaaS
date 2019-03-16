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
        IDictionary<string, string> Auths { get; }

        Task Login(string userName, string password, bool persistent);
        Task Login(long appId, string authId, bool persistent);
        //Task Login(Rbac_User user, bool persistent);
        Task<Token> GetToken(string userName, string passwrod);
        void UpdateAuth(long appId, string authId, string authType, bool needSaveUser);
        void Logout();
        bool IsInRole(long roleId);
        bool CheckAccess(long refId);
        string Replace(string str, bool reapceQuery = true);

        long GetDefaultScaleId();
    }
}
