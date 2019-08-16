using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acesoft.Data;
using Acesoft.Platform.Models;
using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class CheckBoxList : TableWidgetBase<CheckBox>, IDataSourceWidget
	{
        public InputType Type { get; set; }

        public string Value { get; set; }

		public DataSource DataSource { get; set; }

        public override void DataBind()
        {
            if (Model == null)
            {
                var ds = DataSource.RouteValues.GetValue("datasource", "");
                if (ds.HasValue())
                {
                    var ctx = new RequestContext(ds)
                        .SetCmdType(CmdType.query)
                        .SetExtraParam(Ace.AppCtx.AC.Params);
                    Model = Ace.AppCtx.Session.Query<DictItem>(ctx);
                }
            }

            if (Model is IEnumerable<DictItem> models)
            {
                models.Each(item => AddItem(item));
            }
            else if (Model is IEnumerable<KeyValuePair<string, string>> dict)
            {
                dict.Each(item => AddItem(new DictItem(item.Key, item.Value)));
            }
        }

        private void AddItem(DictItem item)
        {
            var check = new CheckBox(this.Ace);
            check.Id = $"{Id}_{Items.Count}";
            check.Type = Type;
            check.Text = item.text;
            check.Value = item.value;
            check.Group = Id;
            check.Checked = Value.HasValue() && Value.Split(',').Contains(item.value);
            Events.Each(e => check.Events.Add(e));
            Items.Add(check);
        }

        public CheckBoxList(WidgetFactory ace) : base(ace)
		{
			base.Widget = "checkboxlist";
			DataSource = new DataSource(this);
			base.ColumnSize = 1;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new CheckBoxListHtmlBuilder(this);
		}
    }
}
