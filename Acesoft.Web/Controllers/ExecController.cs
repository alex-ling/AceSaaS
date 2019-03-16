using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class ExecController : ApiControllerBase
	{
		[HttpPost, MultiAuthorize, DataSource, Action("执行语句")]
		public async Task<IActionResult> ExecSql([FromBody] JObject data)
		{
			CheckDataSourceParameter();

			var ctx = new RequestContext(SqlScope, SqlId).SetParam(data.ToDictionary());
            var result = await AppCtx.Session.ExecuteAsync(ctx);

            return Ok(result);
		}

        [HttpPost, MultiAuthorize, DataSource, Action("执行语句")]
        public async Task<IActionResult> ExecId([FromBody] JObject data)
		{
            CheckDataSourceParameter();

            var param = data.ToDictionary();
            Check.Assert(param.ContainsKey("id"), "未提交ID参数");
            param["ids"] = param["id"].ToString().Split<long>();

            var ctx = new RequestContext(SqlScope, SqlId).SetParam(param);
            var result = await AppCtx.Session.ExecuteAsync(ctx);

            return Ok(result);
        }

        [HttpPost, MultiAuthorize, DataSource, Action("执行语句")]
        public async Task<IActionResult> ExecIds([FromBody] JObject data)
		{
            CheckDataSourceParameter();

            var param = data.ToDictionary();
            Check.Assert(param.ContainsKey("ids"), "未提交ID参数");
            var ids = param["id"].ToString().Split<string>(',');

            foreach (var id in ids)
            {
                param["id"] = id;
                var ctx = new RequestContext(SqlScope, SqlId).SetParam(param);
                await AppCtx.Session.ExecuteAsync(ctx);
            }

			return Ok(null);
		}

        [HttpPost, MultiAuthorize, DataSource, Action("执行语句")]
        public async Task<IActionResult> ExecObj([FromBody] JObject data)
		{
            CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId).SetParam(data.ToDictionary());
            var result = await AppCtx.Session.ExecuteScalarAsync(ctx);

            return Ok(result);
		}

        [HttpPost, MultiAuthorize, DataSource, Action("获取字典")]
		public IActionResult ExecObjs([FromBody] JObject data)
		{
            CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId).SetParam(data.ToDictionary());
            var result = AppCtx.Session.Query(ctx).ToDictionary(item => item.id);

			return Ok(result);
		}

        [HttpPost, MultiAuthorize, DataSource, Action("获取列表")]
		public IActionResult ExecList([FromBody] JObject data)
        {
            CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId).SetParam(data.ToDictionary());
            var result = AppCtx.Session.Query(ctx).Select(item => item.id);

			return Ok(result);
		}

        [HttpPost, MultiAuthorize, DataSource, Action("获取查询")]
		public IActionResult ExecQuery([FromBody] JObject data)
        {
            CheckDataSourceParameter();

            var ctx = new RequestContext(SqlScope, SqlId).SetParam(data.ToDictionary());
            var result = AppCtx.Session.Query(ctx);

			return Ok(result);
		}
	}
}
