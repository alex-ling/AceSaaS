using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class AccordionItemBuilder : PanelBuilder<AccordionItem, AccordionItemBuilder>
	{
		public AccordionItemBuilder(AccordionItem component)
			: base(component)
		{
		}

		public virtual AccordionItemBuilder Selected(bool selected = true)
		{
			base.Component.Selected = selected;
			return this;
		}

		public AccordionItemBuilder Events(Action<PanelEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PanelEventBuilder(base.Component.Events));
			return this;
		}
	}
}
