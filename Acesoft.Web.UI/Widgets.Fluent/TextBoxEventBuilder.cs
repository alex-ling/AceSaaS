using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TextBoxEventBuilder : ValidateBoxEventBuilder
	{
		public TextBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TextBoxEventBuilder OnChange(string handler)
		{
			Handler(TextBox.OnChange.EventName, handler);
			return this;
		}

		public ValidateBoxEventBuilder OnResize(string handler)
		{
			Handler(TextBox.OnResize.EventName, handler);
			return this;
		}

		public ValidateBoxEventBuilder OnClickButton(string handler)
		{
			Handler(TextBox.OnClickButton.EventName, handler);
			return this;
		}

		public ValidateBoxEventBuilder OnClickIcon(string handler)
		{
			Handler(TextBox.OnClickIcon.EventName, handler);
			return this;
		}
	}
}
