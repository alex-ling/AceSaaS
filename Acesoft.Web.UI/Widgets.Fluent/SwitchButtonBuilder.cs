using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SwitchButtonBuilder : WidgetBuilder<SwitchButton, SwitchButtonBuilder>
	{
		public SwitchButtonBuilder(SwitchButton component)
			: base(component)
		{
		}

		public virtual SwitchButtonBuilder Width(int width)
		{
			base.Component.Width = width;
			return this;
		}

		public virtual SwitchButtonBuilder Height(int height)
		{
			base.Component.Height = height;
			return this;
		}

		public virtual SwitchButtonBuilder HandleWidth(int handleWidth)
		{
			base.Component.HandleWidth = handleWidth;
			return this;
		}

		public virtual SwitchButtonBuilder Checked(bool @checked = true)
		{
			base.Component.Checked = @checked;
			return this;
		}

		public new virtual SwitchButtonBuilder Disabled(bool disabled = true)
		{
			base.Component.Disabled = disabled;
			return this;
		}

		public virtual SwitchButtonBuilder Readonly(bool @readonly = true)
		{
			base.Component.Readonly = @readonly;
			return this;
		}

		public virtual SwitchButtonBuilder Reversed(bool reversed = true)
		{
			base.Component.Reversed = reversed;
			return this;
		}

		public virtual SwitchButtonBuilder OnText(string onText)
		{
			base.Component.OnText = onText;
			return this;
		}

		public virtual SwitchButtonBuilder OffText(string offText)
		{
			base.Component.OffText = offText;
			return this;
		}

		public virtual SwitchButtonBuilder HandleText(string handleText)
		{
			base.Component.HandleText = handleText;
			return this;
		}

		public virtual SwitchButtonBuilder Value(string value)
		{
			base.Component.Value = value;
			return this;
		}

		public SwitchButtonBuilder Events(Action<SwitchButtonEventBuilder> clientEventsAction)
		{
			clientEventsAction(new SwitchButtonEventBuilder(base.Component.Events));
			return this;
		}
	}
}
