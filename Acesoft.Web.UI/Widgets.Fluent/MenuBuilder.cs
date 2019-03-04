using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MenuBuilder : TreeBuilder<Menu, MenuItem, MenuBuilder>
	{
		public MenuBuilder(Menu component)
			: base(component)
		{
		}

		public virtual MenuBuilder ZIndex(int zIndex)
		{
			base.Component.ZIndex = zIndex;
			return this;
		}

		public virtual MenuBuilder Left(int left)
		{
			base.Component.Left = left;
			return this;
		}

		public virtual MenuBuilder Top(int top)
		{
			base.Component.Top = top;
			return this;
		}

		public virtual MenuBuilder Align(Align align)
		{
			base.Component.Align = align;
			return this;
		}

		public virtual MenuBuilder MinWidth(int minWidth)
		{
			base.Component.MinWidth = minWidth;
			return this;
		}

		public virtual MenuBuilder ItemHeight(int itemHeight)
		{
			base.Component.ItemHeight = itemHeight;
			return this;
		}

		public virtual MenuBuilder Duration(int duration)
		{
			base.Component.Duration = duration;
			return this;
		}

		public virtual MenuBuilder HideOnUnhover(bool hideOnUnhover = true)
		{
			base.Component.HideOnUnhover = hideOnUnhover;
			return this;
		}

		public virtual MenuBuilder Inline(bool inline = true)
		{
			base.Component.Inline = inline;
			return this;
		}

		public virtual MenuBuilder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this;
		}

		public MenuBuilder Items(Action<ItemsBuilder<MenuItem, MenuItemBuilder>> addAction)
		{
			return base.Nodes(addAction, () => new MenuItem(base.Component.Ace, base.Component), (MenuItem item) => new MenuItemBuilder(item));
		}

		public MenuBuilder Events(Action<MenuEventBuilder> clientEventsAction)
		{
			clientEventsAction(new MenuEventBuilder(base.Component.Events));
			return this;
		}
	}
}
