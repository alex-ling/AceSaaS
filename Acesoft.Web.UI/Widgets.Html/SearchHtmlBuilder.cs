using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Html;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class SearchHtmlBuilder : FormHtmlBuilder<Search>
	{
		public SearchHtmlBuilder(Search component)
			: base(component)
		{
		}

		protected override void BuildContent(IContentWidget content, IHtmlNode html)
		{
			IHtmlNode html2 = new HtmlNode("div").AddClass("fl").AppendTo(html);
			base.BuildContent(content, html2);
			if (base.Component.Button.HasValue && base.Component.Button.Value)
			{
				base.Component.Ace.Button().Text("查询").IconCls("fa fa-search")
					.Css("ml5")
					.Click("AX.gridSrh('#" + base.Component.For + "','#" + base.Component.Id + "')")
					.AppendTo(html2);
				base.Component.Ace.Button().Text("清空").IconCls("fa fa-remove")
					.Click("AX.gridClr('#" + base.Component.For + "','#" + base.Component.Id + "')")
					.AppendTo(html2);
			}
			if (base.Component.Tools.Any())
			{
				IHtmlNode right = new HtmlNode("div").AddClass("fr").AppendTo(html);
				base.Component.Tools.Each(delegate(IHtmlContent tool)
				{
					new LiteralNode(tool.ToHtml()).AppendTo(right);
				});
			}
		}
	}
}
