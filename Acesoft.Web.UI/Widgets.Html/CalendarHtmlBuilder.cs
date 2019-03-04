using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class CalendarHtmlBuilder : EasyUIHtmlBuilder<Calendar>
	{
		public CalendarHtmlBuilder(Calendar component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Width.HasValue)
			{
				base.Options["width"] = base.Component.Width;
			}
			if (base.Component.Height.HasValue)
			{
				base.Options["height"] = base.Component.Height;
			}
			if (base.Component.Fit.HasValue)
			{
				base.Options["fit"] = base.Component.Fit;
			}
			if (base.Component.Border.HasValue)
			{
				base.Options["border"] = base.Component.Border;
			}
			if (base.Component.ShowWeek.HasValue)
			{
				base.Options["showWeek"] = base.Component.ShowWeek;
			}
			if (base.Component.WeekNumberHeader.HasValue())
			{
				base.Options["weekNumberHeader"] = base.Component.WeekNumberHeader;
			}
			if (base.Component.FirstDay.HasValue)
			{
				base.Options["firstDay"] = base.Component.FirstDay;
			}
			if (base.Component.Year.HasValue)
			{
				base.Options["year"] = base.Component.Year;
			}
			if (base.Component.Month.HasValue)
			{
				base.Options["month"] = base.Component.Month;
			}
			if (base.Component.Current.HasValue)
			{
				base.Options["current"] = base.Component.Current;
			}
		}
	}
}
