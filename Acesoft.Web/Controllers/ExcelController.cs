using System;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Platform.Office;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class ExcelController : ApiControllerBase
	{
		[HttpPost, MultiAuthorize, Action("导出Excel")]
		public IActionResult Down([FromQuery] GridRequest request)
		{
			CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.query)
                .SetParam(request)
                .SetExtraParam(AppCtx.AC.Params);
            var res = AppCtx.Session.QueryPageTable(ctx, request);

            var path = "/pages" + App.GetQuery("path", "");
			var temp = SqlMap.Params.GetValue("ex_tempfile", "temp.xlsx");
			var fileName = SqlMap.Params.GetValue("ex_filename", "down");
            var xls = new XlsExport(res, App.GetLocalPath(path + temp));

			fileName = fileName + "_" + DateTime.Now.ToYMD() + ".xlsx";
			return File(xls.Export(), "application/vnd.ms-excel", fileName);
		}
	}
}
