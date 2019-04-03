using System;
using System.Collections;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Builder
{
	public class ListBuilder<Widget, Builder, Item, ItemBuilder> : WidgetBuilder<Widget, Builder> 
        where Widget : ListWidgetBase<Item>
        where Builder : ListBuilder<Widget, Builder, Item, ItemBuilder>
        where Item : WidgetBase
        where ItemBuilder: WidgetBuilder<Item, ItemBuilder>
	{
		public ListBuilder(Widget component) : base(component)
		{
		}

		protected virtual Builder Items(
            Action<ItemsBuilder<Item, ItemBuilder>> addAction, 
            Func<Item> itemCreator, 
            Func<Item, ItemBuilder> builderCreator)
		{
			addAction(new ItemsBuilder<Item, ItemBuilder>(Component.Items, itemCreator, builderCreator));
			return this as Builder;
        }

        protected virtual Builder ItemBind<Model>(
            Action<ItemBuilder, Model> bindItem,
            Func<Item> itemCreator,
            Func<Item, ItemBuilder> builderCreator)
        {
            Component.ItemBind = item =>
            {
                bindItem(new ItemsBuilder<Item, ItemBuilder>(Component.Items, itemCreator, builderCreator).Add(), (Model)item);
            };
            return this as Builder;
        }

        public virtual Builder Items(Func<IEnumerable> itemModelsFunc)
        {
            Component.Model = itemModelsFunc();
            return this as Builder;
        }

        public virtual Builder Items(IEnumerable itemModels)
        {
            Component.Model = itemModels;
            return this as Builder;
        }
    }
}
