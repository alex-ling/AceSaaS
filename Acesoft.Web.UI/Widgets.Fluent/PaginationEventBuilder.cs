using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PaginationEventBuilder : EventBuilder
	{
		public PaginationEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public PaginationEventBuilder OnBeforeRefresh(string handler)
		{
			Handler(Pagination.OnBeforeRefresh.EventName, handler);
			return this;
		}

		public PaginationEventBuilder OnChangePageSize(string handler)
		{
			Handler(Pagination.OnChangePageSize.EventName, handler);
			return this;
		}

		public PaginationEventBuilder OnRefresh(string handler)
		{
			Handler(Pagination.OnRefresh.EventName, handler);
			return this;
		}

		public PaginationEventBuilder OnSelectPage(string handler)
		{
			Handler(Pagination.OnSelectPage.EventName, handler);
			return this;
		}
	}
}
