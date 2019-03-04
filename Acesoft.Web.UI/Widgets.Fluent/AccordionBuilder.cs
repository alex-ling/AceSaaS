using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class AccordionBuilder : ListBuilder<Accordion, AccordionItem, AccordionBuilder>
	{
		public AccordionBuilder(Accordion component)
			: base(component)
		{
		}

		public virtual AccordionBuilder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this;
		}

		public virtual AccordionBuilder Border(bool border = true)
		{
			base.Component.Border = border;
			return this;
		}

		public virtual AccordionBuilder Animate(bool animate = true)
		{
			base.Component.Animate = animate;
			return this;
		}

		public virtual AccordionBuilder Multiple(bool multiple = true)
		{
			base.Component.Multiple = multiple;
			return this;
		}

		public virtual AccordionBuilder Selected(int index)
		{
			base.Component.Selected = index;
			return this;
		}

		public virtual AccordionBuilder HAlign(Align align)
		{
			base.Component.HAlign = align;
			return this;
		}

		public AccordionBuilder Items(Action<ItemsBuilder<AccordionItem, AccordionItemBuilder>> addAction)
		{
			return Items(addAction, () => new AccordionItem(base.Component.Ace), (AccordionItem item) => new AccordionItemBuilder(item));
		}

		public AccordionBuilder Events(Action<AccordionEventBuilder> clientEventsAction)
		{
			clientEventsAction(new AccordionEventBuilder(base.Component.Events));
			return this;
		}
	}
}
