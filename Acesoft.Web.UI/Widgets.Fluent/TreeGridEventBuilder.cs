using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TreeGridEventBuilder : DataGridEventBuilder
	{
		public TreeGridEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TreeGridEventBuilder OnCheckbox(string handler)
		{
			Handler(TreeGrid.OnCheckbox.EventName, handler);
			return this;
		}

		public TreeGridEventBuilder OnBeforeCheckNode(string handler)
		{
			Handler(TreeGrid.OnBeforeCheckNode.EventName, handler);
			return this;
		}

		public TreeGridEventBuilder OnCheckNode(string handler)
		{
			Handler(TreeGrid.OnCheckNode.EventName, handler);
			return this;
		}

		public TreeGridEventBuilder OnContextMenu(string handler)
		{
			Handler(TreeGrid.OnContextMenu.EventName, handler);
			return this;
		}
	}
}
