using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Acesoft.Config;
using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStore(this IServiceCollection services, Action<IConfiguration> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var config = new Configuration();
            setupAction.Invoke(config);
            services.AddSingleton<IStore>(new Store(config));

            return services;
        }

        public static IServiceCollection AddStroeConfig(this IServiceCollection services)
        {
            // 添加数据访问配置data.config.json
            services.AddJsonConfig<DataConfig>(opts =>
            {
                opts.ConfigFile = "data.config.json";
                opts.TenantConfig = true;
            });

            return services;
        }
    }
}
