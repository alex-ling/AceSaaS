using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TabsEventBuilder : EventBuilder
	{
		public TabsEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TabsEventBuilder OnLoad(string handler)
		{
			Handler(Tabs.OnLoad.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnAdd(string handler)
		{
			Handler(Tabs.OnAdd.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnBeforeClose(string handler)
		{
			Handler(Tabs.OnBeforeClose.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnClose(string handler)
		{
			Handler(Tabs.OnClose.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnContextMenu(string handler)
		{
			Handler(Tabs.OnContextMenu.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnSelect(string handler)
		{
			Handler(Tabs.OnSelect.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnUnselect(string handler)
		{
			Handler(Tabs.OnUnselect.EventName, handler);
			return this;
		}

		public TabsEventBuilder OnUpdate(string handler)
		{
			Handler(Tabs.OnUpdate.EventName, handler);
			return this;
		}
	}
}
