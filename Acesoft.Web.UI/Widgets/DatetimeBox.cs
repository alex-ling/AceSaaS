using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class DatetimeBox : DateBox
	{
		public int? SpinnerWidth { get; set; }

		public bool? ShowSeconds { get; set; }

		public string TimeSeparator { get; set; }

		public DatetimeBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "datetimebox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DatetimeBoxHtmlBuilder(this);
		}
	}
}
