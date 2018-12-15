﻿using System;
using System.Collections.Generic;

namespace Acesoft.Data.Sql.Schema
{
    public abstract class TableCommand : ISchemaCommand, ITableCommand
    {
        public string Name { get; private set; }

        public List<ITableCommand> TableCommands { get; private set; }

        public TableCommand(string tableName)
        {
            Name = tableName;
        }
    }
}
