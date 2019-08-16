using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Platform.Entity;

namespace Acesoft.Platform.Services
{
	public class FieldService : Service<Sys_Field>, IFieldService
	{
		public IList<Sys_Field> Gets(string tableName)
		{
			var sql = "select * from sys_field where [table]=@tableName and created=0 order by orderno";
			return Session.Query<Sys_Field>(sql, new
			{
                tableName
            }).ToList();
		}

		public IList<Sys_Field> Gets(string tableName, long[] fieldIds, int created = 0)
		{
			var sql = "select * from sys_field where [table]=@tableName and created=@created and id in @fieldIds";
			return Session.Query<Sys_Field>(sql, new
			{
                tableName,
                created,
				fieldIds
			}).ToList();
		}
	}
}
