using System;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Acesoft.Util;
using Acesoft.Platform;
using Acesoft.Web.Razor;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class CodeController : ApiControllerBase
	{
        private ITableService tableService;

        public CodeController(ITableService tableService)
        {
            this.tableService = tableService;
        }

        [HttpPost, MultiAuthorize, Action("�����ļ�")]
        public IActionResult CreateCode([FromBody]JObject data)
        {
            var tableName = data["table"].Value<string>();
            var tempPath = data["temp"].Value<string>();
            var path = data["path"].Value<string>();

            var table = tableService.Query(tableName);
            Check.Require(table != null, $"���ݱ�{tableName}�����ڣ�");

            var tempDir = new DirectoryInfo(App.GetLocalPath(tempPath));
            Check.Require(tempDir.Exists, $"ģ��Ŀ¼{tempPath}�����ڣ�");

            var newDir = new DirectoryInfo(App.GetLocalPath(path));
            if (!newDir.Exists) newDir.Create();

            foreach (var file in tempDir.GetFiles())
            {
                var content = RazorHelper.Generate(file.Read(), table);
                FileHelper.Write(Path.Combine(newDir.FullName, file.Name), content);
            }

            return Ok(null);
        }
    }
}
