using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Rbac
{
    public class Membership
    {
        public const string Auth_Cookie = "Cookie";
        public const string AUth_WeChat = "Wechat";
        public const string Auth_Bearer = "Bearer";

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
    }
}
