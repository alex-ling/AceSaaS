using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ListBoxHtmlBuilder<Widget> : HiddenBoxHtmlBuilder<Widget> where Widget : ListBox
	{
		public ListBoxHtmlBuilder(Widget component)
			: base(component)
		{
            this.RenderType = "aceui";
        }

        protected override void PreBuild()
        {
            base.PreBuild();

            if (base.Component.SelectUrl.HasValue())
            {
                base.Options["url"] = Component.SelectUrl;
            }
            if (base.Component.Width.HasValue)
            {
                base.Options["width"] = Component.Width;
            }
            if (base.Component.Height.HasValue)
            {
                base.Options["height"] = Component.Height;
            }
            if (base.Component.Prompt.HasValue())
            {
                base.Options["prompt"] = Component.Prompt;
            }
        }
    }
}
