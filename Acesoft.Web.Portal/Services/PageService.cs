using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Acesoft.Cache;
using Acesoft.Data;
using Acesoft.Util;
using Acesoft.Web.Portal.Entity;
using Acesoft.Web.UI;

namespace Acesoft.Web.Portal.Services
{
    public class PageService : Service<Port_Page>, IPageService
    {
        private const string REG_Portal_Container = "<div[^>]+id=\"p\\d+\"[^>]*class=\"portal-container\"[^>]*>((?>(?<o><div[^>]*>)|(?<-o></div>)|(?:(?!</?div)[\\s\\S]))*)(?(o)(?!))</div>";

        private readonly IModuleService moduleService;

        public PageService(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }

        public Port_Page GetByUrl(string url)
        {
            Port_Page page = null;

            while (page == null)
            {
                page = Session.QueryFirst<Port_Page>(
                    new RequestContext("sys", "get_page_by_url")
                    .SetParam(new
                    {
                        url
                    })
                );

                var ix = url.LastIndexOf('/');
                if (ix > 0)
                {
                    url = url.Substring(0, ix);
                }
                else
                {
                    break;
                }
            }

            return page;
        }

        public int UpdateLayout(long pageId, string layout)
        {
            var sql = "update port_page set layout=@layout where id=@pageid";
            var result = Session.Execute(sql, new 
            {
                pageId,
                layout
            });

            if (result > 0)
            {
                Session.FlushCache("sys.page");
            }
            return result;
        }

        public async Task<string> RenderHtml(WidgetFactory ace, Port_Page page, bool renderMode = true, bool design = false)
        {
            var layout = page.Layout ?? "";
            if (design && renderMode)
            {
                layout = layout.Replace("class=\"container\"", "class=\"container-fluid\"");
            }

            if (renderMode)
            {
                var modules = moduleService.QueryByPage(page);
                foreach (Match item in RegexHelper.GetMatchValues(layout, REG_Portal_Container))
                {
                    var dock = RegexHelper.GetMatchValue(item.Value, "(?<=id=\")\\w+(?=\")");
                    var html = item.Value.Substring(0, item.Value.Length - 6);
                    foreach (var module in modules.Where(m => m.DockName == dock))
                    {
                        if (module.Cache > 0 && !design)
                        {
                            var cacheKey = $"mod_{module.Id}";
                            var result = App.Cache.GetOrAdd(cacheKey, key =>
                            {
                                return moduleService.LoadModule(ace, module, design).Result;
                            }, option =>
                            {
                                option.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(module.Cache);
                            });
                            html += result;
                        }
                        else
                        {
                            html += await moduleService.LoadModule(ace, module, design);
                        }
                    }
                    html += "</div>";
                    layout = layout.Replace(item.Value, html);
                }
            }

            if (design)
            {
                return $"<div class=\"portal-layout\">{layout}</div>";
            }
            return layout;
        }
    }
}
