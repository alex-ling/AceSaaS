using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ProgressBarBuilder : WidgetBuilder<ProgressBar, ProgressBarBuilder>
	{
		public ProgressBarBuilder(ProgressBar component)
			: base(component)
		{
		}

		public virtual ProgressBarBuilder Width(int width)
		{
			base.Component.Width = width;
			return this;
		}

		public virtual ProgressBarBuilder Height(int height)
		{
			base.Component.Height = height;
			return this;
		}

		public virtual ProgressBarBuilder Value(int value)
		{
			base.Component.Value = value;
			return this;
		}

		public virtual ProgressBarBuilder Text(string text)
		{
			base.Component.Text = text;
			return this;
		}

		public ProgressBarBuilder Events(Action<ProgressBarEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ProgressBarEventBuilder(base.Component.Events));
			return this;
		}
	}
}
