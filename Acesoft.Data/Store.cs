using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Dapper;
using Acesoft.Logger;

namespace Acesoft.Data
{
    public class Store : IStore
    {
        private static ILogger<Store> logger = LoggerContext.GetLogger<Store>();

        public IStoreOption Option { get; set; }
        public ISqlDialect Dialect { get; private set; }
        public IIdGenerator IdGenerator { get; private set; }

        static Store()
        {
            SqlMapper.ResetTypeHandlers();

            // Add Type Handlers here
        }

        public Store(Action<IStoreOption> optionAction)
        {
            Option = new StoreOption();
            optionAction?.Invoke(Option);

            AfterOptionAssigned();
        }

        public Store(IStoreOption option)
        {
            Option = option;

            AfterOptionAssigned();
        }

        public void AfterOptionAssigned()
        {
            Dialect = SqlDialectFactory.For(Option.ConnectionFactory.DbConnectionType);

            logger.LogDebug($"Store has initlialized with database \"{Option.Name}\"");
        }        

        public ISession OpenSession()
        {
            return OpenSession(Option.IsolationLevel);
        }

        public ISession OpenSession(IsolationLevel isolationLevel)
        {
            logger.LogDebug($"Open new session with database \"{Option.Name}\"");

            return new Session(this, isolationLevel);
        }

        public void Dispose()
        {
        }
    }
}
