using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class DataGridColumn : WidgetBase
	{
		public static readonly ScriptEvent Formatter = new ScriptEvent("formatter", "value,row,index");

		public static readonly ScriptEvent Styler = new ScriptEvent("styler", "value,row,index");

		public static readonly ScriptEvent Sorter = new ScriptEvent("sorter", "a,b");

		public string Title { get; set; }

		public string Field { get; set; }

		public int? Width { get; set; }

		public int? Rowspan { get; set; }

		public int? Colspan { get; set; }

		public Align? Align { get; set; }

		public Align? Halign { get; set; }

		public bool? Sortable { get; set; }

		public Order? Order { get; set; }

		public bool? Resizable { get; set; }

		public bool? Fixed { get; set; }

		public bool? Hidden { get; set; }

		public bool? Checkbox { get; set; }

		public string Editor { get; set; }

		public string Format { get; set; }

		public bool? Merged { get; set; }

        public int? Type { get; set; }

        public DataGrid Grid { get; set; }

		public DataGridColumn(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = null;
			Sortable = true;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DataGridColumnHtmlBuilder(this);
		}
	}
}
