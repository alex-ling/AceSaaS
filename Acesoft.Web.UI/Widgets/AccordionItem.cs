using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class AccordionItem : Panel
	{
		public bool? Selected
		{
			get;
			set;
		}

		public AccordionItem(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = null;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new AccordionItemHtmlBuilder(this);
		}
	}
}
