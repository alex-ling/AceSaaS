using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class LayoutBuilder : ListBuilder<Layout, LayoutBuilder, LayoutItem, LayoutItemBuilder>
	{
		public LayoutBuilder(Layout component) : base(component)
		{
		}

		public virtual LayoutBuilder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this;
		}

		public LayoutBuilder Items(Action<ItemsBuilder<LayoutItem, LayoutItemBuilder>> addAction)
		{
			return Items(addAction, () => new LayoutItem(base.Component.Ace), (LayoutItem item) => new LayoutItemBuilder(item));
		}

		public LayoutBuilder Events(Action<LayoutEventBuilder> clientEventsAction)
		{
			clientEventsAction(new LayoutEventBuilder(base.Component.Events));
			return this;
		}
	}
}
