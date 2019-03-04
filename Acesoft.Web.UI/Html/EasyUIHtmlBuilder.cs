using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Html
{
	public abstract class EasyUIHtmlBuilder<Widget> : WidgetHtmlBuilder<Widget> where Widget : WidgetBase
	{
		public EasyUIHtmlBuilder(Widget component, string tagName)
			: this(component, tagName, TagRenderMode.Normal)
		{
		}

		public EasyUIHtmlBuilder(Widget component, string tagName, TagRenderMode renderMode)
			: base(component, tagName, renderMode)
		{
			base.RenderType = "easyui";
		}

		public override IHtmlNode Build()
		{
			IHtmlNode htmlNode = base.Build();
			string text = BuildJson();
			if (text.HasValue())
			{
				htmlNode.Attribute("data-options", text, true);
			}
			return htmlNode;
		}

		public string BuildJson()
		{
			return base.Component.Serializer.Serialize(BuildOptions(), true);
		}
	}
}
