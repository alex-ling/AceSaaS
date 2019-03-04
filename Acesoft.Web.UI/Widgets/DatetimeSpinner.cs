using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class DatetimeSpinner : TimeSpinner
	{
		public DatetimeSpinner(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "datetimespinner";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DatetimeSpinnerHtmlBuilder(this);
		}
	}
}
