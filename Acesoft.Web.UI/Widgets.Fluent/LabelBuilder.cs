using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class LabelBuilder : WidgetBuilder<Label, LabelBuilder>
	{
		public LabelBuilder(Label component)
			: base(component)
		{
		}

        public LabelBuilder Text(string text)
        {
            Component.Text = text;
            return this;
        }

        public LabelBuilder Events(Action<EventBuilder> clientEventsAction)
        {
            clientEventsAction(new EventBuilder(base.Component.Events));
            return this;
        }
    }
}
