using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;

using Acesoft.Web.Multitenancy;

namespace Acesoft.Web
{
    public static class HttpContextExtensions
    {
        private const string TenantContextKey = "Ace.TenantContext";

        public static void SetTenantContext(this HttpContext context, TenantContext tenantContext)
        {
            context.Items[TenantContextKey] = tenantContext;
        }

        public static TenantContext GetTenantContext(this HttpContext context)
        {
            if (context.Items.TryGetValue(TenantContextKey, out object tenantContext))
            {
                return tenantContext as TenantContext;
            }

            return null;
        }

        public static Tenant GetTenant(this HttpContext context)
        {
            var tenantContext = GetTenantContext(context);
            if (tenantContext != null)
            {
                return tenantContext.Tenant;
            }

            return null;
        }
    }
}
