using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataGridEventBuilder : PanelEventBuilder
	{
		public DataGridEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public DataGridEventBuilder OnRowStyle(string handler)
		{
			Handler(DataGrid.OnRowStyle.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnClickRow(string handler)
		{
			Handler(DataGrid.OnClickRow.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnDblClickRow(string handler)
		{
			Handler(DataGrid.OnDblClickRow.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnClickCell(string handler)
		{
			Handler(DataGrid.OnClickCell.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnDblClickCell(string handler)
		{
			Handler(DataGrid.OnDblClickCell.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeSortColumn(string handler)
		{
			Handler(DataGrid.OnBeforeSortColumn.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnSortColumn(string handler)
		{
			Handler(DataGrid.OnSortColumn.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnResizeColumn(string handler)
		{
			Handler(DataGrid.OnResizeColumn.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeSelect(string handler)
		{
			Handler(DataGrid.OnBeforeSelect.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnSelect(string handler)
		{
			Handler(DataGrid.OnSelect.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeUnselect(string handler)
		{
			Handler(DataGrid.OnBeforeUnselect.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnUnselect(string handler)
		{
			Handler(DataGrid.OnUnselect.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnSelectAll(string handler)
		{
			Handler(DataGrid.OnSelectAll.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnUnselectAll(string handler)
		{
			Handler(DataGrid.OnUnselectAll.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeCheck(string handler)
		{
			Handler(DataGrid.OnBeforeCheck.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnCheck(string handler)
		{
			Handler(DataGrid.OnCheck.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeUncheck(string handler)
		{
			Handler(DataGrid.OnBeforeUncheck.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnUncheck(string handler)
		{
			Handler(DataGrid.OnUncheck.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnCheckAll(string handler)
		{
			Handler(DataGrid.OnCheckAll.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnUncheckAll(string handler)
		{
			Handler(DataGrid.OnUncheckAll.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeforeEdit(string handler)
		{
			Handler(DataGrid.OnBeforeEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnBeginEdit(string handler)
		{
			Handler(DataGrid.OnBeginEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnEndEdit(string handler)
		{
			Handler(DataGrid.OnEndEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnAfterEdit(string handler)
		{
			Handler(DataGrid.OnAfterEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnCancelEdit(string handler)
		{
			Handler(DataGrid.OnCancelEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnHeaderContextMenu(string handler)
		{
			Handler(DataGrid.OnHeaderContextMenu.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnRowContextMenu(string handler)
		{
			Handler(DataGrid.OnRowContextMenu.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnEdit(string handler)
		{
			Handler(DataGrid.OnEdit.EventName, handler);
			return this;
		}

		public DataGridEventBuilder OnDelete(string handler)
		{
			Handler(DataGrid.OnDelete.EventName, handler);
			return this;
		}
	}
}
