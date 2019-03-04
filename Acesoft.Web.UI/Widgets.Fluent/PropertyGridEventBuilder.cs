using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PropertyGridEventBuilder : DataGridEventBuilder
	{
		public PropertyGridEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public PropertyGridEventBuilder OnGroupFormatter(string handler)
		{
			Handler(PropertyGrid.OnGroupFormatter.EventName, handler);
			return this;
		}
	}
}
