using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MonthDataGridBuilder : DataGridBuilder<MonthDataGrid, MonthDataGridBuilder>
	{
		public MonthDataGridBuilder(MonthDataGrid component)
			: base(component)
		{
		}

        public virtual MonthDataGridBuilder ShowWeek(bool showWeek = true)
        {
            base.Component.ShowWeek = showWeek;
            return this;
        }

        public virtual MonthDataGridBuilder ColumnTitle(string columnTitle)
        {
            base.Component.ColumnTitle = columnTitle;
            return this;
        }

        public virtual MonthDataGridBuilder ColumnWidth(int width)
        {
            base.Component.ColumnWidth = width;
            return this;
        }

        public virtual MonthDataGridBuilder ColumnIndex(int index)
        {
            base.Component.ColumnIndex = index;
            return this;
        }

        public virtual MonthDataGridBuilder Month(int year, int month)
        {
            base.Component.Year = year;
            base.Component.Month = month;
            return this;
        }
    }
}
