using System;
using System.Collections;
using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public abstract class ListWidgetBase<Item> : WidgetBase, IDataBind<IEnumerable>, IListContainer<Item> where Item : WidgetBase
	{
		public IList<Item> Items { get; }
        public IEnumerable Model { get; set; }
        public Action<object> ItemBind { get; set; }

        public virtual void DataBind()
        {
            if (Model != null && ItemBind != null)
            {
                foreach (var item in Model)
                {
                    ItemBind(item);
                }
            }
        }

        public ListWidgetBase(WidgetFactory ace) : base(ace)
		{
			Items = new List<Item>();
		}
    }
}
