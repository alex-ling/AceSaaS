using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Acesoft.Config;
using Acesoft.Web.Modules;
using Acesoft.Web.Multitenancy;
using Acesoft.Web.Mvc;
using Acesoft.Platform;
using Acesoft.Rbac;

namespace Acesoft.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfig(this IServiceCollection services)
        {
            return services.AddJsonConfig<AppConfig>(opts =>
            {
                opts.ConfigFile = "app.config.json";
            });
        }

        public static IServiceCollection AddMultitenancy(this IServiceCollection services)
        {
            return services.AddMultitenancy<DefaultTenantResolver>();
        }

        public static IServiceCollection AddMultitenancy<R>(this IServiceCollection services)
            where R : class, ITenantResolver
        {
            var mvcBuilder = AddDefaultServices(services);
            services.AddSingleton(mvcBuilder);

            // add Modules
            AddModules(services);

            // add tentans
            AddMultitenancyCore<R>(services);

            // add self
            services.AddSingleton(_ => services);

            return services;
        }

        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            // load internal/core modules
            ModuleLoader.LoadInternalModules();

            // add modules services
            ModuleLoader.ConfigureServices(services);

            // add module container builder
            services.AddSingleton<IModulesHost, ModulesHost>();

            return services;
        }

        private static IMvcBuilder AddDefaultServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 引入HttpClient
            //services.AddHttpClient();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRouting();

            // Add compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            // AddMvc
            var webpageRoutes = App.AppConfig.Settings.GetValue("webpage.routes", "");
            return services.AddMvc(opts =>
            {
                // 改为ExceptionMiddleware处理
                opts.Filters.Add<ExceptionFilter>();
                opts.Filters.Add<ApiResultFilter>();
                opts.RespectBrowserAcceptHeader = true;
            })
            .AddRazorOptions(opts =>
            {
                // 此处可添加外部views目录
                //opts.FileProviders.Add(new PhysicalFileProvider(""));
                opts.AllowRecompilingViewsOnFileChange = true;
            })
            .AddRazorPagesOptions(opts =>
            {
                // 此处设置RazorPages
                // 匹配所有未知Url，包含定制动态页面
                //opts.Conventions.AddPageRoute("/desktop", "{*url}");
                // {*url}反斜杠编码，{**url}反斜杠不编码{text?}是否存在
                webpageRoutes.Split<string>(',').Each(page =>
                {
                    opts.Conventions.AddPageRoute("/desktop", $"/{page}/{{**url}}");
                });
            })
            .AddJsonOptions(opts =>
            {
                opts.SerializerSettings.Converters.Add(new LongConverter());
                opts.SerializerSettings.Converters.Add(new TreeConverter());
                opts.SerializerSettings.Converters.Add(new TreeNodeConverter());
                opts.SerializerSettings.Converters.Add(new GridConverter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        private static void AddMultitenancyCore<R>(this IServiceCollection services)
            where R : class, ITenantResolver
        {
            // Add config
            services.AddJsonConfig<TenantsConfig>(opts =>
            {
                opts.ConfigFile = "tenants.config.json";
            });

            // Make TenantResolver
            services.AddScoped<ITenantResolver, R>();

            // Make Tenant and TenantContext injectable
            services.AddSingleton<ITenantsHost, TenantsHost>();
            services.AddSingleton<ITenantContainerFactory, TenantContainerFactory>();
            services.AddScoped(p => p.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext());
            services.AddScoped(p => p.GetService<TenantContext>()?.Tenant);

            // Make ITenant injectable for handling null injection, similar to IOptions
            services.AddScoped<ITenant>(p => new TenantWrapper(p.GetService<Tenant>()));

            // Ensure caching is available for caching resolvers
            if (typeof(MemoryTenantResolver).IsAssignableFrom(typeof(R)))
            {
                services.AddMemoryCache();
            }
        }
    }
}
