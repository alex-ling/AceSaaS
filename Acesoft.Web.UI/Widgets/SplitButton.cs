using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class SplitButton : LinkButton
	{
		public string Menu { get; set; }

		public int? Duration { get; set; }

		public SplitButton(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "splitbutton";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new SplitButtonHtmlBuilder<SplitButton>(this);
		}
	}
}
