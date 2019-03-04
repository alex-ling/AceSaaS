using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SwitchButtonEventBuilder : EventBuilder
	{
		public SwitchButtonEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public SwitchButtonEventBuilder OnChange(string handler)
		{
			Handler(SwitchButton.OnChange.EventName, handler);
			return this;
		}
	}
}
