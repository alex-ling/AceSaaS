using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class AccordionEventBuilder : EventBuilder
	{
		public AccordionEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public AccordionEventBuilder OnAdd(string handler)
		{
			Handler(Accordion.OnAdd.EventName, handler);
			return this;
		}

		public AccordionEventBuilder OnBeforeRemove(string handler)
		{
			Handler(Accordion.OnBeforeRemove.EventName, handler);
			return this;
		}

		public AccordionEventBuilder OnRemove(string handler)
		{
			Handler(Accordion.OnRemove.EventName, handler);
			return this;
		}

		public AccordionEventBuilder OnSelect(string handler)
		{
			Handler(Accordion.OnSelect.EventName, handler);
			return this;
		}

		public AccordionEventBuilder OnUnselect(string handler)
		{
			Handler(Accordion.OnUnselect.EventName, handler);
			return this;
		}
	}
}
