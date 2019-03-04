using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ProgressBarEventBuilder : EventBuilder
	{
		public ProgressBarEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public ProgressBarEventBuilder OnChange(string handler)
		{
			Handler(ProgressBar.OnChange.EventName, handler);
			return this;
		}
	}
}
