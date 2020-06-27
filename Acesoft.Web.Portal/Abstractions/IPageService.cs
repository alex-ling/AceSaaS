using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Web.Portal.Entity;
using Acesoft.Web.UI;

namespace Acesoft.Web.Portal
{
    public interface IPageService : IService<Port_Page>
    {
        Port_Page GetByUrl(string url);
        int UpdateLayout(long pageId, string layout);

        Task<string> RenderHtml(WidgetFactory ace, Port_Page page, bool renderMode = true, bool design = false);
    }
}