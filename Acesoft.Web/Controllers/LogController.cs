using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "PLAT")]
    [Route("api/[controller]/[action]")]
    public class LogController : ApiControllerBase
    {
        [MultiAuthorize, Action("获取日志")]
        public IActionResult Grid()
        {
            var result = new List<object>();

            var folder = App.GetLocalPath("logs");
            foreach (var file in Directory.GetFiles(folder))
            {
                var fi = new FileInfo(file);

                result.Insert(0, new
                {
                    Id = fi.Name,
                    Date = fi.LastWriteTime,
                    Size = fi.Length,
                    Url = App.GetWebPath($"logs/{fi.Name}"),
                    Action = "del_remove=删除"
                });
            }

            return Json(result);
        }

        public IActionResult Delete(string id)
        {
            id.Split<string>().Each(file =>
            {
                var path = App.GetLocalPath($"logs/{file}");
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            });

            return Ok();
        }
    }
}
