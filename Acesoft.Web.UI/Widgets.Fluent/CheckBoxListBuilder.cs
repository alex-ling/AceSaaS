using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class CheckBoxListBuilder : ListBuilder<CheckBoxList, CheckBoxListBuilder, CheckBox, CheckBoxBuilder>
	{
		public CheckBoxListBuilder(CheckBoxList component) : base(component)
		{
		}

		public virtual CheckBoxListBuilder ColumnSize(int columnSize)
		{
			base.Component.ColumnSize = columnSize;
			return this;
		}

		public virtual CheckBoxListBuilder Value(string value)
		{
			base.Component.Value = value;
			return this;
		}

		public CheckBoxListBuilder Items(Action<ItemsBuilder<CheckBox, CheckBoxBuilder>> addAction)
		{
			return Items(addAction, () => new CheckBox(base.Component.Ace), (CheckBox item) => new CheckBoxBuilder(item));
		}

        public CheckBoxListBuilder Ajax(Action<DataSourceBuilder> ajaxAction)
        {
            ajaxAction(new DataSourceBuilder(Component.DataSource).Controller("crud").Action("list"));
            return this;
        }

        public CheckBoxListBuilder Events(Action<EventBuilder> clientEventsAction)
		{
			clientEventsAction(new EventBuilder(base.Component.Events));
			return this;
		}
	}
}
