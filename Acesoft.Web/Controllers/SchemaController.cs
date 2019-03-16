using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Platform;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class SchemaController : ApiControllerBase
	{
        private readonly ITableService tableService;

        public SchemaController(ITableService tableService)
        {
            this.tableService = tableService;
        }

		[HttpGet("{id}"), MultiAuthorize, Action("构建表")]
		public IActionResult CreateTable(string id)
		{
            tableService.CreateTable(id);

			SqlMapper.CacheManager.Flush("sys.table");
			SqlMapper.CacheManager.Flush("sys.field");

			return Ok(null);
		}

        [HttpGet("{id}"), MultiAuthorize, Action("撤销表")]
		public IActionResult DropTable(string id)
		{
            tableService.DropTable(id);

			SqlMapper.CacheManager.Flush("sys.table");
			SqlMapper.CacheManager.Flush("sys.field");

			return Ok(null);
		}

        [HttpGet("{id}"), MultiAuthorize, Action("构建字段")]
		public IActionResult CreateFields(string id, string fields)
		{
            tableService.CreateFields(id, fields.Split<long>(',').ToArray());

            SqlMapper.CacheManager.Flush("sys.field");

			return Ok(null);
		}

        [HttpGet("{id}"), MultiAuthorize, Action("撤销字段")]
		public IActionResult DropFields(string id, string fields)
		{
            tableService.DropFields(id, fields.Split<long>(',').ToArray());

            SqlMapper.CacheManager.Flush("sys.field");

			return Ok(null);
		}
	}
}
