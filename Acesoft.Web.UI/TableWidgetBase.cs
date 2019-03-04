namespace Acesoft.Web.UI
{
	public abstract class TableWidgetBase<Item> : ListWidgetBase<Item> where Item : WidgetBase
	{
		public int ColumnSize
		{
			get;
			set;
		}

		public TableWidgetBase(WidgetFactory ace)
			: base(ace)
		{
		}
	}
}
