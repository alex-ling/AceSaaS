using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TreeGridBuilder : DataGridBuilder<TreeGrid, TreeGridBuilder>
	{
		public TreeGridBuilder(TreeGrid component)
			: base(component)
		{
		}

		public virtual TreeGridBuilder TreeField(string treeField)
		{
			base.Component.TreeField = treeField;
			return this;
		}

		public virtual TreeGridBuilder Animate(bool animate = true)
		{
			base.Component.Animate = animate;
			return this;
		}

		public virtual TreeGridBuilder CascadeCheck(bool cascadeCheck = true)
		{
			base.Component.CascadeCheck = cascadeCheck;
			return this;
		}

		public virtual TreeGridBuilder OnlyLeafCheck(bool onlyLeafCheck = true)
		{
			base.Component.OnlyLeafCheck = onlyLeafCheck;
			return this;
		}

		public virtual TreeGridBuilder Lines(bool lines = true)
		{
			base.Component.Lines = lines;
			return this;
		}

		public TreeGridBuilder Events(Action<TreeGridEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TreeGridEventBuilder(base.Component.Events));
			return this;
		}
	}
}
