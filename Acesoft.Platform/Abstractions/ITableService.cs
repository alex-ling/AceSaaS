using System;
using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Platform.Entity;

namespace Acesoft.Platform
{
    public interface ITableService : IService<Sys_Table>
	{
        Sys_Table Get(string tableName);

		void CreateTable(string tableName);

		void DropTable(string tableName);

		void CreateFields(string tableName, long[] fieldIds);

		void DropFields(string tableName, long[] fieldIds);
	}
}
