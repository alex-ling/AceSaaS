using System;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.UI.RazorPages
{
	public abstract class WebPageBase : Page, IWebPage
	{
		const string UI_Factory = "UI_Factory";

        public IApplicationContext AppCtx
        {
            get
            {
                return Ace.AppCtx;
            }
        }

		public WidgetFactory Ace
		{
			get
			{
                return HttpContext.GetOrAddScoped(
                    UI_Factory,
                    key => new WidgetFactory(this)
                );
			}
		}
	}
}
