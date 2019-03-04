using System;

namespace Acesoft.Web.UI.Builder
{
	public class TreeNodeBuilder<Tree, Node, Builder> : WidgetBuilder<Node, Builder> where Tree : TreeWidgetBase<Node> where Node : TreeNodeWidgetBase<Tree, Node> where Builder : TreeNodeBuilder<Tree, Node, Builder>
	{
		public TreeNodeBuilder(Node component)
			: base(component)
		{
		}

		public virtual Builder Text(string text)
		{
			base.Component.Text = text;
			return this as Builder;
		}

		protected virtual Builder Nodes(Action<ItemsBuilder<Node, Builder>> addAction, Func<Node> itemCreator, Func<Node, Builder> builderCreator)
		{
			addAction(new ItemsBuilder<Node, Builder>(base.Component.Nodes, itemCreator, builderCreator));
			return this as Builder;
		}
	}
}
