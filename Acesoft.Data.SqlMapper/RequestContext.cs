using System;
using System.Collections.Generic;
using System.Reflection;
using Acesoft.Util;
using Dapper;

namespace Acesoft.Data.SqlMapper
{
    public class RequestContext
    {
        private object param;
        internal DynamicParameters DapperParams { get; set; }
        public IDictionary<string, object> Params { get; set; }
        public IDictionary<string, object> ExtraParams { get; set; }

        public string Scope { get; set; }
        public string SqlId { get; set; }
        public OpType OpType { get; set; }
        public object NewObj { get; set; }
        public object Param
        {
            get { return param; }
            set
            {
                param = value;
                if (param == null)
                {
                    DapperParams = null;
                    Params = null;
                    return;
                }

                DapperParams = new DynamicParameters(param);
                Params = new SortedDictionary<string, object>(ConvertHelper.ObjectToDictionary(param));
            }
        }

        public RequestContext()
        { }

        public RequestContext(string sqlFullId)
        {
            var index = sqlFullId.IndexOf(".");
            if (index <= 0)
            {
                throw new AceException("未设置SqlFullId参数，格式：SqlScope.SqlId");
            }
            Scope = sqlFullId.Substring(0, index);
            SqlId = sqlFullId.Substring(index + 1);
        }

        public RequestContext(string sqlFullId, object param, OpType type = OpType.exe) : this(sqlFullId)
        {
            Param = param;
            OpType = type;
        }

        public RequestContext(string scope, string sqlId)
        {
            Scope = scope;
            SqlId = sqlId;
        }

        public RequestContext(string scope, string sqlId, object param, OpType type = OpType.exe) : this(scope, sqlId)
        {
            Param = param;
            OpType = type;
        }
    }
}