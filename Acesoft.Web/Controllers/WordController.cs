using System;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Platform.Office;
using Newtonsoft.Json.Linq;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class WordController : ApiControllerBase
	{
		[HttpPost, MultiAuthorize, Action("导出Word")]
		public IActionResult Down()
		{
			CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId)
                .SetExtraParam(AppCtx.AC.Params);
            var result = AppCtx.Session.QueryMultiple(ctx);

            var path = "/pages" + App.GetQuery("path", "");
			var temp = SqlMap.Params.GetValue("ex_tempfile", "temp.docx");
            temp = temp.Replace("{tanent}", AppCtx.TenantContext.Tenant.Name);

			var fileName = SqlMap.Params.GetValue("ex_filename", "down");
            // 数据源必须配置
            var props = SqlMap.Params.GetValue("bookmarks_dataset").ToDictionary("dataset");
            var props_parent = SqlMap.Params.GetValue("bookmarks_parent", "");
            if (props_parent.HasValue())
            {
                // 父键配置
                props.Merge(props_parent.ToDictionary("parent"));
            }
            var props_mergecols = SqlMap.Params.GetValue("bookmarks_mergecols", "");
            if (props_mergecols.HasValue())
            {
                // 合并列配置
                props.Merge(props_mergecols.ToDictionary("mergecols"));
            }
            var xls = new DocReport(App.GetLocalPath(path + temp), result, props);

			fileName = App.ReplaceQuery(fileName) + "_" + DateTime.Now.ToYMD() + ".docx";
			return File(xls.Export(), "application/vnd.ms-word", fileName);
		}
	}
}
