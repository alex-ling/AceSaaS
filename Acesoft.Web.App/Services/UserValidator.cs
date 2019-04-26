using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer4.Validation;
using Acesoft.Rbac;
using IdentityServer4.Models;

namespace Acesoft.Web.App.Services
{
    public class UserValidator : IResourceOwnerPasswordValidator
    {
        private readonly IApplicationContext appCtx;

        public UserValidator(IApplicationContext appCtx)
        {
            this.appCtx = appCtx;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = appCtx.AC.CheckUser(context.UserName, context.Password);
                context.Result = new GrantValidationResult(user.Id.ToString(), "custom");
                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.GetMessage());
            }
        }
    }
}
