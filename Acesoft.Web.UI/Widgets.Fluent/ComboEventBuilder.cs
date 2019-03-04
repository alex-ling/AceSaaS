using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboEventBuilder : TextBoxEventBuilder
	{
		public ComboEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public ComboEventBuilder OnShowPanel(string handler)
		{
			Handler(Combo.OnShowPanel.EventName, handler);
			return this;
		}

		public ComboEventBuilder OnHidePanel(string handler)
		{
			Handler(Combo.OnHidePanel.EventName, handler);
			return this;
		}
	}
}
