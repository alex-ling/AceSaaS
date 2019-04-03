using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class ProgressBar : WidgetBase
	{
		public static readonly ScriptEvent OnChange = new ScriptEvent("oOnChange", "nv,ov");

		public int? Width { get; set; }

		public int? Height { get; set; }

		public int? Value { get; set; }

		public string Text { get; set; }

		public ProgressBar(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "progressbar";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ProgressBarHtmlBuilder(this);
		}
	}
}
