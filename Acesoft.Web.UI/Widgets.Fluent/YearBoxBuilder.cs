using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class YearBoxBuilder : ComboBoxBuilder<YearBox, YearBoxBuilder>
	{
		public YearBoxBuilder(YearBox component)
			: base(component)
		{
		}

		public YearBoxBuilder Start(int start)
		{
			base.Component.Start = start;
			return this;
		}

		public YearBoxBuilder End(int end)
		{
			base.Component.End = end;
			return this;
		}

		public YearBoxBuilder Events(Action<ComboBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ComboBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
