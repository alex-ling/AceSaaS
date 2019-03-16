using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Label : WidgetBase
	{
        public string Text { get; set; }

        public Label(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "label";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new LabelHtmlBuilder(this);
		}
	}
}
