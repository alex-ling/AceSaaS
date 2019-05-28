using System;

using Acesoft.Data;
using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class DataView : ContentWidgetBase, IDataSourceWidget, IPaging, IDataBind
	{
		public Action<DataView> OnLoaded { get; set; }
		public DataSource DataSource { get; set; }
        public Paging Paging { get; set; }
        public object QueryParams { get; set; }

        public void DataBind()
		{
			var ds = DataSource.RouteValues.GetValue<string>("ds");
			if (ds.HasValue())
			{
				var gridRequest = new GridRequest
				{
					Page = App.GetQuery("page", Paging.PageNumber),
					Rows = App.GetQuery("rows", Paging.PageSize)
				};
				var ctx = new RequestContext(ds)
                    .SetCmdType(CmdType.query)
                    .SetParam(gridRequest)
                    .SetExtraParam(QueryParams);
				var gridResponse = Ace.Session.QueryPageTable(ctx, gridRequest);
				base.Model = gridResponse.Data;
				Paging.Load(gridResponse);
				OnLoaded?.Invoke(this);
			}
		}

		public DataView(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "dataview";
			DataSource = new DataSource(this);
			Paging = new Paging();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DataViewHtmlBuilder(this);
		}
	}
}
