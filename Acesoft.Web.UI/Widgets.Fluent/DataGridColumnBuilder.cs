using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataGridColumnBuilder : WidgetBuilder<DataGridColumn, DataGridColumnBuilder>
	{
		public DataGridColumnBuilder(DataGridColumn component)
			: base(component)
		{
		}

		public new virtual DataGridColumnBuilder Title(string title)
		{
			base.Component.Title = title;
			return this;
		}

		public virtual DataGridColumnBuilder Field(string field)
		{
			base.Component.Field = field;
			return this;
		}

		public virtual DataGridColumnBuilder Width(int width)
		{
			base.Component.Width = width;
			return this;
		}

		public virtual DataGridColumnBuilder Rowspan(int rowspan)
		{
			base.Component.Rowspan = rowspan;
			return this;
		}

		public virtual DataGridColumnBuilder Colspan(int colspan)
		{
			base.Component.Colspan = colspan;
			return this;
		}

		public virtual DataGridColumnBuilder Align(Align align)
		{
			base.Component.Align = align;
			return this;
		}

		public virtual DataGridColumnBuilder Halign(Align align)
		{
			base.Component.Halign = align;
			return this;
		}

		public virtual DataGridColumnBuilder Sortable(bool sortable = true)
		{
			base.Component.Sortable = sortable;
			return this;
		}

		public virtual DataGridColumnBuilder Order(Order order)
		{
			base.Component.Order = order;
			return this;
		}

		public virtual DataGridColumnBuilder Resizable(bool resizable = true)
		{
			base.Component.Resizable = resizable;
			return this;
		}

		public virtual DataGridColumnBuilder Fixed(bool @fixed = true)
		{
			base.Component.Fixed = @fixed;
			return this;
		}

		public new virtual DataGridColumnBuilder Hidden(bool hidden = true)
		{
			base.Component.Hidden = hidden;
			return this;
		}

		public virtual DataGridColumnBuilder Checkbox(bool checkbox = true)
		{
			base.Component.Checkbox = checkbox;
			return this;
		}

		public virtual DataGridColumnBuilder Editor(string editor)
		{
			base.Component.Editor = editor;
			return this;
		}

		public virtual DataGridColumnBuilder Format(string format)
		{
			if (format == "bool")
			{
				base.Component.Align = Acesoft.Web.UI.Align.center;
			}
			else if (format == "button")
			{
				format = "button:#" + base.Component.Grid.Id;
				base.Component.Sortable = false;
			}
			base.Component.Format = format;
			return this;
		}

		public virtual DataGridColumnBuilder Merged(bool merged = true)
		{
			base.Component.Merged = merged;
			return this;
        }

        public virtual DataGridColumnBuilder Type(int type)
        {
            base.Component.Type = type;
            return this;
        }

        public DataGridColumnBuilder Events(Action<DataGridColumnEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DataGridColumnEventBuilder(base.Component.Events));
			return this;
		}
	}
}
