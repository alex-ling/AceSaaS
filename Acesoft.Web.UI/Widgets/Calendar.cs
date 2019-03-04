using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Calendar : WidgetBase
	{
		public static readonly ScriptEvent OnGetWeekNumber = new ScriptEvent("getWeekNumber", "date");

		public static readonly ScriptEvent OnFormatter = new ScriptEvent("formatter", "date");

		public static readonly ScriptEvent OnStyler = new ScriptEvent("styler", "date");

		public static readonly ScriptEvent OnValidator = new ScriptEvent("validator", "date");

		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "date");

		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "nDate,oDate");

		public int? Width
		{
			get;
			set;
		}

		public int? Height
		{
			get;
			set;
		}

		public bool? Fit
		{
			get;
			set;
		}

		public bool? Border
		{
			get;
			set;
		}

		public bool? ShowWeek
		{
			get;
			set;
		}

		public string WeekNumberHeader
		{
			get;
			set;
		}

		public int? FirstDay
		{
			get;
			set;
		}

		public IList<string> Weeks
		{
			get;
			set;
		}

		public IList<string> Months
		{
			get;
			set;
		}

		public int? Year
		{
			get;
			set;
		}

		public int? Month
		{
			get;
			set;
		}

		public DateTime? Current
		{
			get;
			set;
		}

		public Calendar(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "calendar";
			Weeks = new List<string>();
			Months = new List<string>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new CalendarHtmlBuilder(this);
		}
	}
}
