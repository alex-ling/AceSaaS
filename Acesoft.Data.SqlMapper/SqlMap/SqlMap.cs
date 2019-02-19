using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Acesoft.Config;
using Acesoft.Config.Xml;
using Acesoft.Core;
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
                // 未设置时，初始化参数对象
                if (ctx.Param == null)
                {
                    ctx.Param = new { };
                }

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
            var provider = session.Store.Dialect;
            var sql = "";

            if (ctx.OpType == OpType.get)
            {
                sql = Params.GetValue("selectsql", "");
                if (!sql.HasValue())
                {
                    sql = BuildSelectSql(provider, ctx);
                }
            }
            else if (ctx.OpType == OpType.ins)
            {
                CheckSql(session, "checkinsert", ctx);
                sql = Params.GetValue("insertsql", "");
                if (!sql.HasValue())
                {
                    sql = BuildInsertSql(provider, ctx);
                }

                var insSql = Params.GetValue("afterinsertsql", "");
                if (insSql.HasValue())
                {
                    sql += insSql;
                }
            }
            else if (ctx.OpType == OpType.upd)
            {
                CheckSql(session, "checkupdate", ctx);
                sql = Params.GetValue("updatesql", "");
                if (!sql.HasValue())
                {
                    sql = BuildUpdateSql(provider, ctx);
                }

                var updSql = Params.GetValue("afterupdatesql", "");
                if (updSql.HasValue())
                {
                    sql += updSql;
                }
            }
            else if (ctx.OpType == OpType.del)
            {
                CheckSql(session, "checkdelete", ctx);
                sql = Params.GetValue("deletesql", "");
                if (!sql.HasValue())
                {
                    sql = BuildDeleteSql(provider, ctx);
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
                    throw new AceException(error.Replace(ctx.Params));
                }
            }
        }

        private string BuildSelectSql(ISqlDialect dialect, RequestContext ctx)
        {
            var props = ConvertHelper.ObjectToDictionary(ctx.NewObj);
            var sb = new StringBuilder($"select {dialect.QuoteForColumnName("id")}, ");

            foreach (var key in props.Keys)
            {
                if (key.StartsWith("__"))
                {
                    // rd表示只读列
                    continue;
                }
                if (key.StartsWith("ar_"))
                {
                    // ar表示数组
                    sb.Append($"{dialect.QuoteForColumnName(key.Substring(3))} as {key}, ");
                }
                else
                {
                    sb.Append($"{dialect.QuoteForColumnName(key)}, ");
                }
            }
            sb.Remove(2).Append($" from {dialect.QuoteForTableName(GetTableName())} where {dialect.QuoteForColumnName("id")}=@id");

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
                ctx.DapperParams.AddDynamicParams(new { id = DataContext.IdWorker.NextId() });
            }
            if (insertTime)
            {
                // 自动插入dcreate列
                sbIns.Append($"{dialect.QuoteForColumnName("dcreate")}, ");
                sbVal.Append("getdate(), ");
                //ctx.DapperParams.AddDynamicParams(new { dcreate = DateTime.Now });
            }

            foreach (var key in ctx.Params.Keys)
            {
                var obj = ctx.Params[key];
                if (obj is string str && !str.HasValue())
                {
                    // 提交的值为空时不插入
                    continue;
                }
                if (key.StartsWith("rd_"))
                {
                    // rd表示只读列
                    continue;
                }
                sbIns.Append($"{dialect.QuoteForColumnName(key)}, ");
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

            foreach (var key in ctx.Params.Keys)
            {
                if (key == "id" || key.StartsWith("rd_"))
                {
                    // rd表示只读列
                    continue;
                }

                var obj = ctx.Params[key];
                if (obj is string str && !str.HasValue())
                {
                    sb.Append($"{dialect.QuoteForColumnName(key)}=null, ");
                }
                else
                {
                    sb.Append($"{dialect.QuoteForColumnName(key)}=@{key}, ");
                }
            }

            sb.Remove(2).Append($" where {dialect.QuoteForColumnName("id")}=@id");
            return sb.ToString();
        }

        private string BuildDeleteSql(ISqlDialect dialect, RequestContext ctx)
        {
            // 默认的删除语句用in，支付多个id
            return $"delete from {dialect.QuoteForTableName(GetTableName())} where {dialect.QuoteForColumnName("id")} in @ids";
        }

        private string GetTableName()
        {
            var table = Params.GetValue("writetable", Params.GetValue("table", ""));
            if (!table.HasValue())
            {
                throw new AceException($"SqlMap must have \"table\" or \"writetable\" params");
            }

            return table;
        }
    }
}
