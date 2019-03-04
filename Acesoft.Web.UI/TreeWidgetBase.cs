using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public abstract class TreeWidgetBase<Node> : WidgetBase, ITreeContainer<Node> where Node : TreeWidgetBase<Node>
	{
		public IList<Node> Nodes
		{
			get;
			private set;
		}

		public TreeWidgetBase(WidgetFactory ace)
			: base(ace)
		{
			Nodes = new List<Node>();
		}
	}
}
