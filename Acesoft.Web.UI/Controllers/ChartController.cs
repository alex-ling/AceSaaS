using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Data;

namespace Acesoft.Web.UI.Controllers
{
	[ApiExplorerSettings(GroupName = "WebUI")]
	[Route("api/[controller]/[action]")]
	public class ChartController : ApiControllerBase
	{
		private readonly ILogger<ChartController> logger;

		public ChartController(ILogger<ChartController> logger)
		{
			this.logger = logger;
		}

		[HttpGet, MultiAuthorize, DataSource, Action("获取图表")]
		public IActionResult Get()
		{
			CheckDataSourceParameter();

			var ctx = new RequestContext(SqlScope, SqlId)
                .SetExtraParam(AppCtx.AC.Params);
			var result = AppCtx.Session.Query(ctx);

			return Ok(result);
		}
	}
}
