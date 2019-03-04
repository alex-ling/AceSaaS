using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class HtmlerBuilder : ContentBuilder<Htmler, HtmlerBuilder>
	{
		public HtmlerBuilder(Htmler component)
			: base(component)
		{
		}

		public HtmlerBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
