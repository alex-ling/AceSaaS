using Acesoft.Web.UI.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TreeHtmlBuilder<Tree, TreeNode> : EasyUIHtmlBuilder<Tree> where Tree : TreeWidgetBase<TreeNode> where TreeNode : TreeNodeWidgetBase<Tree, TreeNode>
	{
		public TreeHtmlBuilder(Tree component, string tagName)
			: base(component, tagName)
		{
		}

		public override IHtmlNode Build()
		{
			IHtmlNode html = base.Build();
			if (Component.Nodes.Any())
			{
				Component.Nodes.Each(item =>
				{
					item.HtmlBuilder.Build().AppendTo(html);
				});
			}
			return html;
		}
	}
	public class TreeHtmlBuilder : TreeHtmlBuilder<Tree, TreeNode>
	{
		public TreeHtmlBuilder(Tree component)
			: base(component, "ul")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Animate.HasValue)
			{
				base.Options["animate"] = base.Component.Animate;
			}
			if (base.Component.Checkbox.HasValue)
			{
				base.Options["checkbox"] = base.Component.Checkbox;
			}
			if (base.Component.CascadeCheck.HasValue)
			{
				base.Options["cascadeCheck"] = base.Component.CascadeCheck;
			}
			if (base.Component.OnlyLeafCheck.HasValue)
			{
				base.Options["onlyLeafCheck"] = base.Component.OnlyLeafCheck;
			}
			if (base.Component.Lines.HasValue)
			{
				base.Options["lines"] = base.Component.Lines;
			}
			if (base.Component.Dnd.HasValue)
			{
				base.Options["dnd"] = base.Component.Dnd;
			}
			if (base.Component.Data.Length > 0)
			{
				base.Options["data"] = base.Component.Data;
			}
			if (base.Component.EditUrl.HasValue())
			{
				base.Options["editUrl"] = base.Component.EditUrl;
			}
			if (base.Component.EditWidth.HasValue)
			{
				base.Options["w"] = base.Component.EditWidth;
			}
			if (base.Component.EditHeight.HasValue)
			{
				base.Options["h"] = base.Component.EditHeight;
			}
			if (base.Component.DelApi.HasValue())
			{
				base.Options["delApi"] = base.Component.DelApi;
			}
			if (base.Component.DelDs.HasValue())
			{
				base.Options["delDs"] = base.Component.DelDs;
			}
		}
	}
}
