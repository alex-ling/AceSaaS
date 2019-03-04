using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PasswordBoxBuilder : TextBoxBuilder<PasswordBox, PasswordBoxBuilder>
	{
		public PasswordBoxBuilder(PasswordBox component)
			: base(component)
		{
		}

		public virtual PasswordBoxBuilder PasswordChar(string passwordChar)
		{
			base.Component.PasswordChar = passwordChar;
			return this;
		}

		public virtual PasswordBoxBuilder CheckInterval(int checkInterval)
		{
			base.Component.CheckInterval = checkInterval;
			return this;
		}

		public virtual PasswordBoxBuilder LastDelay(int lastDelay)
		{
			base.Component.LastDelay = lastDelay;
			return this;
		}

		public virtual PasswordBoxBuilder Revealed(bool revealed = true)
		{
			base.Component.Revealed = revealed;
			return this;
		}

		public virtual PasswordBoxBuilder ShowEye(bool showEye = true)
		{
			base.Component.ShowEye = showEye;
			return this;
		}

		public PasswordBoxBuilder Events(Action<TextBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TextBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
