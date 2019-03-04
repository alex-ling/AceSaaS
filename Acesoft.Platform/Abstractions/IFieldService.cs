using System;
using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Platform.Entity;

namespace Acesoft.Platform
{
	public interface IFieldService : IService<Sys_Field>
	{
		IList<Sys_Field> Gets(string tableName);

		IList<Sys_Field> Gets(string tableName, long[] fieldIds);
	}
}
