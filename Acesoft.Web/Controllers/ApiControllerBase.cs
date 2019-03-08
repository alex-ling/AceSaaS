using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Acesoft.Data.SqlMapper;

namespace Acesoft.Web.Controllers
{
    public abstract class ApiControllerBase : Controller
    {
        public IApplicationContext AppCtx { get; private set; }
        public ISqlMapper SqlMapper { get; private set; }
        public string SqlScope { get; private set; }
        public string SqlId { get; private set; }
        public SqlMap SqlMap { get; private set; }

        public ApiControllerBase()
        {
            AppCtx = HttpContext.RequestServices.GetService<IApplicationContext>();
            SqlMapper = MapperContainer.Instance.GetSqlMapper(AppCtx.Session);
        }

        protected void CheckDataSourceParameter()
        {
            Check.Require(SqlScope.HasValue() && SqlId.HasValue(), $"请求中必须附带ds参数");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ds = App.GetQuery("ds", "");
            if (ds.HasValue())
            {
                var dsItems = ds.Split('.');
                Check.Require(dsItems.Length == 2, "ds参数必须以“.”分隔：SqlScope.SqlId");

                SqlScope = dsItems[0];
                SqlId = dsItems[1];
                SqlMap = SqlMapper.GetSqlMap(SqlScope, SqlId);
            }

            base.OnActionExecuting(context);
        }
    }
}
