using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Cache;
using Acesoft.Data;
using Acesoft.Web.Portal.Entity;
using Acesoft.Web.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Acesoft.Web.Portal.Services
{
    public class ModuleService : Service<Port_Module>, IModuleService
    {
        private readonly ILogger<ModuleService> logger;
        private readonly IRazorViewEngine viewEngine;
        private readonly ITempDataProvider tempDataProvider;

        public ModuleService(ILogger<ModuleService> logger,
            IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            this.logger = logger;
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
        }

        public IEnumerable<Port_Module> QueryByPage(Port_Page page)
        {
            var results = Session.Query<Port_Module>(
                new RequestContext("sys", "get_module_by_page")
                .SetParam(new
                {
                    pageId = page.Id
                })
            );

            results.Each(module =>
            {
                module.Widget = Session.QueryFirst<Port_Widget>(
                    new RequestContext("sys", "get_widget_by_id")
                    .SetParam(new
                    {
                        id = module.Widget_Id
                    })
                );
                module.Configs = Session.Query<Port_ModuleConfig>(
                    new RequestContext("sys", "get_moduleconfig_by_module")
                    .SetParam(new
                    {
                        moduleId = module.Id
                    })
                ).ToDictionary(c => c.Name, c => (object)c.Value);
            });

            return results;
        }

        public Port_Module QueryById(long id)
        {
            return Session.QueryMultiple<Port_Module>(
                new RequestContext("sys", "query_module_by_id")
                .SetParam(new
                {
                    id
                }),
                reader =>
                {
                    var module = reader.Read<Port_Module>(true).SingleOrDefault();
                    if (module != null)
                    {
                        module.Widget = reader.Read<Port_Widget>(true).SingleOrDefault();
                        module.Configs = reader.Read<Port_ModuleConfig>(true).ToDictionary(c => c.Name, c => (object)c.Value);
                    }
                    return module;
                }
            );
        }

        public Port_ModuleConfig GetConfig(Port_Module module, string key)
        {
            return Session.QueryFirst<Port_ModuleConfig>(
                new RequestContext("sys", "get_moduleconfig_by_module_key")
                .SetParam(new
                {
                    moduleId = module.Id,
                    key
                })
            );
        }

        public Port_Module AddModule(Port_Page page, Port_Widget widget, string dockName)
        {
            var module = new Port_Module();
            module.InitializeId();
            module.DCreate = DateTime.Now;
            module.Cache = 0;
            module.DockName = dockName;
            module.DockOrder = GetMaxDockOrder(page, dockName) + 100;
            module.Page_Id = page.Id;
            module.Widget_Id = widget.Id;
            module.Title = widget.Name;
            module.Remark = widget.Remark;

            // 设置关联数据
            module.Widget = widget;
            module.Configs = new Dictionary<string, object>();
            Session.Insert(module);
            Session.FlushCache("sys.module");

            return module;
        }

        private int GetMaxDockOrder(Port_Page page, string dockName)
        {
            return Session.ExecuteScalar<int>(
                new RequestContext("sys", "get_moduleorder_by_dock")
                .SetParam(new
                {
                    pageid = page.Id,
                    dockName
                })
            );
        }

        public void Update(long id, string dockName, long? prevModId)
        {
            Session.Execute(
                new RequestContext("sys", "update_module_by_dockname")
                .SetParam(new
                {
                    id,
                    dockName,
                    prevModId
                })
            );
            Session.FlushCache("sys.module");
        }

        public void Delete(long id)
        {
            Session.Execute(
                new RequestContext("sys", "delete_module_by_id")
                .SetParam(new
                {
                    id
                })
            );
            Session.FlushCache("sys.module");
            Session.FlushCache("sys.moduleconfig");
        }

        public void SaveConfig(Port_Module module, IDictionary<string, object> data)
        {
            module.Icon = data.GetValue("mod_icon", "");
            module.Title = data.GetValue("mod_title", "");
            module.Remark = data.GetValue("mod_remark", "");
            module.Cache = data.GetValue("mod_cache", 0);
            module.Template = data.GetValue("mod_template", "");
            module.DUpdate = DateTime.Now;
            module.Configs = data;
            Session.Update(module);

            foreach (var p in data)
            {
                if (!p.Key.StartsWith("mod_"))
                {
                    var config = GetConfig(module, p.Key);
                    if (config == null)
                    {
                        config = new Port_ModuleConfig();
                        config.InitializeId();
                        config.Module_Id = module.Id;
                        config.Value = p.Value.ToString();
                        config.Name = p.Key;
                        Session.Insert(config);
                    }
                    else
                    {
                        config.Value = p.Value.ToString();
                        Session.Update(config);
                    }
                }
            }

            Session.FlushCache("sys.module");
            Session.FlushCache("sys.moduleconfig");
        }

        public async Task<string> LoadModule(WidgetFactory ace, Port_Module module, bool design)
        {
            if (ace == null)
            {
                var page = Session.Get<Port_Page>(module.Page_Id);
                ace = new WidgetFactory(page.Url);
            }

            var result = "";
            try
            {
                var httpContext = App.Context;
                var routeData = new RouteData();
                var descriptor = new ActionDescriptor();
                var actionContext = new ActionContext(httpContext, routeData, descriptor);
                var template = module.Template ?? "default.cshtml";

                using (var sw = new StringWriter())
                {
                    var viewResult = viewEngine.GetView("", $"{module.Widget.Path}/{template}", false);
                    if (viewResult.View == null)
                    {
                        throw new AceException($"指定的模板{template}不存在！");
                    }

                    var viewDictionary = new ViewDataDictionary(
                        new EmptyModelMetadataProvider(), 
                        new ModelStateDictionary())
                    {
                        Model = ace
                    };
                    foreach (var p in module.Props())
                    {
                        viewDictionary.Add(p.Key, p.Value);
                    }

                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        viewDictionary,
                        new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                        sw,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);
                    result = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"加载组件 {module.Id} 时出错！");
                result = $"<div>内容未加载：{ex.GetMessage()}</div>";
            }

            if (design)
            {
                result = $"<div id=\"{module.Id}\" class=\"portal-drag\">" +
                    $"<div class=\"portal-c\" title=\"{module.Title}\">{result}</div>" +
                    $"</div>";
            }
            return result;
        }
    }
}
