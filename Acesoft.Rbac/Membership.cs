using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

using Acesoft.Rbac.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Acesoft.Rbac
{
    public class Membership
    {
        public const string Auth_Multis = "Cookie,Bearer,Wechat";
        public const string Auth_Cookie = "Cookie";
        public const string AUth_WeChat = "Wechat";
        public const string Auth_Bearer = "Bearer";
        public const string Auth_OpenID = "OpenID";
        public const long Default_ScaleId = 967167765444034560L;

        #region ticket
        public static AuthenticationTicket AuthenticationTicket(string id, string userName, string authId, bool isPersistent, string authenticationSchema = Auth_Cookie)
        {
            var identity = new ClaimsIdentity(authenticationSchema);

            // add user
            identity.AddClaim(new Claim("sub", id));
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("authid", authId));

            return new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = isPersistent }, 
                authenticationSchema);
        }
        #endregion
    }
}
