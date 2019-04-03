using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Text;

namespace Acesoft.Web.UI.Widgets
{
	public class Tree : TreeWidgetBase<TreeNode>, IDataSourceWidget
	{
		public static readonly ScriptEvent OnCheckbox = new ScriptEvent("checkbox", "node");

		public static readonly ScriptEvent OnFormatter = new ScriptEvent("formatter", "node");

		public static readonly ScriptEvent OnFilter = new ScriptEvent("filter", "q,node");

		public static readonly ScriptEvent OnLoadFilter = new ScriptEvent("loadFilter", "data,parent");

		public static readonly ScriptEvent OnClick = new ScriptEvent("onClick", "node");

		public static readonly ScriptEvent OnDblClick = new ScriptEvent("onDblClick", "node");

		public static readonly ScriptEvent OnBeforeExpand = new ScriptEvent("onBeforeExpand", "node");

		public static readonly ScriptEvent OnExpand = new ScriptEvent("onExpand", "node");

		public static readonly ScriptEvent OnBeforeCollapse = new ScriptEvent("onBeforeCollapse", "node");

		public static readonly ScriptEvent OnCollapse = new ScriptEvent("onCollapse", "node");

		public static readonly ScriptEvent OnBeforeCheck = new ScriptEvent("onBeforeCheck", "node,checked");

		public static readonly ScriptEvent OnCheck = new ScriptEvent("onCheck", "node,checked");

		public static readonly ScriptEvent OnBeforeSelect = new ScriptEvent("onBeforeSelect", "node");

		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "node");

		public static readonly ScriptEvent OnContextMenu = new ScriptEvent("onContextMenu", "e,node");

		public static readonly ScriptEvent OnBeforeDrag = new ScriptEvent("onBeforeDrag", "node");

		public static readonly ScriptEvent OnStartDrag = new ScriptEvent("onStartDrag", "node");

		public static readonly ScriptEvent OnStopDrag = new ScriptEvent("onStopDrag", "node");

		public static readonly ScriptEvent OnDragEnter = new ScriptEvent("onDragEnter", "target,source");

		public static readonly ScriptEvent OnDragOver = new ScriptEvent("onDragOver", "target,source");

		public static readonly ScriptEvent OnDragLeave = new ScriptEvent("onDragLeave", "target,source");

		public static readonly ScriptEvent OnBeforeDrop = new ScriptEvent("onBeforeDrop", "target,source,point");

		public static readonly ScriptEvent OnDrop = new ScriptEvent("onDrop", "target,source,point");

		public static readonly ScriptEvent OnBeforeEdit = new ScriptEvent("onBeforeEdit", "node");

		public static readonly ScriptEvent OnAfterEdit = new ScriptEvent("onAfterEdit", "node");

		public static readonly ScriptEvent OnCancelEdit = new ScriptEvent("onCancelEdit", "node");

		public bool? Animate { get; set; }

		public bool? Checkbox { get; set; }

		public bool? CascadeCheck { get; set; }

		public bool? OnlyLeafCheck { get; set; }

		public bool? Lines { get; set; }

		public bool? Dnd { get; set; }

		public StringBuilder Data { get; set; }

		public string EditUrl { get; set; }

		public int? EditWidth { get; set; }

		public int? EditHeight { get; set; }

		public string DelApi { get; set; }

		public string DelDs { get; set; }

		public DataSource DataSource { get; set; }

		public Tree(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "tree";
			base.TagName = "ul";
			Data = new StringBuilder();
			DataSource = new DataSource(this);
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TreeHtmlBuilder(this);
		}
	}
}
