using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public abstract class ListWidgetBase<Item> : WidgetBase, IListContainer<Item> where Item : WidgetBase
	{
		public IList<Item> Items
		{
			get;
			private set;
		}

		public ListWidgetBase(WidgetFactory ace)
			: base(ace)
		{
			Items = new List<Item>();
		}
	}
}
