using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TreeEventBuilder : ValidateBoxEventBuilder
	{
		public TreeEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TreeEventBuilder OnCheckbox(string handler)
		{
			Handler(Tree.OnCheckbox.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnFormatter(string handler)
		{
			Handler(Tree.OnFormatter.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnFilter(string handler)
		{
			Handler(Tree.OnFilter.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnLoadFilter(string handler)
		{
			Handler(Tree.OnLoadFilter.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnClick(string handler)
		{
			Handler(Tree.OnClick.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnDblClick(string handler)
		{
			Handler(Tree.OnDblClick.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeExpand(string handler)
		{
			Handler(Tree.OnBeforeExpand.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnExpand(string handler)
		{
			Handler(Tree.OnExpand.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeCollapse(string handler)
		{
			Handler(Tree.OnBeforeCollapse.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnCollapse(string handler)
		{
			Handler(Tree.OnCollapse.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeCheck(string handler)
		{
			Handler(Tree.OnBeforeCheck.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnCheck(string handler)
		{
			Handler(Tree.OnCheck.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeSelect(string handler)
		{
			Handler(Tree.OnBeforeSelect.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnSelect(string handler)
		{
			Handler(Tree.OnSelect.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnContextMenu(string handler)
		{
			Handler(Tree.OnContextMenu.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeDrag(string handler)
		{
			Handler(Tree.OnBeforeDrag.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnStartDrag(string handler)
		{
			Handler(Tree.OnStartDrag.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnStopDrag(string handler)
		{
			Handler(Tree.OnStopDrag.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnDragEnter(string handler)
		{
			Handler(Tree.OnDragEnter.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnDragOver(string handler)
		{
			Handler(Tree.OnDragOver.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnDragLeave(string handler)
		{
			Handler(Tree.OnDragLeave.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeDrop(string handler)
		{
			Handler(Tree.OnBeforeDrop.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnDrop(string handler)
		{
			Handler(Tree.OnDrop.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnBeforeEdit(string handler)
		{
			Handler(Tree.OnBeforeEdit.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnAfterEdit(string handler)
		{
			Handler(Tree.OnAfterEdit.EventName, handler);
			return this;
		}

		public TreeEventBuilder OnCancelEdit(string handler)
		{
			Handler(Tree.OnCancelEdit.EventName, handler);
			return this;
		}
	}
}
