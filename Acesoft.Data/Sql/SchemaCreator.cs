using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Acesoft.Data.Sql
{
    public class SchemaCreator : ISchemaCreator
    {
        private readonly ILogger<SchemaCreator> logger;

        public SchemaCreator(ILogger<SchemaCreator> logger)
        {
            this.logger = logger;
        }

        public void CreateSchema(ISession session)
        {
            if (!IsCreated(session))
            {
                logger.LogDebug($"Begin creating schemas for \"{session.Store.Option.Name}\" store");

                var schemas = App.Context.RequestServices.GetServices<IStoreSchema>();
                foreach (var schema in schemas)
                {
                    logger.LogDebug($"Creating schema for \"{schema.GetType().FullName}\"");
                    schema.DropSchema(session);
                    schema.CreateSchema(session);
                    schema.InitializeData(session);
                }

                logger.LogDebug("End creating schemas");
            }
        }

        public bool IsCreated(ISession session)
        {
            try
            {
                var count = session.ExecuteScalar<int>("select count(*) from rbac_user");
                logger.LogDebug($"Query {count} users from \"{session.Store.Option.Name}\" store");

                return true;
            }
            catch
            {
                logger.LogDebug($"Not found \"Rbac\" schema from \"{session.Store.Option.Name}\" store");

                return false;
            }
        }
    }
}
