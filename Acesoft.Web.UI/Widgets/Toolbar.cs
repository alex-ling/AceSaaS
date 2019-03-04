using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Toolbar : ContentWidgetBase
	{
		public Toolbar(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "toolbar";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ToolbarHtmlBuilder(this);
		}
	}
}
