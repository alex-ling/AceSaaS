using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DateTimeBoxBuilder : DateBoxBuilder<DatetimeBox, DateTimeBoxBuilder>
	{
		public DateTimeBoxBuilder(DatetimeBox component)
			: base(component)
		{
		}

		public virtual DateTimeBoxBuilder SpinnerWidth(int spinnerWidth)
		{
			base.Component.SpinnerWidth = spinnerWidth;
			return this;
		}

		public virtual DateTimeBoxBuilder ShowSeconds(bool showSeconds = true)
		{
			base.Component.ShowSeconds = showSeconds;
			return this;
		}

		public virtual DateTimeBoxBuilder TimeSeparator(string timeSeparator)
		{
			base.Component.TimeSeparator = timeSeparator;
			return this;
		}

		public DateTimeBoxBuilder Events(Action<DateBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DateBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
