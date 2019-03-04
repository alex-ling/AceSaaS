using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PaginationBuilder : WidgetBuilder<Pagination, PaginationBuilder>
	{
		public PaginationBuilder(Pagination component)
			: base(component)
		{
		}

		public virtual PaginationBuilder Total(int total)
		{
			base.Component.Total = total;
			return this;
		}

		public virtual PaginationBuilder PageSize(int pageSize)
		{
			base.Component.PageSize = pageSize;
			return this;
		}

		public virtual PaginationBuilder PageList(Action<IList<int>> listAction)
		{
			listAction(base.Component.PageList);
			return this;
		}

		public virtual PaginationBuilder Loading(bool loading = true)
		{
			base.Component.Loading = loading;
			return this;
		}

		public virtual PaginationBuilder Buttons(Action<IList<LinkButton>> nuttonsAction)
		{
			nuttonsAction(base.Component.Buttons);
			return this;
		}

		public virtual PaginationBuilder Layout(Action<IList<PaginationItem>> layoutAction)
		{
			layoutAction(base.Component.Layout);
			return this;
		}

		public virtual PaginationBuilder Links(int links)
		{
			base.Component.Links = links;
			return this;
		}

		public virtual PaginationBuilder ShowPageList(bool showPageList = true)
		{
			base.Component.ShowPageList = showPageList;
			return this;
		}

		public virtual PaginationBuilder ShowRefresh(bool showRefresh = true)
		{
			base.Component.ShowRefresh = showRefresh;
			return this;
		}

		public virtual PaginationBuilder ShowPageInfo(bool showPageInfo = true)
		{
			base.Component.ShowPageInfo = showPageInfo;
			return this;
		}

		public virtual PaginationBuilder BeforePageText(string beforePageText)
		{
			base.Component.BeforePageText = beforePageText;
			return this;
		}

		public virtual PaginationBuilder AfterPageText(string afterPageText)
		{
			base.Component.AfterPageText = afterPageText;
			return this;
		}

		public virtual PaginationBuilder DisplayMsg(string displayMsg)
		{
			base.Component.DisplayMsg = displayMsg;
			return this;
		}

		public PaginationBuilder Events(Action<PaginationEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PaginationEventBuilder(base.Component.Events));
			return this;
		}
	}
}
