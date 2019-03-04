using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class DataSourceEventBuilder : AjaxEventBuilder
	{
		public DataSourceEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public DataSourceEventBuilder Loader(string handler)
		{
			Handler(DataSource.Loader.EventName, handler);
			return this;
		}

		public DataSourceEventBuilder LoadFilter(string handler)
		{
			Handler(DataSource.LoadFilter.EventName, handler);
			return this;
		}

		public DataSourceEventBuilder OnBeforeLoad(string handler)
		{
			Handler(DataSource.OnBeforeLoad.EventName, handler);
			return this;
		}

		public DataSourceEventBuilder OnLoadSuccess(string handler)
		{
			Handler(DataSource.OnLoadSuccess.EventName, handler);
			return this;
		}

		public DataSourceEventBuilder OnLoadError(string handler)
		{
			Handler(DataSource.OnLoadError.EventName, handler);
			return this;
		}
	}
}
