namespace Acesoft.Web.UI.Widgets.Html
{
	public class MenuButtonHtmlBuilder : SplitButtonHtmlBuilder<MenuButton>
	{
		public MenuButtonHtmlBuilder(MenuButton component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.MenuAlign.HasValue)
			{
				base.Options["menuAlign"] = base.Component.MenuAlign;
			}
			if (base.Component.HasDownArrow.HasValue)
			{
				base.Options["hasDownArrow"] = base.Component.HasDownArrow;
			}
		}
	}
}
