using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SpinnerBuilder : SpinnerBuilder<Spinner, SpinnerBuilder>
	{
		public SpinnerBuilder(Spinner component)
			: base(component)
		{
		}

		public SpinnerBuilder Events(Action<SpinnerEventBuilder> clientEventsAction)
		{
			clientEventsAction(new SpinnerEventBuilder(base.Component.Events));
			return this;
		}
	}
	public abstract class SpinnerBuilder<Widget, Builder> : TextBoxBuilder<Widget, Builder> where Widget : Spinner where Builder : SpinnerBuilder<Widget, Builder>
	{
		public SpinnerBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder Min(int min)
		{
			base.Component.Min = min;
			return this as Builder;
		}

		public virtual Builder Max(int max)
		{
			base.Component.Max = max;
			return this as Builder;
		}

		public virtual Builder Increment(int increment)
		{
			base.Component.Increment = increment;
			return this as Builder;
		}

		public virtual Builder SpinAlign(Align align)
		{
			base.Component.SpinAlign = align;
			return this as Builder;
		}
	}
}
