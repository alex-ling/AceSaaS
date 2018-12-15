using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Acesoft.Web.Multitenancy
{
    public class TenantResolutionMiddleware<T>
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILogger<TenantResolutionMiddleware<T>> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, ITenantResolver<T> tenantResolver)
        {
            logger.LogDebug("Resolving TenantContext using {loggerType}.", tenantResolver.GetType().Name);

            var tenantContext = await tenantResolver.ResolveAsync(context);
            var tenantsConfig = tenantResolver.TenantsConfig;
            if (tenantContext != null)
            {
                logger.LogDebug("TenantContext resolved successful. Adding to HttpContext.");
                context.SetTenantContext(tenantContext);
            }
            else if (tenantsConfig.UnresolvedRedirect.HasValue())
            {
                logger.LogDebug("TenantContext resolved failure. Now redreact to unresolved url.");
                Redirect(context, tenantsConfig.UnresolvedRedirect, tenantsConfig.RedirectPermanent);
                return;
            }

            await next.Invoke(context);
        }

        private void Redirect(HttpContext context, string redirectUrl, bool permanent)
        {
            context.Response.Redirect(redirectUrl);
            context.Response.StatusCode =
                permanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
        }
    }
}
