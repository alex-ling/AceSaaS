using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Htmler : ContentWidgetBase
	{
		public Htmler(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "htmler";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new WidgetHtmlBuilder<Htmler>(this, "div");
		}
	}
}
