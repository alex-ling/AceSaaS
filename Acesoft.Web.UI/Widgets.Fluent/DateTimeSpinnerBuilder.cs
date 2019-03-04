using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DateTimeSpinnerBuilder : TimeSpinnerBuilder<DatetimeSpinner, DateTimeSpinnerBuilder>
	{
		public DateTimeSpinnerBuilder(DatetimeSpinner component)
			: base(component)
		{
		}

		public DateTimeSpinnerBuilder Events(Action<TimeSpinnerEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TimeSpinnerEventBuilder(base.Component.Events));
			return this;
		}
	}
}
