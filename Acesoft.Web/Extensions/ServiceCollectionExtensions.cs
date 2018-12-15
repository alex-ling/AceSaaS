using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Acesoft.Core;
using Acesoft.Config;
using Acesoft.Web.Modules;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy(this IServiceCollection services)
        {
            return services.AddMultitenancy<Tenant, CacheTenantResolver>();
        }

        public static IServiceCollection AddMultitenancy<T, R>(this IServiceCollection services)
            where R : class, ITenantResolver<T>
            where T : class
        {
            AddDefaultServices(services);

            // add Modules
            AddModules(services);

            // add tentans
            AddMultitenancyCore<T, R>(services);

            return services;
        }

        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            // add ModuleContainer
            var moduleContainer = new ModuleContainer();
            services.AddSingleton(moduleContainer);

            moduleContainer.LoadInternalModules()
                .LoadExternalModules()
                .ConfigureServices(services);

            return services;
        }

        private static void AddDefaultServices(IServiceCollection services)
        {
            services.AddRouting();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void AddMultitenancyCore<T, R>(this IServiceCollection services)
            where R : class, ITenantResolver<T>
            where T : class
        {
            // Add config
            services.AddJsonConfig<TenantsConfig>(opts =>
            {
                opts.ConfigFile = "tenants.config.json";
            });

            // Make TenantResolver
            services.AddScoped<ITenantResolver<T>, R>();

            // Make Tenant and TenantContext injectable
            services.AddScoped(p => p.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<T>());
            services.AddScoped(p => p.GetService<TenantContext<T>>()?.Tenant);

            // Make ITenant injectable for handling null injection, similar to IOptions
            services.AddScoped<ITenant<T>>(p => new TenantWrapper<T>(p.GetService<T>()));

            // Ensure caching is available for caching resolvers
            if (typeof(MemoryTenantResolver<T>).IsAssignableFrom(typeof(R)))
            {
                services.AddMemoryCache();
            }
        }
    }
}
