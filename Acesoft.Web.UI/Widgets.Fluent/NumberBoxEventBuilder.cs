using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class NumberBoxEventBuilder : TextBoxEventBuilder
	{
		public NumberBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public NumberBoxEventBuilder OnFilter(string handler)
		{
			Handler(NumberBox.OnFilter.EventName, handler);
			return this;
		}

		public NumberBoxEventBuilder OnFormatter(string handler)
		{
			Handler(NumberBox.OnFormatter.EventName, handler);
			return this;
		}

		public NumberBoxEventBuilder OnParser(string handler)
		{
			Handler(NumberBox.OnParser.EventName, handler);
			return this;
		}
	}
}
