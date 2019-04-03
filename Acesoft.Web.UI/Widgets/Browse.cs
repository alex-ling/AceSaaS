using System;
using System.Collections.Generic;
using System.Text;
using Acesoft.Data;
using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
    public class Browse : ContentWidgetBase, IDataSourceWidget, IDataBind
    {
        public DataSource DataSource { get; set; }
        public string RequestId { get; set; }

        public void DataBind()
        {
            var ds = DataSource.RouteValues.GetValue<string>("ds");
            if (ds.HasValue())
            {
                var ctx = new RequestContext(ds)
                    .SetCmdType(CmdType.select)
                    .SetParam(new
                    {
                        id = App.GetQuery(RequestId ?? "id", "")
                    })
                    .SetExtraParam(Ace.AC.Params);
                base.Model = base.Ace.Session.QueryFirst(ctx);
            }
        }

        public Browse(WidgetFactory ace) : base(ace)
        {
            DataSource = new DataSource(this);
            base.Attributes["class"] = "aceui-browse";
        }

        protected override IHtmlBuilder GetHtmlBuilder()
        {
            return new BrowseHtmlBuilder(this);
        }
    }
}
