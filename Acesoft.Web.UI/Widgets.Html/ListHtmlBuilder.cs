using Acesoft.Web.UI.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ListHtmlBuilder<List, Item> : EasyUIHtmlBuilder<List> 
        where List : ListWidgetBase<Item> 
        where Item : WidgetBase
	{
		public ListHtmlBuilder(List component, string tagName)
			: base(component, tagName)
		{
		}

		public override IHtmlNode Build()
		{
			IHtmlNode html = base.Build();
			if (Component.Items.Any())
			{
				Component.Items.Each(item =>
				{
					item.HtmlBuilder.Build().AppendTo(html);
				});
			}
			return html;
		}
	}
}
