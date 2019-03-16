using System;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Acesoft.Util;
using Acesoft.Logger;

namespace Acesoft.Rbac.StateProviders
{
    public class UserStateProvider : IStateProvider
    {
        private readonly ILogger<UserStateProvider> logger = LoggerContext.GetLogger<UserStateProvider>();

        public string Name => "CurrentUser";

        public Func<HttpContext, object> Get()
        {
            return ctx =>
            {
                var userService = ctx.RequestServices.GetService<IUserService>();

                if (ctx.User.Identity.IsAuthenticated)
                {
                    var claim = ctx.User.FindFirst("sub");
                    if (claim != null)
                    { 
                        var userId = NaryHelper.ToNary(claim.Value, 36);
                        return userService.QueryById(userId);
                    }
                    else
                    {
                        logger.LogError($"Create user from \"{ctx.User.Identity.AuthenticationType}\" with error");
                        return null;
                    }
                }

                return userService.QueryByUserName("guest");
            };
        }
    }
}
