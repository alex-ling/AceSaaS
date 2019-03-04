using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TabsHtmlBuilder : ListHtmlBuilder<Tabs, TabItem>
	{
		public TabsHtmlBuilder(Tabs component)
			: base(component, "div")
		{
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
			if (base.Component.Plain.HasValue)
			{
				base.Options["plain"] = base.Component.Plain;
			}
			if (base.Component.Fit.HasValue)
			{
				base.Options["fit"] = base.Component.Fit;
			}
			if (base.Component.Border.HasValue)
			{
				base.Options["border"] = base.Component.Border;
			}
			if (base.Component.ScrollIncrement.HasValue)
			{
				base.Options["scrollIncrement"] = base.Component.ScrollIncrement;
			}
			if (base.Component.ScrollDuration.HasValue)
			{
				base.Options["scrollDuration"] = base.Component.ScrollDuration;
			}
			if (base.Component.Tools.Any())
			{
				base.Options["tools"] = base.Component.Tools;
			}
			if (base.Component.ToolPosition.HasValue)
			{
				base.Options["soolPosition"] = base.Component.ToolPosition;
			}
			if (base.Component.TabPosition.HasValue)
			{
				base.Options["tabPosition"] = base.Component.TabPosition;
			}
			if (base.Component.HeaderWidth.HasValue)
			{
				base.Options["headerWidth"] = base.Component.HeaderWidth;
			}
			if (base.Component.TabWidth.HasValue)
			{
				base.Options["tabWidth"] = base.Component.TabWidth;
			}
			if (base.Component.TabHeight.HasValue)
			{
				base.Options["tabHeight"] = base.Component.TabHeight;
			}
			if (base.Component.Selected.HasValue)
			{
				base.Options["selected"] = base.Component.Selected;
			}
			if (base.Component.ShowHeader.HasValue)
			{
				base.Options["showHeader"] = base.Component.ShowHeader;
			}
			if (base.Component.Justified.HasValue)
			{
				base.Options["justified"] = base.Component.Justified;
			}
			if (base.Component.Narrow.HasValue)
			{
				base.Options["narrow"] = base.Component.Narrow;
			}
			if (base.Component.Pill.HasValue)
			{
				base.Options["pill"] = base.Component.Pill;
			}
		}
	}
}
