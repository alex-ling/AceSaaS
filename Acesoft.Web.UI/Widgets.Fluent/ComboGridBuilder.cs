using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboGridBuilder : ComboBuilder<ComboGrid, ComboGridBuilder>
	{
		public ComboGridBuilder(ComboGrid component)
			: base(component)
		{
		}

		public ComboGridBuilder TextField(string textField)
		{
			base.Component.TextField = textField;
			return this;
		}

		public ComboGridBuilder Mode(Filter mode)
		{
			base.Component.Mode = mode;
			return this;
		}

		public virtual ComboGridBuilder FitColumns(bool fitColumns = true)
		{
			base.Component.FitColumns = fitColumns;
			return this;
		}

		public virtual ComboGridBuilder ResizeHandle(Resize resize)
		{
			base.Component.ResizeHandle = resize;
			return this;
		}

		public virtual ComboGridBuilder ResizeEdge(int resizeEdge)
		{
			base.Component.ResizeEdge = resizeEdge;
			return this;
		}

		public virtual ComboGridBuilder AutoRowHeight(bool autoRowHeight = true)
		{
			base.Component.AutoRowHeight = autoRowHeight;
			return this;
		}

		public virtual ComboGridBuilder Striped(bool striped = true)
		{
			base.Component.Striped = striped;
			return this;
		}

		public virtual ComboGridBuilder Nowrap(bool nowrap = true)
		{
			base.Component.Nowrap = nowrap;
			return this;
		}

		public virtual ComboGridBuilder IdField(string idField)
		{
			base.Component.IdField = idField;
			return this;
		}

		public virtual ComboGridBuilder LoadMsg(string loadMsg)
		{
			base.Component.LoadMsg = loadMsg;
			return this;
		}

		public virtual ComboGridBuilder EmptyMsg(string emptyMsg)
		{
			base.Component.EmptyMsg = emptyMsg;
			return this;
		}

		public virtual ComboGridBuilder Pagination(bool pagination = true)
		{
			base.Component.Pagination = pagination;
			return this;
		}

		public virtual ComboGridBuilder Rownumbers(bool rownumbers = true)
		{
			base.Component.Rownumbers = rownumbers;
			return this;
		}

		public virtual ComboGridBuilder SingleSelect(bool singleSelect = true)
		{
			base.Component.SingleSelect = singleSelect;
			return this;
		}

		public virtual ComboGridBuilder CtrlSelect(bool ctrlSelect = true)
		{
			base.Component.CtrlSelect = ctrlSelect;
			return this;
		}

		public virtual ComboGridBuilder CheckOnSelect(bool checkOnSelect = true)
		{
			base.Component.CheckOnSelect = checkOnSelect;
			return this;
		}

		public virtual ComboGridBuilder SelectOnCheck(bool selectOnCheck = true)
		{
			base.Component.SelectOnCheck = selectOnCheck;
			return this;
		}

		public virtual ComboGridBuilder ScrollOnSelect(bool scrollOnSelect = true)
		{
			base.Component.ScrollOnSelect = scrollOnSelect;
			return this;
		}

		public virtual ComboGridBuilder PagePosition(Position position)
		{
			base.Component.PagePosition = position;
			return this;
		}

		public virtual ComboGridBuilder PageNumber(int pageNumber)
		{
			base.Component.PageNumber = pageNumber;
			return this;
		}

		public virtual ComboGridBuilder PageSize(int pageSize)
		{
			base.Component.PageSize = pageSize;
			return this;
		}

		public virtual ComboGridBuilder PageList(Action<IList<int>> pageListAction)
		{
			pageListAction(base.Component.PageList);
			return this;
		}

		public virtual ComboGridBuilder SortName(string sortName)
		{
			base.Component.SortName = sortName;
			return this;
		}

		public virtual ComboGridBuilder SortOrder(Order order)
		{
			base.Component.SortOrder = order;
			return this;
		}

		public virtual ComboGridBuilder MultiSort(bool multiSort = true)
		{
			base.Component.MultiSort = multiSort;
			return this;
		}

		public virtual ComboGridBuilder RemoteSort(bool remoteSort = true)
		{
			base.Component.RemoteSort = remoteSort;
			return this;
		}

		public virtual ComboGridBuilder ShowHeader(bool showHeader = true)
		{
			base.Component.ShowHeader = showHeader;
			return this;
		}

		public virtual ComboGridBuilder ShowFooter(bool showFooter = true)
		{
			base.Component.ShowFooter = showFooter;
			return this;
		}

		public virtual ComboGridBuilder ScrollbarSize(int scrollbarSize)
		{
			base.Component.ScrollbarSize = scrollbarSize;
			return this;
		}

		public virtual ComboGridBuilder RownumberWidth(int rownumberWidth)
		{
			base.Component.RownumberWidth = rownumberWidth;
			return this;
		}

		public virtual ComboGridBuilder EditorHeight(int editorHeight)
		{
			base.Component.EditorHeight = editorHeight;
			return this;
		}

		public ComboGridBuilder Columns(Action<RowItemsBuilder<DataGridColumn, DataGridColumnBuilder>> addAction)
		{
			addAction(new RowItemsBuilder<DataGridColumn, DataGridColumnBuilder>(base.Component.Columns, () => new DataGridColumn(base.Component.Ace), (DataGridColumn col) => new DataGridColumnBuilder(col)));
			return this;
		}

		public ComboGridBuilder FrozenColumns(Action<ItemsBuilder<DataGridColumn, DataGridColumnBuilder>> addAction)
		{
			addAction(new ItemsBuilder<DataGridColumn, DataGridColumnBuilder>(base.Component.FrozenColumns, () => new DataGridColumn(base.Component.Ace), (DataGridColumn col) => new DataGridColumnBuilder(col)));
			return this;
		}

		public ComboGridBuilder Toolbar(Action<IList<LinkButton>> toolbarAction)
		{
			toolbarAction(base.Component.Toolbar);
			return this;
		}

		public ComboGridBuilder Events(Action<ComboEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ComboEventBuilder(base.Component.Events));
			return this;
		}
	}
}
