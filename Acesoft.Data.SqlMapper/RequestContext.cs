using System;
using System.Collections.Generic;
using System.Reflection;
using Acesoft.Util;
using Dapper;

namespace Acesoft.Data
{
    public class RequestContext
    {
        public string Scope { get; set; }
        public string SqlId { get; set; }
        public DynamicParameters DapperParams { get; private set; }
        public IDictionary<string, object> Params { get; private set; }
        public IDictionary<string, object> ExtraParams { get; set; }
        public CmdType CmdType { get; set; }
        public object NewObj { get; set; }

        public RequestContext(string sqlFullId)
        {
            var index = sqlFullId.IndexOf(".");
            if (index <= 0 || index >= sqlFullId.Length - 1)
            {
                throw new AceException("The SqlFullId must as：SqlScope.SqlId");
            }

            Init(sqlFullId.Substring(0, index), sqlFullId.Substring(index + 1));
        }

        public RequestContext(string scope, string sqlId)
        {
            Init(scope, sqlId);
        }

        public RequestContext SetCmdType(CmdType cmdType)
        {
            CmdType = cmdType;
            return this;
        }

        public RequestContext SetParam(object param)
        {
            if (param != null)
            {
                DapperParams.AddDynamicParams(param);
                Params.Merge(param);
            }
            return this;
        }

        public RequestContext SetExtraParam(object param)
        {
            ExtraParams.Merge(param);
            return this;
        }

        public RequestContext SetNewObj(object newObj)
        {
            NewObj = newObj;
            return this;
        }

        public RequestContext SetParam(string name, object value)
        {
            DapperParams.Add(name, value);
            Params.Add(name, value);
            return this;
        }

        private void Init(string scope, string sqlId)
        {
            Scope = scope;
            SqlId = sqlId;
            
            DapperParams = new DynamicParameters();
            Params = new Dictionary<string, object>();
            ExtraParams = new Dictionary<string, object>();
            CmdType = CmdType.sql;
        }
    }
}