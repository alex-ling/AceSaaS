using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class NumberBox : TextBox
	{
		public static readonly ScriptEvent OnFilter = new ScriptEvent("filter", "e");

		public static readonly ScriptEvent OnFormatter = new ScriptEvent("formatter", "value");

		public static readonly ScriptEvent OnParser = new ScriptEvent("parser", "s");

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

		public NumberBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "numberbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new NumberBoxHtmlBuilder<NumberBox>(this);
		}
	}
}
