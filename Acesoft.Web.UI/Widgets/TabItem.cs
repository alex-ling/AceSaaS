using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class TabItem : Panel
	{
		public TabItem(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = null;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new PanelHtmlBuilder<TabItem>(this);
		}
	}
}
