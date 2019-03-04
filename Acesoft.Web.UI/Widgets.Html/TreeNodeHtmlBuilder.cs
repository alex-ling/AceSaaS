using Acesoft.Web.UI.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TreeNodeHtmlBuilder<Tree, Node> : EasyUIHtmlBuilder<Node> 
        where Tree : TreeWidgetBase<Node> 
        where Node : TreeNodeWidgetBase<Tree, Node>
	{
		public TreeNodeHtmlBuilder(Node component, string tagName)
			: base(component, tagName)
		{
		}

		public override IHtmlNode Build()
		{
			var htmlNode = base.Build();
			BuildItem().AppendTo(htmlNode);
			if (Component.Nodes.Any())
			{
				string tagName = base.Component.Tree.TagName;
				var sub = new HtmlNode(tagName).AppendTo(htmlNode);
				Component.Nodes.Each(item =>
				{
					item.HtmlBuilder.Build().AppendTo(sub);
				});
			}
			return htmlNode;
		}

		public virtual IHtmlNode BuildItem()
		{
			return new LiteralNode("<span>" + Component.Text + "</span>");
		}
	}
	public class TreeNodeHtmlBuilder : TreeNodeHtmlBuilder<Tree, TreeNode>
	{
		public TreeNodeHtmlBuilder(TreeNode component)
			: base(component, "li")
		{
		}

		public override IHtmlNode BuildItem()
		{
			if (Component.Url.HasValue())
			{
				return new HtmlNode("a")
                    .Text(base.Component.Text)
                    .Attribute("href", Component.Url)
                    .Attribute("target", "_blank");
			}
			return base.BuildItem();
		}
	}
}
