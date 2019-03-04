using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TimeSpinnerEventBuilder : SpinnerEventBuilder
	{
		public TimeSpinnerEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TimeSpinnerEventBuilder OnFormatter(string handler)
		{
			Handler(TimeSpinner.OnFormatter.EventName, handler);
			return this;
		}

		public TimeSpinnerEventBuilder OnParser(string handler)
		{
			Handler(TimeSpinner.OnParser.EventName, handler);
			return this;
		}
	}
}
