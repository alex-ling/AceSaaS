using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public class Store : IStore
    {
        public IConfiguration Configuration { get; set; }
        public ISqlDialect Dialect { get; private set; }
        public IIdGenerator IdGenerator { get; private set; }

        static Store()
        {
            SqlMapper.ResetTypeHandlers();

            // Add Type Handlers here
        }

        public Store(Action<IConfiguration> config)
        {
            Configuration = new Configuration();
            config?.Invoke(Configuration);

            AfterConfigurationAssigned();
        }

        public Store(IConfiguration configuration)
        {
            Configuration = configuration;

            AfterConfigurationAssigned();
        }

        public void AfterConfigurationAssigned()
        {
            Dialect = SqlDialectFactory.For(Configuration.ConnectionFactory.DbConnectionType);
        }        

        public ISession OpenSession()
        {
            return OpenSession(Configuration.IsolationLevel);
        }

        public ISession OpenSession(IsolationLevel isolationLevel)
        {
            return new Session(this, isolationLevel);
        }

        public void Dispose()
        {
        }
    }
}
