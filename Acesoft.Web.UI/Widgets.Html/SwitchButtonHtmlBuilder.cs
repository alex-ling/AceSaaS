using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class SwitchButtonHtmlBuilder : EasyUIHtmlBuilder<SwitchButton>
	{
		public SwitchButtonHtmlBuilder(SwitchButton component)
			: base(component, "input")
		{
			base.RenderMode = TagRenderMode.SelfClosing;
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Width.HasValue)
			{
				base.Options["width"] = base.Component.Width;
			}
			if (base.Component.Height.HasValue)
			{
				base.Options["height"] = base.Component.Height;
			}
			if (base.Component.HandleWidth.HasValue)
			{
				base.Options["handleWidth"] = base.Component.HandleWidth;
			}
			if (base.Component.Checked.HasValue)
			{
				base.Options["checked"] = base.Component.Checked;
			}
			if (base.Component.Disabled.HasValue)
			{
				base.Options["disabled"] = base.Component.Disabled;
			}
			if (base.Component.Readonly.HasValue)
			{
				base.Options["readonly"] = base.Component.Readonly;
			}
			if (base.Component.Reversed.HasValue)
			{
				base.Options["reversed"] = base.Component.Reversed;
			}
			if (base.Component.OnText.HasValue())
			{
				base.Options["onText"] = base.Component.OnText;
			}
			if (base.Component.OffText.HasValue())
			{
				base.Options["offText"] = base.Component.OffText;
			}
			if (base.Component.HandleText.HasValue())
			{
				base.Options["handleText"] = base.Component.HandleText;
			}
			if (base.Component.Value.HasValue())
			{
				base.Options["value"] = base.Component.Value;
			}
		}
	}
}
