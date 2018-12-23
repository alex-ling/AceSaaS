using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Acesoft.Data;
using Acesoft.Web.Modules;

namespace Acesoft.Web.Multitenancy
{
    public class TenantContainerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IModulesHost modulesHost;
        private readonly ITenantsHost tenantsHost;
        private readonly TenantsConfig tenantsConfig;
        private readonly ILogger logger;

        public TenantContainerMiddleware(RequestDelegate next, IModulesHost modulesHost, 
            ITenantsHost tenantsHost, IOptions<TenantsConfig> tenantsOption, ILogger<TenantContainerMiddleware> logger)
        {
            this.next = next;
            this.modulesHost = modulesHost;
            this.tenantsHost = tenantsHost;
            this.tenantsConfig = tenantsOption.Value;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, ITenantResolver tenantResolver)
        { 
            tenantsHost.Initialize();

            logger.LogDebug($"Resolving Tenant using \"{tenantResolver.GetType().FullName}\".");
            var tenant = await tenantResolver.ResolveAsync(context);
            if (tenant != null)
            {
                logger.LogDebug("Start getting TenantContext from TenantsHost.");

                var tenantContext = tenantsHost.GetOrCreateContext(tenant);
                context.SetTenantContext(tenantContext);
                logger.LogDebug("Getting TenantContext successful and set to HttpContext.");

                using (var scope = tenantContext.EnterServiceScope())
                {
                    var entity = context.RequestServices.GetService<IEntity>();

                    await next.Invoke(context);
                }
            }
            else if (tenantsConfig.UnresolvedRedirect.HasValue())
            {
                logger.LogDebug($"Tenant resolved failure. Now redreact to url: {tenantsConfig.UnresolvedRedirect}.");
                Redirect(context, tenantsConfig.UnresolvedRedirect, tenantsConfig.RedirectPermanent);
            }
            else
            {
                await next.Invoke(context);
            }
        }

        private void Redirect(HttpContext context, string redirectUrl, bool permanent)
        {
            context.Response.Redirect(redirectUrl);
            context.Response.StatusCode =
                permanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
        }
    }
}