using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class BrowseBuilder : ContentBuilder<Browse, BrowseBuilder>
	{
		public BrowseBuilder(Browse component)
			: base(component)
		{
        }

        public BrowseBuilder RequestId(string requestId)
        {
            Component.RequestId = requestId;
            return this;
        }

        public BrowseBuilder DataSource(string ds)
		{
			new DataSourceBuilder(base.Component.DataSource).DataSource(ds);
			return this;
		}

		public BrowseBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}