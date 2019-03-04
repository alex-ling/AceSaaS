namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MenuButtonBuilder : SplitButtonBuilder<MenuButton, MenuButtonBuilder>
	{
		public MenuButtonBuilder(MenuButton component)
			: base(component)
		{
		}

		public virtual MenuButtonBuilder MenuAlign(Align align)
		{
			base.Component.MenuAlign = align;
			return this;
		}

		public virtual MenuButtonBuilder HasDownArrow(bool hasDownArrow = true)
		{
			base.Component.HasDownArrow = hasDownArrow;
			return this;
		}
	}
}
