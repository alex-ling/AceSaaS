using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class MenuButton : SplitButton
	{
		public Align? MenuAlign
		{
			get;
			set;
		}

		public bool? HasDownArrow
		{
			get;
			set;
		}

		public MenuButton(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "menubutton";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new MenuButtonHtmlBuilder(this);
		}
	}
}
