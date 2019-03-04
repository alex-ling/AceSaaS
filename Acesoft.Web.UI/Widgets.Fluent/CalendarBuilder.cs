using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class CalendarBuilder : WidgetBuilder<Calendar, CalendarBuilder>
	{
		public CalendarBuilder(Calendar component)
			: base(component)
		{
		}

		public virtual CalendarBuilder Width(int width)
		{
			base.Component.Width = width;
			return this;
		}

		public virtual CalendarBuilder Height(int height)
		{
			base.Component.Height = height;
			return this;
		}

		public virtual CalendarBuilder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this;
		}

		public virtual CalendarBuilder Border(bool border = true)
		{
			base.Component.Border = border;
			return this;
		}

		public virtual CalendarBuilder ShowWeek(bool showWeek = true)
		{
			base.Component.ShowWeek = showWeek;
			return this;
		}

		public virtual CalendarBuilder WeekNumberHeader(string weekNumberHeader)
		{
			base.Component.WeekNumberHeader = weekNumberHeader;
			return this;
		}

		public virtual CalendarBuilder FirstDay(int firstDay)
		{
			base.Component.FirstDay = firstDay;
			return this;
		}

		public virtual CalendarBuilder Year(int year)
		{
			base.Component.Year = year;
			return this;
		}

		public virtual CalendarBuilder Month(int month)
		{
			base.Component.Month = month;
			return this;
		}

		public virtual CalendarBuilder Current(DateTime current)
		{
			base.Component.Current = current;
			return this;
		}

		public CalendarBuilder Events(Action<SwitchButtonEventBuilder> clientEventsAction)
		{
			clientEventsAction(new SwitchButtonEventBuilder(base.Component.Events));
			return this;
		}
	}
}
