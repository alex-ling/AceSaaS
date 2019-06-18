using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using Acesoft.Data;
using Acesoft.Util;

namespace Acesoft.Web.UI.Widgets
{
	public class MonthDataGrid : DataGrid
	{
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool? ShowWeek { get; set; }
        public string ColumnTitle { get; set; }
        public int? ColumnWidth { get; set; }
        public int? ColumnIndex { get; set; }

        public MonthDataGrid(WidgetFactory ace)	: base(ace)
		{			
		}

        public override void OnCreateControl()
        {
            var year = Year ?? App.GetQuery("year", DateTime.Now.Year);
            var month = Month ?? App.GetQuery("month", DateTime.Now.Month);

            // 计算rowspan,colspan
            var rowspan = ShowWeek == true ? 3 : 2;
            var colspan = DatetimeHelper.GetMonthDays(year, month);
            var dayTypes = GetDayTypes(year, month);

            if (Columns.Count <= 1)
            {
                var row = Columns.FirstOrDefault();
                if (row == null)
                {
                    row = new List<DataGridColumn>();
                    Columns.Add(row);
                }
                else
                {
                    // 自动处理rowspan
                    row.Each(c => c.Rowspan = rowspan);
                }

                // month
                var col = new DataGridColumn(Ace);
                col.Colspan = colspan;
                col.Title = (ColumnTitle ?? "{0}年{1:D2}月").FormatWith(year, month);
                if (ColumnIndex.HasValue)
                {
                    row.Insert(ColumnIndex.Value, col);
                }
                else
                {
                    row.Add(col);
                }

                // week
                if (rowspan >= 3)
                {
                    row = new List<DataGridColumn>();
                    Columns.Add(row);
                    for (var i = 1; i <= colspan; i++)
                    {
                        col = new DataGridColumn(Ace);
                        col.Title = new DateTime(year, month, i).GetChinaWeekCN();
                        row.Add(col);
                    }
                }

                // day
                row = new List<DataGridColumn>();
                Columns.Add(row);
                for (var i = 1; i <= colspan; i++)
                {
                    col = new DataGridColumn(Ace);
                    col.Title = i.ToString("D2"); //01, 02
                    col.Align = Align.center;
                    col.Width = ColumnWidth;
                    col.Sortable = false;
                    col.Field = $"d{i}";
                    if (dayTypes != null && dayTypes.ContainsKey(i))
                    {
                        col.Type = dayTypes[i];
                    }
                    row.Add(col);
                }
            }
        }

        private IDictionary<int, int> GetDayTypes(int year, int month)
        {
            var sqlFullId = (string)DataSource.RouteValues["ds"];
            var sqlMap = Ace.Session.GetSqlMap(sqlFullId);
            var columnSql = sqlMap.Params.GetValue("columnsql", "");

            if (columnSql.HasValue())
            {
                return Ace.Session.Query<DayType>(columnSql, new
                {
                    year,
                    month
                }).ToDictionary(c => c.Day, c => c.Type);
            }
            return null;
        }

        public class DayType
        {
            public int Day { get; set; }
            public int Type { get; set; }
        }
    }
}
