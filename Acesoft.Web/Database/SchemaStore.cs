using System;
using System.Collections.Generic;
using System.Data;

using Acesoft.Data;
using Acesoft.Data.Sql;

namespace Acesoft.Web.Database
{
    public class SchemaStore : ISchemaStore
    {
        private IApplicationContext appCtx;
        public SchemaStore(IApplicationContext appCtx)
        {
            this.appCtx = appCtx;
        }

        public void CreateSchema()
        {
            var s = appCtx.Session;
            s.BeginTransaction();

            try
            {
                new SchemaBuilder(s)
                    .CreateTable("rbac_scale", t => t.PrimaryKey()
                        .Column<long>("parentid")
                        .Column("ref_id", DbType.AnsiString)
                        .Column<string>("name", c => c.NotNull().WithLength(50))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_user", t => t.PrimaryKey()
                        .Column<long>("scale_id", c => c.NotNull())
                        .Column<long>("client_id")
                        .Column<string>("loginname", c => c.WithLength(20))
                        .Column<string>("nickname", c => c.WithLength(20))
                        .Column<string>("username", c => c.WithLength(20))
                        .Column<string>("password", c => c.WithLength(50))
                        .Column<string>("creator", c => c.WithLength(20))
                        .Column<string>("refcode", c => c.WithLength(20))
                        .Column<string>("weunionid", c => c.WithLength(50))
                        .Column<int>("usertype", c => c.WithDefault(0))
                        .Column<int>("regtype", c => c.WithDefault(0))
                        .Column<bool>("enabled", c => c.WithDefault(true))
                        .Column<string>("mobile", c => c.WithLength(20))
                        .Column<string>("mail", c => c.WithLength(50))
                        .Column<string>("photo", c => c.WithLength(255))
                        .Column<int>("sex")
                        .Column<DateTime>("birthdate")
                        .Column<string>("province", c => c.WithLength(20))
                        .Column<string>("city", c => c.WithLength(20))
                        .Column<string>("county", c => c.WithLength(20))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                        .Column<DateTime>("dupdate")
                        .Column<DateTime>("dlogin")
                        .Column<string>("loginip", c => c.WithLength(20))
                        .Column<bool>("rstpwd")
                        .Column<DateTime>("drstpwd")
                        .Column<int>("trytimes")
                        .Column<string>("remark", c => c.WithLength(255))
                    )
                    .CreateTable("rbac_param", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("value", c => c.WithLength(50))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_role", t => t.PrimaryKey()
                        .Column<long>("scale_id", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_ua", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<DateTime>("dstart")
                        .Column<DateTime>("dend")
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_object", t => t.PrimaryKey()
                        .Column<long>("parentid", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<int>("type", c => c.WithDefault(0))
                        .Column<string>("url", c => c.WithLength(255))
                        .Column<string>("icon", c => c.WithLength(50))
                        .Column<string>("opnames", c => c.WithLength(50))
                        .Column<bool>("visible")
                        .Column<int>("orderno", c => c.WithDefault(0))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_pa", t => t.PrimaryKey()
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<long>("ref_id", c => c.NotNull())
                        .Column<int>("opvalue", c => c.WithDefault(0))
                    )
                    .CreateTable("rbac_auth", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<string>("authtype", c => c.WithLength(20))
                        .Column<string>("authid", c => c.WithLength(20))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                        .Column<DateTime>("dupdate")
                    )
                    .CreateForeignKey("fk_auth_user", "rbac_auth", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_param_user", "rbac_param", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_ua_user", "rbac_ua", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_user_scale", "rbac_user", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_role_scale", "rbac_role", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_ua_role", "rbac_ua", "role_id", "rbac_role", "id")
                    .CreateForeignKey("fk_pa_role", "rbac_pa", "role_id", "rbac_role", "id");

                s.Commit();
            }
            catch
            {
                s.Rollback();

                throw;
            }
        }

        public void DropSchema()
        {
            var s = appCtx.Session;
            s.BeginTransaction();

            try
            {
                new SchemaBuilder(s, false)
                .DropTable("rbac_auth")
                .DropTable("rbac_pa")
                .DropTable("rbac_object")
                .DropTable("rbac_ua")
                .DropTable("rbac_role")
                .DropTable("rbac_param")
                .DropTable("rbac_user")
                .DropTable("rbac_scale");

                s.Commit();
            }
            catch
            {
                s.Rollback();

                throw;
            }
        }
    }
}
