namespace Acesoft.Web.UI.Widgets.Html
{
	public class MenuHtmlBuilder : TreeHtmlBuilder<Menu, MenuItem>
	{
		public MenuHtmlBuilder(Menu component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.ZIndex.HasValue)
			{
				base.Options["zIndex"] = base.Component.ZIndex;
			}
			if (base.Component.Left.HasValue)
			{
				base.Options["left"] = base.Component.Left;
			}
			if (base.Component.Top.HasValue)
			{
				base.Options["top"] = base.Component.Top;
			}
			if (base.Component.Align.HasValue)
			{
				base.Options["align"] = base.Component.Align;
			}
			if (base.Component.MinWidth.HasValue)
			{
				base.Options["minWidth"] = base.Component.MinWidth;
			}
			if (base.Component.ItemHeight.HasValue)
			{
				base.Options["itemHeight"] = base.Component.ItemHeight;
			}
			if (base.Component.Duration.HasValue)
			{
				base.Options["duration"] = base.Component.Duration;
			}
			if (base.Component.HideOnUnhover.HasValue)
			{
				base.Options["hideOnUnhover"] = base.Component.HideOnUnhover;
			}
			if (base.Component.Inline.HasValue)
			{
				base.Options["inline"] = base.Component.Inline;
			}
			if (base.Component.Fit.HasValue)
			{
				base.Options["fit"] = base.Component.Fit;
			}
		}
	}
}
