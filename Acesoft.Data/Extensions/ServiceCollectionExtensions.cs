using System;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, Action<IStoreOption> optionAction)
        {
            if (optionAction == null)
            {
                throw new ArgumentNullException(nameof(optionAction));
            }

            var option = new StoreOption();
            optionAction.Invoke(option);
            services.AddSingleton<IStore>(new Store(option));

            return services;
        }

        public static IServiceCollection AddDataConfig(this IServiceCollection services)
        {
            // 添加数据访问配置data.config.json
            services.AddJsonConfig<DataConfig>(opts =>
            {
                opts.ConfigFile = "data.config.json";
                opts.IsTenantConfig = true;
            });

            return services;
        }
    }
}
