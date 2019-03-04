using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataGridColumnEventBuilder : EventBuilder
	{
		public DataGridColumnEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public DataGridColumnEventBuilder OnFormatter(string handler)
		{
			Handler(DataGridColumn.Formatter.EventName, handler);
			return this;
		}

		public DataGridColumnEventBuilder OnStyler(string handler)
		{
			Handler(DataGridColumn.Styler.EventName, handler);
			return this;
		}

		public DataGridColumnEventBuilder OnSorter(string handler)
		{
			Handler(DataGridColumn.Sorter.EventName, handler);
			return this;
		}
	}
}
