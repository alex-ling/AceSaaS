using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class LayoutEventBuilder : EventBuilder
	{
		public LayoutEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public LayoutEventBuilder OnCollapse(string handler)
		{
			Handler(Layout.OnCollapse.EventName, handler);
			return this;
		}

		public LayoutEventBuilder OnExpand(string handler)
		{
			Handler(Layout.OnExpand.EventName, handler);
			return this;
		}

		public LayoutEventBuilder OnAdd(string handler)
		{
			Handler(Layout.OnAdd.EventName, handler);
			return this;
		}

		public LayoutEventBuilder OnRemove(string handler)
		{
			Handler(Layout.OnRemove.EventName, handler);
			return this;
		}
	}
}
