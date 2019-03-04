using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac.StateProviders
{
    public class UserStateProvider : IStateProvider
    {
        public string Name => "CurrentUser";

        public Func<HttpContext, object> Get()
        {
            return ctx =>
            {
                var userService = ctx.RequestServices.GetService<IUserService>();

                if (ctx.User.Identity.IsAuthenticated)
                {
                    var userId = ctx.User.Identity.Name.ToObject<long>();
                    return userService.QueryById(userId);
                }

                return userService.QueryByUserName("guest");
            };
        }
    }
}
