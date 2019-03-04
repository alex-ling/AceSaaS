using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MenuItemBuilder : TreeNodeBuilder<Menu, MenuItem, MenuItemBuilder>
	{
		public MenuItemBuilder(MenuItem component)
			: base(component)
		{
		}

		public virtual MenuItemBuilder IconCls(string iconCls)
		{
			base.Component.IconCls = iconCls;
			return this;
		}

		public virtual MenuItemBuilder Href(string href)
		{
			base.Component.Href = href;
			return this;
		}

		public new virtual MenuItemBuilder Disabled(bool disabled = true)
		{
			base.Component.Disabled = disabled;
			return this;
		}

		public MenuItemBuilder Separtor()
		{
			return Css("menu-sep");
		}

		public MenuItemBuilder Items(Action<ItemsBuilder<MenuItem, MenuItemBuilder>> addAction)
		{
			return base.Nodes(addAction, () => new MenuItem(base.Component.Ace, base.Component.Tree), (MenuItem item) => new MenuItemBuilder(item));
		}

		public MenuItemBuilder Events(Action<ButtonEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ButtonEventBuilder(base.Component.Events));
			return this;
		}
	}
}
