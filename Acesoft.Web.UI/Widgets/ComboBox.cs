using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class ComboBox : Combo, IDataSourceWidget
	{
		public static readonly ScriptEvent OnFilter = new ScriptEvent("filter", "q,row");

		public static readonly ScriptEvent OnFormatter = new ScriptEvent("formatter", "row");

		public static readonly ScriptEvent OnClick = new ScriptEvent("onClick", "row");

		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "row");

		public static readonly ScriptEvent OnUnselect = new ScriptEvent("onUnselect", "row");

		public string ValueField { get; set; }

		public string TextField { get; set; }

        public string GroupField { get; set; }

        public List<ComboItem> Data { get; set; }

        public bool? LimitToList { get; set; }

		public bool? ShowItemIcon { get; set; }

		public GroupPos? GroupPosition { get; set; }

		public DataSource DataSource { get; set; }

		public ComboBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combobox";
			Data = new List<ComboItem>();
			DataSource = new DataSource(this);
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ComboBoxHtmlBuilder<ComboBox>(this);
		}
	}
}
