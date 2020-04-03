using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Acesoft.Config;
using Acesoft.Config.Xml;
using Acesoft.Data.Sql;
using Acesoft.Data.SqlMapper.Caching;
using Acesoft.Util;

namespace Acesoft.Data.SqlMapper
{
    public class SqlMap : XmlConfigData
    {
        public SqlScope Scope { get; set; }
        public string Id { get; private set; }
        public string SqlId => $"{Scope.Id}.{Id}";
        public Cache Cache { get; private set; }
        public IDictionary<string, string> Params { get; private set; }
        public IDictionary<string, string> Query { get; private set; }
        public IDictionary<string, string> Actions { get; private set; }

        public override void Load(XmlElement config)
        {
            base.Load(config);

            this.Id = config.GetAttribute("id");
            this.Params = ConfigContext.GetXmlConfigParams(config, "param");
            this.Query = ConfigContext.GetXmlConfigParams(config, "query");
            this.Actions = ConfigContext.GetXmlConfigParams(config, "action");

            var cache = config.GetAttribute("cache");
            if (cache.HasValue())
            {
                this.Cache = Scope.Caches[cache];
            }
        }

        public ICacheProvider CacheProvider => Cache.Provider;
        
        public void AppendSqlParams(RequestContext ctx)
        {
            // 处理查询语句中的参数
            if (Query.Count > 0)
            {
                // 根据Query设置sql参数
                foreach (var query in Query)
                {
                    if (!ctx.Params.ContainsKey(query.Key))
                    {
                        // 若包含了参数，暂不做处理
                        if (query.Value == "qs")
                        {
                            var val = App.GetQuery<string>(query.Key);
                            ctx.DapperParams.Add(query.Key, val);
                            ctx.Params.Add(query.Key, val);
                        }
                        else if (query.Value == "fs")
                        {
                            var val = App.GetForm<string>(query.Key);
                            ctx.DapperParams.Add(query.Key, val);
                            ctx.Params.Add(query.Key, val);
                        }
                        else if (query.Value == "ac")
                        {
                            var val = ctx.ExtraParams[query.Key];
                            ctx.DapperParams.Add(query.Key, val);
                            ctx.Params.Add(query.Key, val);
                        }
                        else
                        {
                            var val = query.Value;
                            ctx.DapperParams.Add(query.Key, val);
                            ctx.Params.Add(query.Key, val);
                        }
                    }
                }
            }
        }

        public string BuildSql(ISession session, RequestContext ctx)
        {
            var dialect = session.Store.Dialect;
            var sql = "";

            if (ctx.CmdType == CmdType.select)
            {
                sql = Params.GetValue("selectsql", Params.GetValue("sql", ""));
                if (!sql.HasValue())
                {
                    sql = BuildSelectSql(dialect, ctx);
                }
            }
            else if (ctx.CmdType == CmdType.insert)
            {
                CheckSql(session, "checkinsert", ctx);
                sql = Params.GetValue("insertsql", "");
                if (!sql.HasValue())
                {
                    sql = BuildInsertSql(dialect, ctx);
                }

                var insSql = Params.GetValue("afterinsertsql", "");
                if (insSql.HasValue())
                {
                    sql += " " + insSql;
                }
            }
            else if (ctx.CmdType == CmdType.update)
            {
                CheckSql(session, "checkupdate", ctx);
                sql = Params.GetValue("updatesql", "");
                if (!sql.HasValue())
                {
                    sql = BuildUpdateSql(dialect, ctx);
                }

                var updSql = Params.GetValue("afterupdatesql", "");
                if (updSql.HasValue())
                {
                    sql += " " + updSql;
                }
            }
            else if (ctx.CmdType == CmdType.delete)
            {
                CheckSql(session, "checkdelete", ctx);
                sql = Params.GetValue("deletesql", "");
                if (!sql.HasValue())
                {
                    sql = BuildDeleteSql(dialect, ctx);
                }
            }
            else
            {
                sql = Params.GetValue<string>("sql");
            }

            return sql;
        }

        private void CheckSql(ISession session, string key, RequestContext ctx)
        {
            var sql = Params.GetValue($"{key}sql", "");
            if (sql.HasValue())
            {
                var error = Params.GetValue($"{key}error", "校验未通过");
                var intVal = session.ExecuteScalar<int>(sql, ctx.DapperParams);
                if (intVal > 0)
                {
                    throw new AceException(error.Replace(ctx.DapperParams));
                }
            }
        }

        private string BuildSelectSql(ISqlDialect dialect, RequestContext ctx)
        {
            var props = ConvertHelper.ObjectToDictionary(ctx.NewObj);
            var sb = new StringBuilder($"select ");
            if (props.Count > 0)
            {
                sb.Append($"{dialect.QuoteForColumnName("id")}, ");
                foreach (var key in props.Keys)
                {
                    if (key.StartsWith("r__") || key.StartsWith("a__"))
                    {
                        // r__表示只读列，a__表示数组
                        sb.Append($"{dialect.QuoteForColumnName(key.Substring(3))} as {key}, ");
                    }
                    else if (key.StartsWith("e__"))
                    {
                        // e__表示编辑列
                        sb.Append($"{dialect.QuoteForColumnName(key.Substring(3))} as {key.Substring(3)}, ");
                    }
                    else if (!key.StartsWith("p__"))
                    {
                        // p__参数列之外
                        sb.Append($"{dialect.QuoteForColumnName(key)}, ");
                    }
                }
                sb.Remove(2);
            }
            else
            {
                sb.Append("*");
            }

            var idField = Params.GetValue("idfield", "id");
            sb.Append($" from {dialect.QuoteForTableName(GetTableName(true))}")
                .Append($" where {dialect.QuoteForColumnName(idField)}=@id");
            return sb.ToString();
        }

        private string BuildInsertSql(ISqlDialect dialect, RequestContext ctx)
        {
            var insertId = Params.GetValue("insertid", true);
            var insertTime = Params.GetValue("inserttime", true);

            var sbIns = new StringBuilder($"insert into {dialect.QuoteForTableName(GetTableName())} (");
            var sbVal = new StringBuilder(" values (");

            if (insertId)
            {
                // 自动插入id列
                sbIns.Append($"{dialect.QuoteForColumnName("id")}, ");
                sbVal.Append($"@id, ");
                //ctx.DapperParams.AddDynamicParams(new { id = App.IdWorker.NextId() });
            }
            if (insertTime)
            {
                // 自动插入dcreate列
                sbIns.Append($"{dialect.QuoteForColumnName("dcreate")}, ");
                sbVal.Append("getdate(), ");
                //ctx.DapperParams.AddDynamicParams(new { dcreate = DateTime.Now });
            }

            foreach (var key in ctx.DapperParams.ParameterNames)
            {
                var obj = ctx.DapperParams.Get<object>(key);
                if (obj is string str && !str.HasValue())
                {
                    // 提交的值为空时不插入
                    continue;
                }
                if (key == "id" || key.StartsWith("r__") || key.StartsWith("e__") || key.StartsWith("p__"))
                {
                    // r__表示只读列，e__表示编辑列
                    continue;
                }

                var field = key;
                if (key.StartsWith("a__"))
                {
                    // a__表示数组
                    field = field.Substring(3);
                }
                sbIns.Append($"{dialect.QuoteForColumnName(field)}, ");
                sbVal.Append($"@{key}, ");
            }
            sbIns.Remove(2).Append(")");
            sbVal.Remove(2).Append(")");

            return $"{sbIns}{sbVal}";
        }

        private string BuildUpdateSql(ISqlDialect dialect, RequestContext ctx)
        {
            var updateTime = Params.GetValue("updatetime", false);
            var sb = new StringBuilder($"update {dialect.QuoteForTableName(GetTableName())} set ");

            if (updateTime)
            {
                // 自动更新dupdate列
                sb.Append($"{dialect.QuoteForColumnName("dupdate")}=getdate(), ");
                //ctx.DapperParams.AddDynamicParams(new { dupdate = DateTime.Now });
            }

            foreach (var key in ctx.DapperParams.ParameterNames)
            {
                if (key == "id" || key.StartsWith("r__") || key.StartsWith("p__"))
                {
                    // r__表示只读列，p__表示参数列
                    continue;
                }

                var obj = ctx.DapperParams.Get<object>(key);
                var field = key;
                if (key.StartsWith("a__") || key.StartsWith("e__"))
                {
                    // a__表示数组
                    field = field.Substring(3);
                }

                if (obj is string str && !str.HasValue())
                {
                    sb.Append($"{dialect.QuoteForColumnName(field)}=null, ");
                }
                else
                {
                    sb.Append($"{dialect.QuoteForColumnName(field)}=@{key}, ");
                }
            }

            // 更新时还是按Id列来更新数据
            //var idField = Params.GetValue("idfield", "id");
            sb.Remove(2).Append($" where {dialect.QuoteForColumnName("id")}=@id");
            return sb.ToString();
        }

        private string BuildDeleteSql(ISqlDialect dialect, RequestContext ctx)
        {
            // 默认的删除语句用in，支付多个id
            var idField = Params.GetValue("idfield", "id");
            return $"delete from {dialect.QuoteForTableName(GetTableName())} " +
                $"where {dialect.QuoteForColumnName(idField)} in @ids";
        }

        private string GetTableName(bool read = false)
        {
            var table = Params.GetValue("table", "");
            if (!read) table = Params.GetValue("writetable", table);
            if (!table.HasValue())
            {
                throw new AceException($"SqlMap must have \"table\" or \"writetable\" params");
            }

            return table;
        }
    }
}
