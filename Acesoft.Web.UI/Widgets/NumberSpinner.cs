using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class NumberSpinner : Spinner
	{
		public int? Precision
		{
			get;
			set;
		}

		public string DecimalSeparator
		{
			get;
			set;
		}

		public string GroupSeparator
		{
			get;
			set;
		}

		public string Prefix
		{
			get;
			set;
		}

		public string Suffix
		{
			get;
			set;
		}

		public NumberSpinner(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "numberspinner";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new NumberSpinnerHtmlBuilder<NumberSpinner>(this);
		}
	}
}
