using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Acesoft.Data;
using Acesoft.Data.Sql;

namespace Acesoft.Workflow.Schema
{
    public class WorkflowSchema : IStoreSchema
    {
        public void CreateSchema(ISession session)
        {
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session)
                    .CreateTable("rbac_scale", t => t.PrimaryKey()
                        .Column<long>("parentid")
                        .Column("ref_id", DbType.AnsiString, c => c.WithLength(50))
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
                        .Column<DateTime>("dcreate")
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
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_role", t => t.PrimaryKey()
                        .Column<long>("scale_id", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<int>("orderno")
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_ua", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<DateTime>("dstart")
                        .Column<DateTime>("dend")
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_object", t => t.PrimaryKey()
                        .Column<long>("parentid")
                        .Column<string>("name", c => c.NotNull().WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<int>("type", c => c.WithDefault(0))
                        .Column<string>("url", c => c.WithLength(255))
                        .Column<string>("icon", c => c.WithLength(50))
                        .Column<string>("opnames", c => c.WithLength(50))
                        .Column<bool>("visible")
                        .Column<int>("orderno", c => c.WithDefault(0))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_pa", t => t.PrimaryKey()
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<long>("ref_id", c => c.NotNull())
                        .Column<int>("opvalue", c => c.WithDefault(0))
                    )
                    .CreateTable("rbac_auth", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<long>("app_id", c => c.NotNull())
                        .Column<string>("authtype", c => c.WithLength(20))
                        .Column<string>("authid", c => c.WithLength(50))
                        .Column<DateTime>("dcreate")
                        .Column<DateTime>("dupdate")
                    )
                    .CreateForeignKey("fk_auth_user", "rbac_auth", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_param_user", "rbac_param", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_ua_user", "rbac_ua", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_user_scale", "rbac_user", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_role_scale", "rbac_role", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_ua_role", "rbac_ua", "role_id", "rbac_role", "id")
                    .CreateForeignKey("fk_pa_role", "rbac_pa", "role_id", "rbac_role", "id");

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
            
        }

        public void InitializeData(ISession session)
        {
            
        }
    }
}
