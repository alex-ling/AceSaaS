using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class CheckBoxHtmlBuilder : WidgetHtmlBuilder<CheckBox>
	{
		public CheckBoxHtmlBuilder(CheckBox component)
			: base(component, "input")
		{
			base.EventsToOption = false;
			base.RenderMode = TagRenderMode.SelfClosing;
		}

		public override IHtmlNode Build()
		{
			IHtmlNode htmlNode = base.Build().Attribute("type", base.Component.Type.ToString(), true);
			if (base.Component.Group.HasValue())
			{
				htmlNode.Attribute("name", base.Component.Group, true);
			}
			if (base.Component.Value.HasValue())
			{
				htmlNode.Attribute("value", base.Component.Value, true);
			}
			if (base.Component.Checked)
			{
				htmlNode.Attribute("checked", "true", true);
			}
			if (base.Component.Text.HasValue())
			{
				new HtmlNode("label").Attribute("for", base.Component.Id, true).Text(base.Component.Text).AppendTo(htmlNode.NextSibings);
			}
			return htmlNode;
		}
	}
}
