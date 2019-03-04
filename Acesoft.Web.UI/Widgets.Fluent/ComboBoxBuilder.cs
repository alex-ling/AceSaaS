using Acesoft.Web.UI.Ajax;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboBoxBuilder : ComboBoxBuilder<ComboBox, ComboBoxBuilder>
	{
		public ComboBoxBuilder(ComboBox component)
			: base(component)
		{
		}

		public ComboBoxBuilder Events(Action<ComboBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ComboBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
	public class ComboBoxBuilder<Widget, Builder> : ComboBuilder<Widget, Builder> where Widget : ComboBox where Builder : ComboBoxBuilder<Widget, Builder>
	{
		public ComboBoxBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder ValueField(string valueField)
		{
			base.Component.ValueField = valueField;
			return this as Builder;
		}

		public virtual Builder TextField(string textField)
		{
			base.Component.TextField = textField;
			return this as Builder;
		}

		public virtual Builder GroupField(string groupField)
		{
			base.Component.GroupField = groupField;
			return this as Builder;
		}

		public virtual Builder Items(string items)
		{
			string[] array = items.Split(',');
			foreach (string text in array)
			{
				base.Component.Data.Add(new ComboItem(text, text));
			}
			return this as Builder;
		}

		public virtual Builder Items(Action<IList<ComboItem>> itemsAction)
		{
			itemsAction(base.Component.Data);
			return this as Builder;
		}

		public virtual Builder LimitToList(bool limitToList = true)
		{
			base.Component.LimitToList = limitToList;
			return this as Builder;
		}

		public virtual Builder ShowItemIcon(bool showItemIcon = true)
		{
			base.Component.ShowItemIcon = showItemIcon;
			return this as Builder;
		}

		public virtual Builder GroupPosition(GroupPos position)
		{
			base.Component.GroupPosition = position;
			return this as Builder;
		}

		public Builder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(Component.DataSource).Controller("crud").Action("list"));
			return this as Builder;
		}
	}
}
