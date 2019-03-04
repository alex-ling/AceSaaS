using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class HiddenBoxBuilder : HiddenBoxBuilder<HiddenBox, HiddenBoxBuilder>
	{
		public HiddenBoxBuilder(HiddenBox component)
			: base(component)
		{
		}

		public HiddenBoxBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
	public class HiddenBoxBuilder<Widget, Builder> : WidgetBuilder<Widget, Builder> where Widget : HiddenBox where Builder : HiddenBoxBuilder<Widget, Builder>
	{
		public HiddenBoxBuilder(Widget component)
			: base(component)
		{
		}

		public Builder Value(string value)
		{
			base.Component.Value = value;
			return this as Builder;
		}
	}
}
