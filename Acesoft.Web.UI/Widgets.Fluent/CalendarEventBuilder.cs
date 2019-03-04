using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class CalendarEventBuilder : EventBuilder
	{
		public CalendarEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public CalendarEventBuilder OnGetWeekNumber(string handler)
		{
			Handler(Calendar.OnGetWeekNumber.EventName, handler);
			return this;
		}

		public CalendarEventBuilder OnFormatter(string handler)
		{
			Handler(Calendar.OnFormatter.EventName, handler);
			return this;
		}

		public CalendarEventBuilder OnStyler(string handler)
		{
			Handler(Calendar.OnStyler.EventName, handler);
			return this;
		}

		public CalendarEventBuilder OnValidator(string handler)
		{
			Handler(Calendar.OnValidator.EventName, handler);
			return this;
		}

		public CalendarEventBuilder OnSelect(string handler)
		{
			Handler(Calendar.OnSelect.EventName, handler);
			return this;
		}

		public CalendarEventBuilder OnChange(string handler)
		{
			Handler(Calendar.OnChange.EventName, handler);
			return this;
		}
	}
}
