using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Acesoft.Web.Mvc;
using Acesoft.Data;
using Acesoft.Platform.Entity;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class ClientController : ApiControllerBase
	{
		private readonly ILogger<ClientController> logger;

		public ClientController(ILogger<ClientController> logger)
		{
			this.logger = logger;
		}

		[HttpGet, Action("获取APP")]
		public IActionResult GetVersion(long clientId)
		{
			var ctx = new RequestContext("sys", "get_app_client")
                .SetParam(new
			    {
				    id = clientId
			    });
			var client = AppCtx.Session.QueryFirst<App_Version>(ctx);

			return Ok(client);
		}
	}
}
