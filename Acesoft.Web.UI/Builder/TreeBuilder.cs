using System;

namespace Acesoft.Web.UI.Builder
{
	public class TreeBuilder<Tree, Node, Builder> : WidgetBuilder<Tree, Builder> where Tree : TreeWidgetBase<Node> where Node : TreeWidgetBase<Node> where Builder : TreeBuilder<Tree, Node, Builder>
	{
		public TreeBuilder(Tree component)
			: base(component)
		{
		}

		protected virtual Builder Nodes<ItemBuilder>(Action<ItemsBuilder<Node, ItemBuilder>> addAction, Func<Node> itemCreator, Func<Node, ItemBuilder> builderCreator) where ItemBuilder : WidgetBuilder<Node, ItemBuilder>
		{
			addAction(new ItemsBuilder<Node, ItemBuilder>(((TreeWidgetBase<Node>)base.Component).Nodes, itemCreator, builderCreator));
			return this as Builder;
		}
	}
}
