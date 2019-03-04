using Acesoft.Web.UI.Html;
using System;

namespace Acesoft.Web.UI
{
	public abstract class TreeNodeWidgetBase<T, Node> : TreeWidgetBase<Node> 
        where T : TreeWidgetBase<Node> 
        where Node : TreeWidgetBase<Node>
	{
		public T Tree { get; }
		public string Text { get; set; }

		public TreeNodeWidgetBase(WidgetFactory ace, T tree)
			: base(ace)
		{
			base.Widget = null;
			Tree = tree;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			throw new NotImplementedException();
		}
	}
}
