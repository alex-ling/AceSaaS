using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class TreeNode : TreeNodeWidgetBase<Tree, TreeNode>
	{
		public string Url
		{
			get;
			set;
		}

		public TreeNode(WidgetFactory ace, Tree tree)
			: base(ace, tree)
		{
			base.Widget = null;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TreeNodeHtmlBuilder(this);
		}
	}
}
