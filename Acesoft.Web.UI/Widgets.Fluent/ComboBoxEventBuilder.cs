using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboBoxEventBuilder : ComboEventBuilder
	{
		public ComboBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public ComboBoxEventBuilder OnFilter(string handler)
		{
			Handler(ComboBox.OnFilter.EventName, handler);
			return this;
		}

		public ComboBoxEventBuilder OnFormatter(string handler)
		{
			Handler(ComboBox.OnFormatter.EventName, handler);
			return this;
		}

		public ComboBoxEventBuilder OnClick(string handler)
		{
			Handler(ComboBox.OnClick.EventName, handler);
			return this;
		}

		public ComboBoxEventBuilder OnSelect(string handler)
		{
			Handler(ComboBox.OnSelect.EventName, handler);
			return this;
		}

		public ComboBoxEventBuilder OnUnselect(string handler)
		{
			Handler(ComboBox.OnUnselect.EventName, handler);
			return this;
		}
	}
}
