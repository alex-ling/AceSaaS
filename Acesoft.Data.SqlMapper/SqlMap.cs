using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Acesoft.NetCore.Config;
using Acesoft.Data.SqlMapper.Caching;

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

        //public ICacheProvider CacheProvider => Cache.Provider;

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
            var provider = session.Provider;
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

        private string BuildSelectSql(IDbProvider provider, RequestContext ctx)
        {
            var start = provider.StartQuote;
            var end = provider.CloseQuote;
            var table = Params.GetValue("table", "");

            var props = DictEx.FromObject(ctx.NewObj);
            var sbSel = new StringBuilder($"select {start}id{end},");
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
                    sbSel.Append($"{start}{key.Substring(3)}{end} as {key},");
                }
                else
                {
                    sbSel.Append($"{start}{key}{end},");
                }
            }
            sbSel.Remove().Append($" from {start}{table}{end} where {start}id{end}={provider.ParamChar}id");

            return sbSel.ToString();
        }

        private string BuildInsertSql(IDbProvider provider, RequestContext ctx)
        {
            var start = provider.StartQuote;
            var end = provider.CloseQuote;
            var table = Params.GetValue("table", "");
            table = Params.GetValue("writetable", table);
            var insertId = Params.GetValue("insertid", true);
            var insertTime = Params.GetValue("inserttime", true);

            var sbIns = new StringBuilder($"insert into {table}(");
            var sbVal = new StringBuilder("values(");
            if (insertId)
            {
                // 自动插入id列
                sbIns.Append($"{start}id{end},");
                sbVal.Append($"{provider.ParamChar}id,");
                ctx.DapperParams.AddDynamicParams(new { id = App.IdWorker.NextId() });
            }
            if (insertTime)
            {
                // 自动插入dcreate列
                sbIns.Append($"{start}dcreate{end},");
                sbVal.Append("getdate(),");
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
                sbIns.Append($"{start}{key}{end},");
                sbVal.Append($"{provider.ParamChar}{key},");
            }
            sbIns.Remove().Append(")");
            sbVal.Remove().Append(")");

            return $"{sbIns}{sbVal}";
        }

        private string BuildUpdateSql(IDbProvider provider, RequestContext ctx)
        {
            var start = provider.StartQuote;
            var end = provider.CloseQuote;
            var table = Params.GetValue("table", "");
            table = Params.GetValue("writetable", table);
            var updateTime = Params.GetValue("updatetime", false);

            var sbUpd = new StringBuilder($"update {start}{table}{end} set ");
            if (updateTime)
            {
                // 自动更新dupdate列
                sbUpd.Append($"{start}dupdate{end}=getdate(),");
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
                    sbUpd.Append($"{start}{key}{end}=null,");
                }
                else
                {
                    sbUpd.Append($"{start}{key}{end}={provider.ParamChar}{key},");
                }
            }
            sbUpd.Remove().Append($" where {start}id{end}={provider.ParamChar}id");

            return sbUpd.ToString();
        }

        private string BuildDeleteSql(IDbProvider provider, RequestContext ctx)
        {
            var start = provider.StartQuote;
            var end = provider.CloseQuote;
            var table = Params.GetValue("table", "");
            table = Params.GetValue("writetable", table);

            // 默认的删除语句用in，支付多个id
            return $"delete from {start}{table}{end} where {start}id{end} in {provider.ParamChar}ids";
        }

        public override void Load(XmlElement config)
        {
            base.Load(config);

            this.Id = config.GetAttribute("id");
            this.Params = ConfigFactory.GetConfigParams(config, "param");
            this.Query = ConfigFactory.GetConfigParams(config, "query");
            this.Actions = ConfigFactory.GetConfigParams(config, "action");

            var cache = config.GetAttribute("cache");
            if (cache.HasValue())
            {
                this.Cache = Scope.Caches[cache];
            }
        }
    }
}
