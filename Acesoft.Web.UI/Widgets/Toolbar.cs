using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Toolbar : ContentWidgetBase
	{
        public string Buttons { get; set; }
        public string OnClick { get; set; }

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
