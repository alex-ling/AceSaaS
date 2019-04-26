using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "APP")]
    [Route("api/[controller]/[action]")]
    public class AppController : ApiControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IObjectService objectService;

        public AppController(ILogger<AuthController> logger, 
            IObjectService objectService)
        {
            this.logger = logger;
            this.objectService = objectService;
        }

        [HttpGet, Action("获取Token")]
        public async Task<IActionResult> GetToken(string userName, string password)
        {
            return Ok(await AppCtx.AC.GetToken(userName, password));
        }

        [HttpPost, Action("获取Token")]
        public async Task<IActionResult> Token([FromBody]JObject data)
        {
            var userName = data.GetValue<string>("username");
            var password = data.GetValue<string>("password");
            return Ok(await AppCtx.AC.GetToken(userName, password));
        }

        [HttpGet, MultiAuthorize, Action("获取模块")]
        public IActionResult GetObjects()
        {
            var roles = AppCtx.AC.Roles;
            var res = objectService.Gets(roles, ObjectType.App, AppCtx.AC.User.LoginName);
            return Ok(res);
        }
    }
}