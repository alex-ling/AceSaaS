using Acesoft.Web.UI.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TableHtmlBuilder<Table, Item> : WidgetHtmlBuilder<Table> 
        where Table : TableWidgetBase<Item> 
        where Item : WidgetBase
	{
		public TableHtmlBuilder(Table component)
			: base(component, "table")
		{
		}

		public override IHtmlNode Build()
		{
			var html = base.Build();

			if (Component.Items.Any())
			{
				IHtmlNode tr = null;
				int curRow = 0;
				int count = base.Component.Items.Count;
				Component.Items.Each(item =>
				{
					if (++curRow % base.Component.ColumnSize == 0)
					{
						tr = new HtmlNode("tr").AppendTo(html);
					}

					var td = new HtmlNode("td").AppendTo(tr);
					item.HtmlBuilder.Build().AppendTo(td).NextSibings.Each(label =>
					{
						label.AppendTo(td);
					});
				});
			}

			return html;
		}
	}
}
