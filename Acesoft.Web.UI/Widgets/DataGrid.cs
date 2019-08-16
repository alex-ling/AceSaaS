using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class DataGrid : Panel, IDataSourceWidget
	{
		public static readonly ScriptEvent OnRowStyle = new ScriptEvent("rowStyle", "index,row");

		public static readonly ScriptEvent OnClickRow = new ScriptEvent("onClickRow", "index,row");

		public static readonly ScriptEvent OnDblClickRow = new ScriptEvent("onDblClickRow", "index,row");

		public static readonly ScriptEvent OnClickCell = new ScriptEvent("onClickCell", "index,field,value");

		public static readonly ScriptEvent OnDblClickCell = new ScriptEvent("onDblClickCell", "index,field,value");

		public static readonly ScriptEvent OnBeforeSortColumn = new ScriptEvent("onBeforeSortColumn", "sort,order");

		public static readonly ScriptEvent OnSortColumn = new ScriptEvent("onSortColumn", "sort,order");

		public static readonly ScriptEvent OnResizeColumn = new ScriptEvent("onResizeColumn", "field,width");

		public static readonly ScriptEvent OnBeforeSelect = new ScriptEvent("onBeforeSelect", "");

		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "index,row");

		public static readonly ScriptEvent OnBeforeUnselect = new ScriptEvent("onBeforeSelect", "");

		public static readonly ScriptEvent OnUnselect = new ScriptEvent("onUnselect", "index,row");

		public static readonly ScriptEvent OnSelectAll = new ScriptEvent("onSelectAll", "rows");

		public static readonly ScriptEvent OnUnselectAll = new ScriptEvent("onUnselectAll", "rows");

		public static readonly ScriptEvent OnBeforeCheck = new ScriptEvent("onBeforeCheck", "index,row");

		public static readonly ScriptEvent OnCheck = new ScriptEvent("onCheck", "index,row");

		public static readonly ScriptEvent OnBeforeUncheck = new ScriptEvent("onBeforeUncheck", "index,row");

		public static readonly ScriptEvent OnUncheck = new ScriptEvent("onUncheck", "index,row");

		public static readonly ScriptEvent OnCheckAll = new ScriptEvent("onCheckAll", "rows");

		public static readonly ScriptEvent OnUncheckAll = new ScriptEvent("onUncheckAll", "rows");

		public static readonly ScriptEvent OnBeforeEdit = new ScriptEvent("onBeforeEdit", "index,row");

		public static readonly ScriptEvent OnBeginEdit = new ScriptEvent("onBeginEdit", "index,row");

		public static readonly ScriptEvent OnEndEdit = new ScriptEvent("onEndEdit", "index,row,changes");

		public static readonly ScriptEvent OnAfterEdit = new ScriptEvent("onAfterEdit", "index,row,changes");

		public static readonly ScriptEvent OnCancelEdit = new ScriptEvent("onCancelEdit", "index,row");

		public static readonly ScriptEvent OnHeaderContextMenu = new ScriptEvent("onHeaderContextMenu", "e,field");

		public static readonly ScriptEvent OnRowContextMenu = new ScriptEvent("onRowContextMenu", "e,index,row");

		public static readonly ScriptEvent OnEdit = new ScriptEvent("onEdit", "id");

		public static readonly ScriptEvent OnDelete = new ScriptEvent("onDelete", "id");

		public IList<IList<DataGridColumn>> Columns { get; set; }

		public IList<IList<DataGridColumn>> FrozenColumns { get; set; }

		public IList<LinkButton> Toolbar { get; set; }

		public bool? FitColumns { get; set; }

		public Resize? ResizeHandle { get; set; }

		public int? ResizeEdge { get; set; }

		public bool? AutoRowHeight { get; set; }

		public bool? Striped { get; set; }

		public bool? Nowrap { get; set; }

		public string IdField { get; set; }

		public string LoadMsg { get; set; }

		public string EmptyMsg { get; set; }

		public bool? Pagination { get; set; }

		public bool? Rownumbers { get; set; }

		public bool? SingleSelect { get; set; }

		public bool? CtrlSelect { get; set; }

		public bool? CheckOnSelect { get; set; }

		public bool? SelectOnCheck { get; set; }

		public bool? ScrollOnSelect { get; set; }

		public Position? PagePosition { get; set; }

		public int? PageNumber { get; set; }

		public int? PageSize { get; set; }

		public IList<int> PageList { get; set; }

		public string SortName { get; set; }

		public Order? SortOrder { get; set; }

		public bool? MultiSort { get; set; }

		public bool? RemoteSort { get; set; }

		public bool? ShowHeader { get; set; }

		public bool? ShowFooter { get; set; }

		public int? ScrollbarSize { get; set; }

		public int? RownumberWidth { get; set; }

		public int? EditorHeight { get; set; }

		public bool CheckBox { get; set; }

		public string EditUrl { get; set; }

		public int? EditWidth { get; set; }

		public int? EditHeight { get; set; }

		public string DelApi { get; set; }

		public string DelDs { get; set; }

		public string DelTip { get; set; }

		public bool? Export { get; set; }

        public bool Sortable { get; set; }

		public DataSource DataSource { get; set; }

		public DataGrid(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "datagrid";
			Columns = new List<IList<DataGridColumn>>();
			FrozenColumns = new List<IList<DataGridColumn>>();
			Toolbar = new List<LinkButton>();
			PageList = new List<int>();
			DataSource = new DataSource(this);
            Sortable = true;
			FitColumns = true;
			Striped = true;
			base.Fit = true;
			Pagination = true;
			Rownumbers = true;
			SelectOnCheck = false;
			CheckOnSelect = false;
			SingleSelect = true;
			PageSize = 20;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DataGridHtmlBuilder<DataGrid>(this, "table");
		}
	}
}
