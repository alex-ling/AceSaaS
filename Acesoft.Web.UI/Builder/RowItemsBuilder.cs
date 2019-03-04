using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Builder
{
	public class RowItemsBuilder<Item, ItemBuilder> where Item : WidgetBase where ItemBuilder : WidgetBuilder<Item, ItemBuilder>
	{
		private readonly IList<IList<Item>> container;

		private Func<Item> creator;

		private Func<Item, ItemBuilder> builder;

		private IList<Item> row;

		public RowItemsBuilder(IList<IList<Item>> container, Func<Item> itemCreator, Func<Item, ItemBuilder> builderCreator)
		{
			this.container = container;
			creator = itemCreator;
			builder = builderCreator;
		}

		public virtual void AddRow()
		{
			row = new List<Item>();
			container.Add(row);
		}

		public virtual ItemBuilder Add()
		{
			if (row == null)
			{
				AddRow();
			}
			Item val = creator();
			row.Add(val);
			return builder(val);
		}
	}
}
