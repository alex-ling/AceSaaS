using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class CheckBoxBuilder : WidgetBuilder<CheckBox, CheckBoxBuilder>
	{
		public CheckBoxBuilder(CheckBox component)
			: base(component)
		{
		}

		public CheckBoxBuilder Text(string text)
		{
			base.Component.Text = text;
			return this;
		}

		public CheckBoxBuilder Value(string value)
		{
			base.Component.Value = value;
			return this;
		}

		public CheckBoxBuilder Checked(bool @checked = true)
		{
			base.Component.Checked = @checked;
			return this;
		}

		public CheckBoxBuilder Group(string group)
		{
			base.Component.Group = group;
			return this;
		}

		public CheckBoxBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
