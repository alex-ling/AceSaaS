using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class PropertyGrid : DataGrid
	{
		public static readonly ScriptEvent OnGroupFormatter = new ScriptEvent("groupFormatter", "group,rows");

		public bool? ShowGroup
		{
			get;
			set;
		}

		public string GroupField
		{
			get;
			set;
		}

		public PropertyGrid(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "propertygrid";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new PropertyGridHtmlBuilder(this);
		}
	}
}
