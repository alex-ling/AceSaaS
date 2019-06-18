using System;
using System.Data;
using System.Linq;

using Acesoft.Data;
using Acesoft.Data.Sql;
using Acesoft.Platform.Entity;

namespace Acesoft.Platform.Services
{
	public class TableService : Service<Sys_Table>, ITableService
	{
        private IFieldService fieldService;

        public TableService(IFieldService fieldService)
        {
            this.fieldService = fieldService;
        }

		public Sys_Table Get(string tableName)
		{
			var sql = "select * from sys_table where [table]=@tableName";
			return Session.QueryFirst<Sys_Table>(sql, new { tableName });
		}

        public Sys_Table Query(string tableName)
        {
            var sql = "select * from sys_table where [table]=@tableName;"
                + "select * from sys_field where [table]=@tableName and created=1 order by orderno";
            var reader = Session.QueryMultiple(sql, new { tableName });
            var table = reader.Read<Sys_Table>(true).SingleOrDefault();
            if (table != null)
            {
                table.Fields = reader.Read<Sys_Field>(true).ToList();
            }
            return table;
        }

        public void UpdateCreated(string tableName, int created)
		{
			var sql = "update sys_table set created=@created where [table]=@tableName;" +
                "update sys_field set created=@created where [table]=@tableName";
			Session.Execute(sql, new
			{
                tableName,
				created
			});
		}

		public void UpdateCreated(string tableName, long[] fieldIds, int created)
		{
			var sql = "update sys_field set created=@created where [table]=@tableName and id in @fieldIds";
			Session.Execute(sql, new
			{
                tableName,
				fieldIds,
				created
			});
		}

		public void CreateTable(string tableName)
		{
			var table = Get(tableName);
            Check.Assert(table.Created, $"表 [{table.Table}.{table.Name}] 已构建，无需构建");
            var fields = fieldService.Gets(tableName);

            Session.BeginTransaction();
			try
            {
                var sb = new SchemaBuilder(Session).CreateTable(table.Table, t =>
                {
                    foreach (var item in fields)
                    {
                        t.Column(item.Field, GetDbType(item.Type), c =>
                        {
                            if (item.Type == FieldType.key) c.PrimaryKey();
                            if (!item.IsNull) c.NotNull();
                            if (item.Default.HasValue()) c.WithDefault(item.Default);
                            if (item.Length.HasValue) c.WithLength(item.Length);
                            if (item.Type == FieldType.text) c.Unlimited();
                            if (item.Unique) c.Unique();
                        });
                    }
                });

                foreach (var fk in fields.Where(f => f.Type == FieldType.fkey))
                {
                    sb.CreateForeignKey(
                        $"FK_{table.Table}_{fk.Field}_{fk.Ref}",
                        table.Table, 
                        fk.Field, 
                        fk.Ref, 
                        "id");
                }

				UpdateCreated(table.Table, 1);

				Session.Commit();
			}
			catch (Exception ex)
			{
				Session.Rollback();
				throw new AceException(ex.GetMessage());
			}
		}

		public void DropTable(string tableName)
		{
			var table = Get(tableName);
            Check.Require(table.Created, $"表 [{table.Table}.{table.Name}] 未构建，无需撤销");

			Session.BeginTransaction();
			try
			{
                new SchemaBuilder(Session).DropTable(table.Table);

				UpdateCreated(table.Table, 0);

				Session.Commit();
			}
			catch (Exception ex)
			{
				Session.Rollback();
				throw new AceException(ex.GetMessage());
			}
		}

		public void CreateFields(string tableName, long[] fieldIds)
		{
			var table = Get(tableName);
            Check.Require(table.Created, $"表 [{table.Table}.{table.Name}] 未构建，请先构建表");
            var fields = fieldService.Gets(tableName, fieldIds);

			Session.BeginTransaction();
			try
			{
                var sb = new SchemaBuilder(Session).AlterTable(table.Table, t =>
                {
                    foreach (var item in fields)
                    {
                        t.AddColumn(item.Field, GetDbType(item.Type), c =>
                        {
                            if (item.Type == FieldType.key) c.PrimaryKey();
                            if (!item.IsNull) c.NotNull();
                            if (item.Default.HasValue()) c.WithDefault(item.Default);
                            if (item.Length.HasValue) c.WithLength(item.Length);
                            if (item.Type == FieldType.text) c.Unlimited();
                        });
                    }
                });

                foreach (var fk in fields.Where(f => f.Type == FieldType.fkey))
                {
                    sb.CreateForeignKey(
                        $"FK_{table.Table}_{fk.Field}_{fk.Ref}",
                        table.Table,
                        fk.Field,
                        fk.Ref,
                        "id");
                }

                UpdateCreated(table.Table, fieldIds, 1);

				Session.Commit();
			}
			catch (Exception ex)
			{
				Session.Rollback();
				throw new AceException(ex.GetMessage());
			}
		}

		public void DropFields(string tableName, long[] fieldIds)
		{
			var table = Get(tableName);
            Check.Require(table.Created, $"表 [{table.Table}.{table.Name}] 未构建，无需撤销");
            var fields = fieldService.Gets(tableName, fieldIds, 1);

			Session.BeginTransaction();
			try
			{
                var sb = new SchemaBuilder(Session);

                foreach (var fk in fields.Where(f => f.Type == FieldType.fkey))
                {
                    sb.DropForeignKey(
                        table.Table,
                        $"FK_{table.Table}_{fk.Field}_{fk.Ref}");
                }

                foreach (var item in fields)
                {
                    sb.AlterTable(table.Table, t => t.DropColumn(item.Field));
                }

                UpdateCreated(table.Table, fieldIds, 0);

				Session.Commit();
			}
			catch (Exception ex)
			{
				Session.Rollback();
				throw new AceException(ex.GetMessage());
			}
		}

        private DbType GetDbType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.nvarchar:
                case FieldType.media:
                case FieldType.attach:
                case FieldType.text:
                    return DbType.String;

                case FieldType.varchar:
                case FieldType.dict:
                case FieldType.rcode:
                    return DbType.AnsiString;

                case FieldType.boolean:
                    return DbType.Boolean;

                case FieldType.integer:
                    return DbType.Int32;

                case FieldType.datetime:
                    return DbType.DateTime;

                case FieldType.numeric:
                    return DbType.Decimal;

                case FieldType.key:
                case FieldType.fkey:
                case FieldType.rkey:
                    return DbType.Int64;

                default:
                    return DbType.String;
            }
        }
    }
}
