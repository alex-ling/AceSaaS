using System;
using System.Collections.Generic;
using System.Data;

using Acesoft.Data;
using Acesoft.Data.Sql;

namespace Acesoft.Platform.Schema
{
    public class AppSchema : IStoreSchema
    {
        public void CreateSchema(ISession session)
        {
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session)
                    .CreateTable("app_client", t => t.PrimaryKey()
                        .Column<string>("name", c => c.NotNull().WithLength(20))
                        .Column<string>("remark", c => c.WithLength(255))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("app_version", t => t.PrimaryKey()
                        .Column<long>("client_id", c => c.NotNull())
                        .Column<int>("type")
                        .Column("version", DbType.AnsiString, c => c.WithLength(20))
                        .Column<DateTime>("pubdate")
                        .Column<string>("package")
                        .Column<string>("changelog")
                        .Column<bool>("force")
                        .Column<bool>("current")
                        .Column<string>("remark", c => c.WithLength(255))
                        .Column<DateTime>("dcreate")
                        .Column<DateTime>("dupdate")
                    )
                    .CreateForeignKey("fk_version_app", "app_version", "client_id", "app_client", "id");

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public void DropSchema(ISession session)
        {
            new SchemaBuilder(session, false)
                .DropForeignKey("app_version", "fk_version_app")
                .DropTable("app_version")
                .DropTable("app_client");
        }

        public void InitializeData(ISession session)
        {
            
        }
    }
}
