using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class DataList : DataGrid
	{
		public static readonly ScriptEvent OnTextFormatter = new ScriptEvent("textFormatter", "value,row,index");

		public static readonly ScriptEvent OnGroupFormatter = new ScriptEvent("groupFormatter", "value,rows");

		public bool? Lines { get; set; }

		public string ValueField { get; set; }

		public string TextField { get; set; }

		public string GroupField { get; set; }

		public DataList(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "datalist";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DataListHtmlBuilder(this);
		}
	}
}
