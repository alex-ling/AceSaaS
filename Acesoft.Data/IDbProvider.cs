using System;
using System.Collections.Generic;
using System.Data;

using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public interface IDbProvider
    {
        string StartQuote { get; }
        string CloseQuote { get; }
        string ParamChar { get; }
        string CreateTableString { get; }
        string PrimaryKeyString { get; }
        string NullColumnString { get; }

        IDictionary<string, string> Properties { get; }

        IDbConnection GetConnection();
        IDbProvider Configure(IDictionary<string, string> config);
        string GetQuoteName(string name);
        string GetDbTypeString(DbType type, int length = 0, int precision = 0);
        string GetDropTableString(string name);
        string GetAddForeignKeyString(string name, string[] fKeys, string refTable, string[] pKeys, bool refPKey);
        string GetDropForeignKeyString(string name, string fkey);
    }
}