using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class HiddenBox : WidgetBase
	{
		public string Value { get; set; }

		public InputType Type { get; set; }

		public HiddenBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "hiddenbox";
			Type = InputType.hidden;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new HiddenBoxHtmlBuilder<HiddenBox>(this);
		}
	}
}
