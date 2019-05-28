using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Cache;
using Acesoft.Data;
using Acesoft.Data.SqlMapper;
using Acesoft.Platform.Models;

namespace Acesoft.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "PLAT")]
    [Route("api/[controller]/[action]")]
    public class CrudController : ApiControllerBase
    {
        [HttpPost, MultiAuthorize, DataSource, Action("新增数据")]
        public async Task<IActionResult> Post([FromBody]JObject data)
        {
            CheckDataSourceParameter();

            var param = data.ToDictionary();
            var id = param.GetValue("id", App.IdWorker.NextId());
            param["id"] = id;

            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.insert)
                .SetParam(param)
                .SetExtraParam(AppCtx.AC.Params);
            var result = await AppCtx.Session.ExecuteAsync(ctx);
            if (result > 0)
            {
                var value = SqlMap.Params.GetValue("savetoredis", "");
                if (value.HasValue())
                {
                    var array = value.Split('=');
                    var key = array[0].Replace(ctx.DapperParams);
                    var val = array[1].Replace(ctx.DapperParams);
                    App.Cache.SetString(key, val, null);
                }
            }

            return Ok(result);
        }

        [HttpPut, MultiAuthorize, DataSource, Action("编辑数据")]
        public async Task<IActionResult> Put([FromBody]JObject data)
        {
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.update)
                .SetParam(data.ToDictionary())
                .SetExtraParam(AppCtx.AC.Params);
            var result = await AppCtx.Session.ExecuteAsync(ctx);

            return Ok(result);
        }

        [HttpDelete, MultiAuthorize, DataSource, Action("删除数据")]
        public async Task<IActionResult> Delete(string id)
        {
            var ids = id.Split<string>(',');
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.delete)
                .SetParam(new { ids, id })
                .SetExtraParam(AppCtx.AC.Params);
            var result = await AppCtx.Session.ExecuteAsync(ctx);

            return Ok(result);
        }

        [HttpDelete, MultiAuthorize, DataSource, Action("删除数据")]
        public async Task<IActionResult> DeleteIds(string id)
        {
            var ids = id.Split<string>(',');
            var count = 0;
            foreach (var itemId in ids)
            {
                var ctx = new RequestContext(SqlScope, SqlId)
                    .SetCmdType(CmdType.delete)
                    .SetParam(new { id = itemId })
                    .SetExtraParam(AppCtx.AC.Params);
                count += await AppCtx.Session.ExecuteAsync(ctx);
            }

            return Ok(count);
        }

        [HttpGet, MultiAuthorize, DataSource, Action("获取数据")]
        public IActionResult Get(long id)
        {
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.select)
                .SetParam(new { id })
                .SetExtraParam(AppCtx.AC.Params);
            var result = AppCtx.Session.QueryFirst(ctx);

            return Ok(result);
        }

        [HttpGet, MultiAuthorize, DataSource, Action("查询Grid")]
        public IActionResult Grid([FromQuery]GridRequest request)
        {
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.query)
                .SetParam(request)
                .SetExtraParam(AppCtx.AC.Params);
            var grid = AppCtx.Session.QueryPageTable(ctx, request);

            return Json(grid);
        }

        [HttpGet, MultiAuthorize, DataSource, Action("查询List")]
        public IActionResult List()
        {
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.query)
                .SetExtraParam(AppCtx.AC.Params);
            var list = AppCtx.Session.Query<DictItem>(ctx).ToList();
            if (App.GetQuery("nullselect", false))
            {
                list.Insert(0, new DictItem("", "\u3000"));
            }

            return Json(list);
        }

        [HttpGet, MultiAuthorize, DataSource, Action("查询Tree")]
        public IActionResult Tree([FromQuery]TreeRequest request)
        {
            var ctx = new RequestContext(SqlScope, SqlId)
                .SetCmdType(CmdType.query)
                .SetParam(request)
                .SetExtraParam(AppCtx.AC.Params);
            var tree = AppCtx.Session.QueryDataTable(ctx);

            return Json(new TreeResponse
            {
                Request = request,
                Data = tree
            });
        }
    }
}
