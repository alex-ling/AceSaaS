using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataViewBuilder : ContentBuilder<DataView, DataViewBuilder>
	{
		public DataViewBuilder(DataView component)
			: base(component)
		{
		}

		public DataViewBuilder PageSize(int pageSize)
		{
			base.Component.Paging.PageSize = pageSize;
			return this;
		}

		public DataViewBuilder PageNumber(int page)
		{
			base.Component.Paging.PageNumber = page;
			return this;
		}

		public DataViewBuilder DataSource(string ds)
		{
			new DataSourceBuilder(base.Component.DataSource).DataSource(ds);
			return this;
		}

		public DataViewBuilder OnLoaded(Action<DataView> action)
		{
			base.Component.OnLoaded = action;
			return this;
		}

		public DataViewBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
