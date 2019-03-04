using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataListEventBuilder : DataGridEventBuilder
	{
		public DataListEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public DataListEventBuilder OnTextFormatter(string handler)
		{
			Handler(DataList.OnTextFormatter.EventName, handler);
			return this;
		}

		public DataListEventBuilder OnGroupFormatter(string handler)
		{
			Handler(DataList.OnGroupFormatter.EventName, handler);
			return this;
		}
	}
}
