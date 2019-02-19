using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Rbac;

namespace Acesoft.Web.StateProviders
{
    public class UserStateProvider : IApplicationStateProvider
    {
        public string Name => "CurrentUser";

        private IUser currentUser;
        public Func<IApplicationContext, T> Get<T>()
        {
            return appCtx =>
            {
                if (currentUser != null)
                {
                    return (T)currentUser;
                }

                var ctx = appCtx.HttpContext;
                if (ctx != null && ctx.User.Identity.IsAuthenticated 
                    && ctx.User.Identity.Name.HasValue())
                {
                    var userService = ctx.RequestServices.GetService<IUserService>();
                    currentUser = userService.Get(ctx.User.Identity.Name);
                    return (T)currentUser;
                }

                return default(T);
            };
        }
    }
}
