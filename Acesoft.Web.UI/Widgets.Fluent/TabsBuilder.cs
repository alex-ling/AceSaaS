using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TabsBuilder : ListBuilder<Tabs, TabsBuilder, TabItem, TabItemBuilder>
	{
		public TabsBuilder(Tabs component) : base(component)
		{
		}

		public virtual TabsBuilder Width(int width)
		{
			base.Component.Width = width;
			return this;
		}

		public virtual TabsBuilder Plain(bool plain = true)
		{
			base.Component.Plain = plain;
			return this;
		}

		public virtual TabsBuilder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this;
		}

		public virtual TabsBuilder Border(bool border = true)
		{
			base.Component.Border = border;
			return this;
		}

		public virtual TabsBuilder ScrollIncrement(int scrollIncrement)
		{
			base.Component.ScrollIncrement = scrollIncrement;
			return this;
		}

		public virtual TabsBuilder ScrollDuration(int scrollDuration)
		{
			base.Component.ScrollDuration = scrollDuration;
			return this;
		}

		public virtual TabsBuilder Tools(Action<IList<LinkButton>> toolsAction)
		{
			toolsAction(base.Component.Tools);
			return this;
		}

		public virtual TabsBuilder ToolPosition(Position position)
		{
			base.Component.ToolPosition = position;
			return this;
		}

		public virtual TabsBuilder TabPosition(Position position)
		{
			base.Component.TabPosition = position;
			return this;
		}

		public virtual TabsBuilder HeaderWidth(int headerWidth)
		{
			base.Component.HeaderWidth = headerWidth;
			return this;
		}

		public virtual TabsBuilder TabWidth(int tabWidth)
		{
			base.Component.TabWidth = tabWidth;
			return this;
		}

		public virtual TabsBuilder TabHeight(int tabHeight)
		{
			base.Component.TabHeight = tabHeight;
			return this;
		}

		public virtual TabsBuilder Selected(int selected)
		{
			base.Component.Selected = selected;
			return this;
		}

		public virtual TabsBuilder ShowHeader(bool showHeader = true)
		{
			base.Component.ShowHeader = showHeader;
			return this;
		}

		public virtual TabsBuilder Justified(bool justified = true)
		{
			base.Component.Justified = justified;
			return this;
		}

		public virtual TabsBuilder Narrow(bool narrow = true)
		{
			base.Component.Narrow = narrow;
			return this;
		}

		public virtual TabsBuilder Pill(bool pill = true)
		{
			base.Component.Pill = pill;
			return this;
		}

		public TabsBuilder Items(Action<ItemsBuilder<TabItem, TabItemBuilder>> addAction)
		{
			return Items(addAction, () => new TabItem(Component.Ace), item => new TabItemBuilder(item));
		}

        public TabsBuilder ItemBind<Model>(Action<TabItemBuilder, Model> bindItem)
        {
            return ItemBind(bindItem, () => new TabItem(Component.Ace), item => new TabItemBuilder(item));
        }

        public TabsBuilder Events(Action<TabsEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TabsEventBuilder(base.Component.Events));
			return this;
		}
	}
}
