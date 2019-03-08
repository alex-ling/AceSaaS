using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public static AuthenticationTicket AuthenticationTicket(
            string userHashId, bool isPersistent, string authenticationSchema = Auth_Cookie)
        {
            var identity = new ClaimsIdentity(authenticationSchema);

            // add user
            identity.AddClaim(new Claim(ClaimTypes.Sid, userHashId));
            identity.AddClaim(new Claim(ClaimTypes.Name, userHashId));
            //identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.Mobile));
            //identity.AddClaim(new Claim(ClaimTypes.Email, user.Mail));

            return new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = isPersistent }, 
                authenticationSchema);
        }
        #endregion

        #region params
        public static object GetParams()
        {
            var ac = App.Context.RequestServices.GetService<IAccessControl>();
            return new
            {
                userid = ac.User.Id,
                loginname = ac.User.LoginName,
                username = ac.User.UserName,
                refcode = ac.User.RefCode,
                nickname = ac.User.NickName,
                scaleid = ac.User.Scale_Id,
                inroles = ac.InRoles,
                roleids = ac.Roles
            };
        }
        #endregion
    }
}
