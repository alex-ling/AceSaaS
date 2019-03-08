using System;
using System.Collections.Generic;
using System.Data;

using Acesoft.Data;
using Acesoft.Data.Sql;

namespace Acesoft.Platform.Schema
{
    public class PlatformSchema : IStoreSchema
    {
        public void CreateSchema(ISession session)
        {
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session)
                    .CreateTable("sys_schema", t => t.PrimaryKey()
                        .Column("code", DbType.AnsiString, c => c.NotNull().WithLength(20))
                        .Column<string>("name", c => c.NotNull().WithLength(20))
                        .Column<int>("orderno")
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_table", t => t.PrimaryKey()
                        .Column<long>("schema_id", c => c.NotNull())
                        .Column("table", DbType.AnsiString, c => c.Unique().WithLength(20))
                        .Column<string>("name", c => c.NotNull().WithLength(20))
                        .Column<bool>("created")
                        .Column<string>("remark", c => c.WithLength(255))
                        .Column<int>("orderno")
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_field", t => t.PrimaryKey()
                        .Column("table", DbType.AnsiString, c => c.NotNull().WithLength(20))
                        .Column("field", DbType.AnsiString, c => c.WithLength(20))
                        .Column<string>("name", c => c.NotNull().WithLength(20))
                        .Column<int>("type")
                        .Column<int>("length")
                        .Column<bool>("isnull")
                        .Column("default", DbType.AnsiString, c => c.WithLength(50))
                        .Column<bool>("unique")
                        .Column("ref", DbType.AnsiString, c => c.WithLength(50))
                        .Column<bool>("created")
                        .Column<string>("remark", c => c.WithLength(255))
                        .Column<int>("orderno")
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_seed", t => t.PrimaryKey()
                        .Column("name", DbType.AnsiString, c => c.WithLength(50))
                        .Column("value", DbType.AnsiString, c => c.NotNull().WithLength(50))
                        .Column<DateTime>("dcreate")
                        .Column<DateTime>("dupdate")
                    )
                    .CreateTable("sys_media", t => t.PrimaryKey()
                        .Column<long>("user_id")
                        .Column<string>("title", c => c.WithLength(50))
                        .Column<string>("url")
                        .Column<int>("type")
                        .Column<bool>("sync")
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_send", t => t.PrimaryKey()
                        .Column<long>("parentid")
                        .Column<long>("user_id")
                        .Column<long>("template_id")
                        .Column<string>("sender", c => c.WithLength(50))
                        .Column<string>("title", c => c.WithLength(50))
                        .Column<string>("content", c => c.Unlimited())
                        .Column<int>("type")
                        .Column<int>("status")
                        .Column("rec_ids", DbType.AnsiString, c => c.NotNull())
                        .Column<string>("rec_names")
                        .Column<long>("ref_id")
                        .Column<int>("trytimes")
                        .Column<DateTime>("dcreate")
                        .Column<DateTime>("dupdate")
                    )
                    .CreateTable("sys_receive", t => t.PrimaryKey()
                        .Column<long>("user_id")
                        .Column<long>("send_id")
                        .Column<int>("status")
                        .Column("rec_addr", DbType.AnsiString, c => c.NotNull().WithLength(50))
                        .Column<int>("trytimes")
                        .Column<DateTime>("dcreate")
                        .Column<DateTime>("dupdate")
                    )
                    .CreateTable("sys_dict", t => t.PrimaryKey()
                        .Column("dict", DbType.AnsiString, c => c.Unique().WithLength(20))
                        .Column<string>("name", c => c.WithLength(20))
                        .Column("type", DbType.AnsiString, c => c.WithLength(20))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_dictitem", t => t.PrimaryKey()
                        .Column("parentid", DbType.AnsiString, c => c.WithLength(20))
                        .Column("dict", DbType.AnsiString, c => c.WithLength(20))
                        .Column("value", DbType.AnsiString, c => c.WithLength(20))
                        .Column<string>("name", c => c.WithLength(50))
                        .Column<int>("orderno")
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_cfg", t => t.PrimaryKey()
                        .Column("code", DbType.AnsiString, c => c.WithLength(20))
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("sys_cfgitem", t => t.PrimaryKey()
                        .Column<long>("cfg_id")
                        .Column("key", DbType.AnsiString, c => c.WithLength(20))
                        .Column<string>("value")
                        .Column<string>("name", c => c.WithLength(50))
                        .Column<int>("orderno")
                        .Column<DateTime>("dcreate")
                    )
                    .CreateForeignKey("fk_table_schema", "sys_table", "schema_id", "sys_schema", "id")
                    .CreateForeignKey("fk_field_table", "sys_field", "table", "sys_table", "table")
                    .CreateForeignKey("fk_receive_send", "sys_receive", "send_id", "sys_send", "id")
                    .CreateForeignKey("fk_dictitem_dict", "sys_dictitem", "dict", "sys_dict", "dict")
                    .CreateForeignKey("fk_cfgitem_cfg", "sys_cfgitem", "cfg_id", "sys_cfg", "id");

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
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session, false)
                .DropForeignKey("sys_table", "fk_table_schema")
                .DropForeignKey("sys_field", "fk_field_table")
                .DropForeignKey("sys_receive", "fk_receive_send")
                .DropForeignKey("sys_dictitem", "fk_dictitem_dict")
                .DropForeignKey("sys_cfgitem", "fk_cfgitem_cfg")
                .DropTable("sys_table")
                .DropTable("sys_field")
                .DropTable("sys_schema")
                .DropTable("sys_seed")
                .DropTable("sys_media")
                .DropTable("sys_receive")
                .DropTable("sys_send")
                .DropTable("sys_dictitem")
                .DropTable("sys_dict")
                .DropTable("sys_cfgitem")
                .DropTable("sys_cfg");

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public void InitializeData(ISession session)
        {

        }
    }
}
