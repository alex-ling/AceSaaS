namespace Acesoft.Web.UI.Widgets.Html
{
	public class MenuItemHtmlBuilder : TreeNodeHtmlBuilder<Menu, MenuItem>
	{
		public MenuItemHtmlBuilder(MenuItem component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.IconCls.HasValue())
			{
				base.Options["iconCls"] = base.Component.IconCls;
			}
			if (base.Component.Href.HasValue())
			{
				base.Options["href"] = base.Component.Href;
			}
			if (base.Component.Disabled.HasValue)
			{
				base.Options["disabled"] = base.Component.Disabled;
			}
		}
	}
}
