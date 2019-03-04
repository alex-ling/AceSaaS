using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class HiddenBoxHtmlBuilder<Widget> : EasyUIHtmlBuilder<Widget> where Widget : HiddenBox
	{
		public HiddenBoxHtmlBuilder(Widget component)
			: base(component, "input")
		{
			base.EventsToOption = false;
			base.RenderMode = TagRenderMode.SelfClosing;
		}

		public override IHtmlNode Build()
		{
			IHtmlNode htmlNode = base.Build().Attribute("type", base.Component.Type.ToString(), true);
			if (base.Component.Value.HasValue())
			{
				htmlNode.Attribute("value", base.Component.Value, true);
			}
			return htmlNode;
		}
	}
}
