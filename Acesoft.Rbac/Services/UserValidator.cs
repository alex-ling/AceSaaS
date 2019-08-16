using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Acesoft.Rbac.Services
{
    public class UserValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService userService;

        public UserValidator(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = userService.CheckUser(context.UserName, context.Password);
                context.Result = new GrantValidationResult(user.HashId, "custom");
                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.GetMessage());
            }
        }
    }
}
