using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TimeSpinnerBuilder : TimeSpinnerBuilder<TimeSpinner, TimeSpinnerBuilder>
	{
		public TimeSpinnerBuilder(TimeSpinner component)
			: base(component)
		{
		}

		public TimeSpinnerBuilder Events(Action<SpinnerEventBuilder> clientEventsAction)
		{
			clientEventsAction(new SpinnerEventBuilder(base.Component.Events));
			return this;
		}
	}
	public abstract class TimeSpinnerBuilder<Widget, Builder> : TextBoxBuilder<Widget, Builder> where Widget : TimeSpinner where Builder : TimeSpinnerBuilder<Widget, Builder>
	{
		public TimeSpinnerBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder Separator(string separator)
		{
			base.Component.Separator = separator;
			return this as Builder;
		}

		public virtual Builder ShowSeconds(bool showSeconds = true)
		{
			base.Component.ShowSeconds = showSeconds;
			return this as Builder;
		}

		public virtual Builder Highlight(TimeSection highlight)
		{
			base.Component.Highlight = (int)highlight;
			return this as Builder;
		}

		public virtual Builder Selections(Action<IList<IList<int>>> action)
		{
			action(base.Component.Selections);
			return this as Builder;
		}
	}
}
