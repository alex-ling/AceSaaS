using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Core;
using Acesoft.Data;
using Acesoft.Data.Config;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddSingleton<IStore>(sp =>
            {
                var tenant = sp.GetService<IApplicationContext>().Tenant;

                // get tentant's store config.
                var dataConfig = ConfigContext.GetConfig<DataConfig>(tenant.Name);

                // init store configuration.
                var option = new StoreOption();
                // Disabling query gating as it's failing to improve performance right now
                option.DisableQueryGating().UseDatabase(tenant.Name).UseConfig(dataConfig);

                return new Store(option);
            });

            services.AddScoped(sp =>
            {
                var store = sp.GetRequiredService<IStore>();
                return store.OpenSession();
            });

            return services;
        }
    }
}
