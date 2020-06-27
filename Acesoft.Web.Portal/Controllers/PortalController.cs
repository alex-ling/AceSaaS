using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Data;
using Acesoft.Config;
using Acesoft.Web.Portal.Config;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Acesoft.Web.Portal.Controllers
{
    [ApiExplorerSettings(GroupName = "PLAT")]
    [Route("api/[controller]/[action]")]
    public class PortalController : ApiControllerBase
    {
        private readonly IPageService pageService;
        private readonly IWidgetService widgetService;
        private readonly IModuleService moduleService;

        public PortalController(
            IPageService pageService,
            IWidgetService widgetService,
            IModuleService moduleService)
        {
            this.pageService = pageService;
            this.widgetService = widgetService;
            this.moduleService = moduleService;
        }

        #region widget
        [HttpGet, MultiAuthorize, Action("获取目录")]
        public IActionResult RegistWidgets(string path)
        {
            var dir = new DirectoryInfo(App.GetLocalPath(path));
            LoadWidgets(path, dir.GetDirectories());

            return Ok();
        }

        private void LoadWidgets(string parentPath, DirectoryInfo[] dirs)
        {
            foreach (var subDir in dirs)
            {
                var path = $"{parentPath}/{subDir.Name}";
                var widgetConfig = ConfigContext.GetJsonConfig<WidgetConfig>(opts =>
                {
                    opts.Optional = true;
                    opts.ConfigPath = path;
                    opts.ConfigFile = "widget.config.json";
                });

                if (widgetConfig.Name.HasValue())
                {
                    widgetService.Regist(path, parentPath.Split('/').Last(), widgetConfig);
                }
                else
                {
                    LoadWidgets(path, subDir.GetDirectories());
                }
            }
        }
        #endregion

        #region module
        [HttpGet, MultiAuthorize, Action("获取模块Html")]
        public async Task<IActionResult> GetModule(long modId)
        {
            var module = moduleService.QueryById(modId);
            return Ok(new
            {
                title = module.Title,
                html = await moduleService.LoadModule(null, module, true)
            });
        }

        [HttpPost, MultiAuthorize, Action("添加模块实例")]
        public async Task<IActionResult> AddModule(long pageId, [FromBody]JObject data)
        {
            var page = pageService.Get(pageId);
            var widget = widgetService.Get(data.GetValue<long>("widget"));
            var dockName = data.GetValue<string>("dock");
            var module = moduleService.AddModule(page, widget, dockName);
            return Ok(new
            {
                title = module.Title,
                html = await moduleService.LoadModule(null, module, true)
            });
        }

        [HttpPost, MultiAuthorize, Action("更新模块配置")]
        public async Task<IActionResult> PostModule(long modId, [FromBody]JObject data)
        {
            var module = moduleService.QueryById(modId);
            moduleService.SaveConfig(module, data.ToDictionary());
            return Ok(new
            {
                title = module.Title,
                html = await moduleService.LoadModule(null, module, true)
            });
        }

        [HttpPut, MultiAuthorize, Action("更新模块位置")]
        public IActionResult PutModule(long modId, [FromBody]JObject data)
        {
            var prevModuleId = data.GetValue<long>("modid", 0);
            var dockName = data.GetValue<string>("dock");
            moduleService.Update(modId, dockName, prevModuleId);
            return Ok();
        }

        [HttpDelete, MultiAuthorize, Action("删除模块")]
        public IActionResult DeleteModule(long modId)
        {
            moduleService.Delete(modId);
            return Ok();
        }
        #endregion

        #region page
        [HttpPut, MultiAuthorize, Action("获取模块Html")]
        public IActionResult PutPage(long pageId, [FromBody]JObject data)
        {
            var layout = data.GetValue("content", "");
            var result = pageService.UpdateLayout(pageId, layout);

            return Ok(result);
        }
        #endregion
    }
}
