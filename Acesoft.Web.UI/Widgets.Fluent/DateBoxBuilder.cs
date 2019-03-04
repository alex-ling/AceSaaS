using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DateBoxBuilder : DateBoxBuilder<DateBox, DateBoxBuilder>
	{
		public DateBoxBuilder(DateBox component)
			: base(component)
		{
		}

		public DateBoxBuilder Events(Action<DateBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DateBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
	public class DateBoxBuilder<Widget, Builder> : ComboBuilder<Widget, Builder> where Widget : DateBox where Builder : DateBoxBuilder<Widget, Builder>
	{
		public DateBoxBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder CurrentText(string currentText)
		{
			base.Component.CurrentText = currentText;
			return this as Builder;
		}

		public virtual Builder CloseText(string closeText)
		{
			base.Component.CloseText = closeText;
			return this as Builder;
		}

		public virtual Builder OkText(string okText)
		{
			base.Component.OkText = okText;
			return this as Builder;
		}

		public virtual Builder Buttons(Action<IList<LinkButton>> buttonsAction)
		{
			buttonsAction(base.Component.Buttons);
			return this as Builder;
		}

		public virtual Builder SharedCalendar(string sharedCalendar)
		{
			base.Component.SharedCalendar = sharedCalendar;
			return this as Builder;
		}
	}
}
