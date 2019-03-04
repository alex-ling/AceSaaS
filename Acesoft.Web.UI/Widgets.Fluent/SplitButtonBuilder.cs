using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SplitButtonBuilder : SplitButtonBuilder<SplitButton, SplitButtonBuilder>
	{
		public SplitButtonBuilder(SplitButton component)
			: base(component)
		{
		}
	}
	public class SplitButtonBuilder<Widget, Builder> : LinkButtonBuilder<Widget, Builder> where Widget : SplitButton where Builder : WidgetBuilder<Widget, Builder>
	{
		public SplitButtonBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder Menu(string menu)
		{
			base.Component.Menu = menu;
			return this as Builder;
		}

		public virtual Builder Duration(int duration)
		{
			base.Component.Duration = duration;
			return this as Builder;
		}

		public Builder Events(Action<PaginationEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PaginationEventBuilder(base.Component.Events));
			return this as Builder;
		}
	}
}
