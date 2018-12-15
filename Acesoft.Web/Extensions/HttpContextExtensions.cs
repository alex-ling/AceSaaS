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

        public static void SetTenantContext<T>(this HttpContext context, TenantContext<T> tenantContext)
        {
            context.Items[TenantContextKey] = tenantContext;
        }

        public static TenantContext<T> GetTenantContext<T>(this HttpContext context)
        {
            if (context.Items.TryGetValue(TenantContextKey, out object tenantContext))
            {
                return tenantContext as TenantContext<T>;
            }

            return null;
        }

        public static T GetTenant<T>(this HttpContext context)
        {
            var tenantContext = GetTenantContext<T>(context);
            if (tenantContext != null)
            {
                return tenantContext.Tenant;
            }

            return default(T);
        }

        public static TenantContext<Tenant> GetTenantContext(this HttpContext context)
        {
            return context.GetTenantContext<Tenant>();
        }

        public static Tenant GetTenant(this HttpContext context)
        {
            return context.GetTenant<Tenant>();
        }
    }
}
