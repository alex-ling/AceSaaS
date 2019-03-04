using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class CheckBoxList : TableWidgetBase<CheckBox>, IDataSourceWidget, IWidget, IHtml, IHtmlContent
	{
		public string Value
		{
			get;
			set;
		}

		public DataSource DataSource
		{
			get;
			set;
		}

		public CheckBoxList(WidgetFactory ace)
			: base(ace)
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
