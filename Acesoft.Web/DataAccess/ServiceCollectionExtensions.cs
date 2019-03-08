using System;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Data;
using Acesoft.Data.Config;

namespace Acesoft.Web.DataAccess
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddSingleton<IStore>(sp =>
            {
                var tenant = sp.GetService<IApplicationContext>().TenantContext.Tenant;

                // get tentant's store config.
                var dataConfig = ConfigContext.GetConfig<DataConfig>(tenant.Name);

                // init store configuration.
                var option = new StoreOption();

                // Disabling query gating as it's failing to improve performance right now
                option.UseDatabase(tenant.Name).UseConfig(dataConfig);

                // create schemas.
                var store = new Store(option);
                if (option.AutoCreateSchema)
                {
                    var storeCreator = sp.GetService<ISchemaCreator>();
                    if (storeCreator != null)
                    {
                        using (var session = store.OpenSession())
                        {
                            storeCreator.CreateSchema(session);
                        }
                    }
                }

                return store;
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
