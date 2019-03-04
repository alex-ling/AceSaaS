using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Authorization;

namespace Acesoft.Rbac
{
    public class MultiAuthorizeAttribute : AuthorizeAttribute
    {
        public MultiAuthorizeAttribute()
        {
            base.AuthenticationSchemes = $"{Membership.Auth_Cookie},Bearer";
        }
    }
}
