using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Spinner : TextBox
	{
		public static readonly ScriptEvent OnSpin = new ScriptEvent("spin", "");

		public static readonly ScriptEvent OnSpinUp = new ScriptEvent("onSpinUp", "");

		public static readonly ScriptEvent OnSpinDown = new ScriptEvent("onSpinDown", "");

		public int? Min
		{
			get;
			set;
		}

		public int? Max
		{
			get;
			set;
		}

		public int? Increment
		{
			get;
			set;
		}

		public Align? SpinAlign
		{
			get;
			set;
		}

		public Spinner(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "spinner";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new SpinnerHtmlBuilder<Spinner>(this);
		}
	}
}
