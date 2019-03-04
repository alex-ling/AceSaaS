using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DateBoxEventBuilder : ComboEventBuilder
	{
		public DateBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public DateBoxEventBuilder OnSelect(string handler)
		{
			Handler(DateBox.OnSelect.EventName, handler);
			return this;
		}
	}
}
