using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataGridBuilder : DataGridBuilder<DataGrid, DataGridBuilder>
	{
		public DataGridBuilder(DataGrid component)
			: base(component)
		{
		}
	}
	public abstract class DataGridBuilder<Widget, Builder> : PanelBuilder<Widget, Builder> where Widget : DataGrid where Builder : DataGridBuilder<Widget, Builder>
	{
		public DataGridBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder FitColumns(bool fitColumns = true)
		{
			base.Component.FitColumns = fitColumns;
			return this as Builder;
		}

		public virtual Builder ResizeHandle(Resize resize)
		{
			base.Component.ResizeHandle = resize;
			return this as Builder;
		}

		public virtual Builder ResizeEdge(int resizeEdge)
		{
			base.Component.ResizeEdge = resizeEdge;
			return this as Builder;
		}

		public virtual Builder AutoRowHeight(bool autoRowHeight = true)
		{
			base.Component.AutoRowHeight = autoRowHeight;
			return this as Builder;
		}

		public virtual Builder Striped(bool striped = true)
		{
			base.Component.Striped = striped;
			return this as Builder;
		}

		public virtual Builder Nowrap(bool nowrap = true)
		{
			base.Component.Nowrap = nowrap;
			return this as Builder;
		}

		public virtual Builder IdField(string idField)
		{
			base.Component.IdField = idField;
			return this as Builder;
		}

		public virtual Builder LoadMsg(string loadMsg)
		{
			base.Component.LoadMsg = loadMsg;
			return this as Builder;
		}

		public virtual Builder EmptyMsg(string emptyMsg)
		{
			base.Component.EmptyMsg = emptyMsg;
			return this as Builder;
		}

		public virtual Builder Pagination(bool pagination = true)
		{
			base.Component.Pagination = pagination;
			if (!pagination)
			{
				base.Component.DataSource.RouteValues["rows"] = 0;
			}
			return this as Builder;
		}

		public virtual Builder Rownumbers(bool rownumbers = true)
		{
			base.Component.Rownumbers = rownumbers;
			return this as Builder;
		}

		public virtual Builder SingleSelect(bool singleSelect = true)
		{
			base.Component.SingleSelect = singleSelect;
			return this as Builder;
		}

		public virtual Builder CtrlSelect(bool ctrlSelect = true)
		{
			base.Component.CtrlSelect = ctrlSelect;
			return this as Builder;
		}

		public virtual Builder CheckOnSelect(bool checkOnSelect = true)
		{
			base.Component.CheckOnSelect = checkOnSelect;
			return this as Builder;
		}

		public virtual Builder SelectOnCheck(bool selectOnCheck = true)
		{
			base.Component.SelectOnCheck = selectOnCheck;
			return this as Builder;
		}

		public virtual Builder ScrollOnSelect(bool scrollOnSelect = true)
		{
			base.Component.ScrollOnSelect = scrollOnSelect;
			return this as Builder;
		}

		public virtual Builder PagePosition(Position position)
		{
			base.Component.PagePosition = position;
			return this as Builder;
		}

		public virtual Builder PageNumber(int pageNumber)
		{
			base.Component.PageNumber = pageNumber;
			return this as Builder;
		}

		public virtual Builder PageSize(int pageSize)
		{
			base.Component.PageSize = pageSize;
			return this as Builder;
		}

		public virtual Builder PageList(Action<IList<int>> pageListAction)
		{
			pageListAction(base.Component.PageList);
			return this as Builder;
		}

		public virtual Builder SortName(string sortName)
		{
			base.Component.SortName = sortName;
			return this as Builder;
		}

		public virtual Builder SortOrder(Order order)
		{
			base.Component.SortOrder = order;
			return this as Builder;
		}

		public virtual Builder MultiSort(bool multiSort = true)
		{
			base.Component.MultiSort = multiSort;
			return this as Builder;
		}

		public virtual Builder RemoteSort(bool remoteSort = true)
		{
			base.Component.RemoteSort = remoteSort;
			return this as Builder;
		}

		public virtual Builder ShowHeader(bool showHeader = true)
		{
			base.Component.ShowHeader = showHeader;
			return this as Builder;
		}

		public virtual Builder ShowFooter(bool showFooter = true)
		{
			base.Component.ShowFooter = showFooter;
			return this as Builder;
		}

		public virtual Builder ScrollbarSize(int scrollbarSize)
		{
			base.Component.ScrollbarSize = scrollbarSize;
			return this as Builder;
		}

		public virtual Builder RownumberWidth(int rownumberWidth)
		{
			base.Component.RownumberWidth = rownumberWidth;
			return this as Builder;
		}

		public virtual Builder EditorHeight(int editorHeight)
		{
			base.Component.EditorHeight = editorHeight;
			return this as Builder;
		}

		public virtual Builder CheckBox(bool checkBox = true)
		{
			base.Component.CheckBox = checkBox;
			return this as Builder;
		}

		public virtual Builder Edit(string url, int width = 0, int height = 0)
		{
			base.Component.EditUrl = url;
			if (width > 0)
			{
				base.Component.EditWidth = width;
			}
			if (height > 0)
			{
				base.Component.EditHeight = height;
			}
			return this as Builder;
		}

		public virtual Builder Delete(string api, string ds = null, string delTip = null)
		{
			base.Component.DelApi = api;
			if (ds.HasValue())
			{
				base.Component.DelDs = ds;
			}
			if (delTip.HasValue())
			{
				base.Component.DelTip = delTip;
			}
			return this as Builder;
		}

		public virtual Builder DataSource(string ds)
		{
			base.Component.DelDs = ds;
			base.Component.DataSource.RouteValues["ds"] = ds + "_grid";
			return this as Builder;
		}

		public virtual Builder Export(bool flag = true)
		{
			base.Component.Export = flag;
			return this as Builder;
		}

		public Builder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(base.Component.DataSource).Controller("crud").Action("grid"));
			return this as Builder;
		}

		public Builder Columns(Action<RowItemsBuilder<DataGridColumn, DataGridColumnBuilder>> addAction)
		{
			addAction(new RowItemsBuilder<DataGridColumn, DataGridColumnBuilder>(base.Component.Columns, () => new DataGridColumn(base.Component.Ace)
			{
				Grid = base.Component
			}, (DataGridColumn col) => new DataGridColumnBuilder(col)));
			return this as Builder;
		}

		public Builder FrozenColumns(Action<ItemsBuilder<DataGridColumn, DataGridColumnBuilder>> addAction)
		{
			addAction(new ItemsBuilder<DataGridColumn, DataGridColumnBuilder>(base.Component.FrozenColumns, () => new DataGridColumn(base.Component.Ace)
			{
				Grid = base.Component
			}, (DataGridColumn col) => new DataGridColumnBuilder(col)));
			return this as Builder;
		}

		public Builder Toolbar(Action<IList<LinkButton>> toolbarAction)
		{
			toolbarAction(base.Component.Toolbar);
			return this as Builder;
		}

		public Builder Events(Action<DataGridEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DataGridEventBuilder(base.Component.Events));
			return this as Builder;
		}
	}
}
