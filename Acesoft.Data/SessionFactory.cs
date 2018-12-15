using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Web;

using Acesoft.NetCore.Core;
using Acesoft.NetCore.Config;
using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public class SessionFactory
    {
        static SessionFactory instance = null;
        static readonly ConcurrentDictionary<string, IDbProvider> providers = new ConcurrentDictionary<string, IDbProvider>();

        public static SessionFactory Instance => instance;

        private SessionFactory()
        { }

        static SessionFactory()
        {
            instance = new SessionFactory();
        }

        public ISession OpenSession(string database = null)
        {
            if (database == null)
            {
                database = DataConfig.GetDatabase();
            }

            var provider = GetDbProvider(database);
            return new Session(provider);
        }

        public IDbProvider GetDbProvider(string database)
        {
            IDbProvider provider = null;
            return providers.GetOrAdd(database, (key) =>
            {
                var cfg = ConfigFactory.GetConfig<DataConfig>();
                IDictionary<string, string> config = null;
                if (!cfg.Databases.TryGetValue(database, out config))
                {
                    throw new AceException($"Not found the AppName：{database}");
                }

                var providerType = config.GetValue<string>(DataConfig.DbProvider);
                var type = Type.GetType(providerType);
                if (type == null || !typeof(IDbProvider).IsAssignableFrom(type))
                {
                    throw new AceException($"{providerType} not assign from IDbProvider");
                }

                provider = Dynamic.GetInstanceCreator(type)() as IDbProvider;
                return provider.Configure(config);
            });
        }
    }
}
