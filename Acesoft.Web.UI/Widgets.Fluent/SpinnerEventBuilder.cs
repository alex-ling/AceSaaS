using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SpinnerEventBuilder : TextBoxEventBuilder
	{
		public SpinnerEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public SpinnerEventBuilder OnSpin(string handler)
		{
			Handler(Spinner.OnSpin.EventName, handler);
			return this;
		}

		public SpinnerEventBuilder OnSpinUp(string handler)
		{
			Handler(Spinner.OnSpinUp.EventName, handler);
			return this;
		}

		public SpinnerEventBuilder OnSpinDown(string handler)
		{
			Handler(Spinner.OnSpinDown.EventName, handler);
			return this;
		}
	}
}
