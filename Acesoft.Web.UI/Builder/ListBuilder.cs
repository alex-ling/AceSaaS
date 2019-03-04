using System;

namespace Acesoft.Web.UI.Builder
{
	public class ListBuilder<Widget, Item, Builder> : WidgetBuilder<Widget, Builder> where Widget : ListWidgetBase<Item> where Item : WidgetBase where Builder : ListBuilder<Widget, Item, Builder>
	{
		public ListBuilder(Widget component)
			: base(component)
		{
		}

		protected virtual Builder Items<ItemBuilder>(Action<ItemsBuilder<Item, ItemBuilder>> addAction, Func<Item> itemCreator, Func<Item, ItemBuilder> builderCreator) where ItemBuilder : WidgetBuilder<Item, ItemBuilder>
		{
			addAction(new ItemsBuilder<Item, ItemBuilder>(((ListWidgetBase<Item>)base.Component).Items, itemCreator, builderCreator));
			return this as Builder;
		}
	}
}
