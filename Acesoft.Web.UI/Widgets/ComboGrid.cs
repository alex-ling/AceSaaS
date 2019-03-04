using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class ComboGrid : Combo
	{
		public string TextField
		{
			get;
			set;
		}

		public Filter? Mode
		{
			get;
			set;
		}

		public IList<IList<DataGridColumn>> Columns
		{
			get;
			set;
		}

		public IList<DataGridColumn> FrozenColumns
		{
			get;
			set;
		}

		public IList<LinkButton> Toolbar
		{
			get;
			set;
		}

		public bool? FitColumns
		{
			get;
			set;
		}

		public Resize? ResizeHandle
		{
			get;
			set;
		}

		public int? ResizeEdge
		{
			get;
			set;
		}

		public bool? AutoRowHeight
		{
			get;
			set;
		}

		public bool? Striped
		{
			get;
			set;
		}

		public bool? Nowrap
		{
			get;
			set;
		}

		public string IdField
		{
			get;
			set;
		}

		public string LoadMsg
		{
			get;
			set;
		}

		public string EmptyMsg
		{
			get;
			set;
		}

		public bool? Pagination
		{
			get;
			set;
		}

		public bool? Rownumbers
		{
			get;
			set;
		}

		public bool? SingleSelect
		{
			get;
			set;
		}

		public bool? CtrlSelect
		{
			get;
			set;
		}

		public bool? CheckOnSelect
		{
			get;
			set;
		}

		public bool? SelectOnCheck
		{
			get;
			set;
		}

		public bool? ScrollOnSelect
		{
			get;
			set;
		}

		public Position? PagePosition
		{
			get;
			set;
		}

		public int? PageNumber
		{
			get;
			set;
		}

		public int? PageSize
		{
			get;
			set;
		}

		public IList<int> PageList
		{
			get;
			set;
		}

		public string SortName
		{
			get;
			set;
		}

		public Order? SortOrder
		{
			get;
			set;
		}

		public bool? MultiSort
		{
			get;
			set;
		}

		public bool? RemoteSort
		{
			get;
			set;
		}

		public bool? ShowHeader
		{
			get;
			set;
		}

		public bool? ShowFooter
		{
			get;
			set;
		}

		public int? ScrollbarSize
		{
			get;
			set;
		}

		public int? RownumberWidth
		{
			get;
			set;
		}

		public int? EditorHeight
		{
			get;
			set;
		}

		public ComboGrid(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combogrid";
			Columns = new List<IList<DataGridColumn>>();
			FrozenColumns = new List<DataGridColumn>();
			Toolbar = new List<LinkButton>();
			PageList = new List<int>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ComboGridHtmlBuilder<ComboGrid>(this);
		}
	}
}
