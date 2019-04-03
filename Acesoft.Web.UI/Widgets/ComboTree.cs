using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Text;

namespace Acesoft.Web.UI.Widgets
{
	public class ComboTree : Combo, IDataSourceWidget
	{
		public string TextField { get; set; }

		public DataSource DataSource { get; set; }

		public bool? Animate { get; set; }

		public bool? Checkbox { get; set; }

		public bool? CascadeCheck { get; set; }

		public bool? OnlyLeafCheck { get; set; }

		public bool? Lines { get; set; }

		public StringBuilder Data { get; set; }

		public ComboTree(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combotree";
			Data = new StringBuilder();
			DataSource = new DataSource(this);
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ComboTreeHtmlBuilder<ComboTree>(this);
		}
	}
}
