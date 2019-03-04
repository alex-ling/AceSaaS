using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Builder
{
	public class ItemsBuilder<Item, ItemBuilder> where Item : WidgetBase where ItemBuilder : WidgetBuilder<Item, ItemBuilder>
	{
		private readonly IList<Item> container;

		private Func<Item> creator;

		private Func<Item, ItemBuilder> builder;

		public ItemsBuilder(IList<Item> container, Func<Item> itemCreator, Func<Item, ItemBuilder> builderCreator)
		{
			this.container = container;
			creator = itemCreator;
			builder = builderCreator;
		}

		public virtual ItemBuilder Add()
		{
			Item val = creator();
			container.Add(val);
			return builder(val);
		}
	}
}
