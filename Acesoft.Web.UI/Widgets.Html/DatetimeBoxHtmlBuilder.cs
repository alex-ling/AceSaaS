namespace Acesoft.Web.UI.Widgets.Html
{
	public class DatetimeBoxHtmlBuilder : ComboHtmlBuilder<DatetimeBox>
	{
		public DatetimeBoxHtmlBuilder(DatetimeBox component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.SpinnerWidth.HasValue)
			{
				base.Options["spinnerWidth"] = base.Component.SpinnerWidth;
			}
			if (base.Component.ShowSeconds.HasValue)
			{
				base.Options["showSeconds"] = base.Component.ShowSeconds;
			}
			if (base.Component.TimeSeparator.HasValue())
			{
				base.Options["timeSeparator"] = base.Component.TimeSeparator;
			}
		}
	}
}
