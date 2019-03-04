using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class TreeGrid : DataGrid
	{
		public static readonly ScriptEvent OnCheckbox = new ScriptEvent("checkbox", "row");

		public static readonly ScriptEvent OnBeforeCheckNode = new ScriptEvent("OnBeforeCheckNode", "");

		public static readonly ScriptEvent OnCheckNode = new ScriptEvent("OnCheckNode", "");

		public static readonly ScriptEvent OnContextMenu = new ScriptEvent("OnContextMenu", "");

		public string TreeField
		{
			get;
			set;
		}

		public bool? Animate
		{
			get;
			set;
		}

		public bool? Checkbox
		{
			get;
			set;
		}

		public bool? CascadeCheck
		{
			get;
			set;
		}

		public bool? OnlyLeafCheck
		{
			get;
			set;
		}

		public bool? Lines
		{
			get;
			set;
		}

		public TreeGrid(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "treegrid";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TreeGridHtmlBuilder(this);
		}
	}
}
