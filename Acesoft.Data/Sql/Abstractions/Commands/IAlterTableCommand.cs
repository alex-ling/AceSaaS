﻿using System;
using System.Data;

namespace Acesoft.Data.Sql.Schema
{
    public interface IAlterTableCommand : ISchemaCommand
    {
        void AddColumn(string columnName, DbType dbType, Action<IAddColumnCommand> column = null);
        void AddColumn<T>(string columnName, Action<IAddColumnCommand> column = null);
        void AlterColumn(string columnName, Action<IAlterColumnCommand> column = null);
        void DropColumn(string columnName);
        void CreateIndex(string indexName, params string[] columnNames);
        void DropIndex(string indexName);
    }
}
