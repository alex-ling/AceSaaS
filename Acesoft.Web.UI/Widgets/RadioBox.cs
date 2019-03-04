using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class RadioBox : CheckBox
	{
		public RadioBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "radiobox";
			base.Type = InputType.radio;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new CheckBoxHtmlBuilder(this);
		}
	}
}
