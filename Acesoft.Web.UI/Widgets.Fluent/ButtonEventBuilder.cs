using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ButtonEventBuilder : EventBuilder
	{
		public ButtonEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public ButtonEventBuilder OnClick(string handler)
		{
			Handler(LinkButton.OnClick.EventName, handler);
			return this;
		}
	}
}
