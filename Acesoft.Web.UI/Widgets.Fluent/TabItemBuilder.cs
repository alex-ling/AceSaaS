using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TabItemBuilder : PanelBuilder<TabItem, TabItemBuilder>
	{
		public TabItemBuilder(TabItem component)
			: base(component)
		{
		}

		public TabItemBuilder Events(Action<PanelEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PanelEventBuilder(base.Component.Events));
			return this;
		}
	}
}
