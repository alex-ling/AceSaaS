using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MenuEventBuilder : EventBuilder
	{
		public MenuEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public MenuEventBuilder OnClick(string handler)
		{
			Handler(Menu.OnClick.EventName, handler);
			return this;
		}

		public MenuEventBuilder OnHide(string handler)
		{
			Handler(Menu.OnHide.EventName, handler);
			return this;
		}

		public MenuEventBuilder OnShow(string handler)
		{
			Handler(Menu.OnShow.EventName, handler);
			return this;
		}
	}
}
