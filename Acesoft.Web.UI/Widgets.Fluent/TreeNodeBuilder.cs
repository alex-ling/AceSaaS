using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TreeNodeBuilder : TreeNodeBuilder<Tree, TreeNode, TreeNodeBuilder>
	{
		public TreeNodeBuilder(TreeNode component)
			: base(component)
		{
		}

		public virtual TreeNodeBuilder Url(string url)
		{
			base.Component.Url = url;
			return this;
		}

		public TreeNodeBuilder Nodes(Action<ItemsBuilder<TreeNode, TreeNodeBuilder>> addAction)
		{
			return Nodes(addAction, () => new TreeNode(base.Component.Ace, base.Component.Tree), (TreeNode item) => new TreeNodeBuilder(item));
		}

		public TreeNodeBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
