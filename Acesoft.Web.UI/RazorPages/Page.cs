using System;

using Microsoft.AspNetCore.Mvc.Razor;

namespace Acesoft.Web.UI.RazorPages
{
	public abstract class Page<T> : RazorPage<T>, IWebPage
	{
        const string UI_Factory = "UI_Factory";

        public WidgetFactory Ace
        {
            get
            {
                return base.Context.GetOrAddScoped(
                    UI_Factory,
                    key => new WidgetFactory(this)
                );
            }
        }
    }
}
