using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ListBoxBuilder : HiddenBoxBuilder<ListBox, ListBoxBuilder>
	{
		public ListBoxBuilder(ListBox component)
			: base(component)
		{
		}

        public virtual ListBoxBuilder SelectUrl(string selectUrl)
        {
            Component.SelectUrl = selectUrl;
            return this;
        }

        public ListBoxBuilder Width(int width)
        {
            base.Component.Width = width;
            return this;
        }

        public ListBoxBuilder Height(int height)
        {
            base.Component.Height = height;
            return this;
        }

        public ListBoxBuilder Prompt(string prompt)
        {
            base.Component.Prompt = prompt;
            return this;
        }

		public ListBoxBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
