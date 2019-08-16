using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class TimeSpinner : Spinner
	{
		public static readonly ScriptEvent OnFormatter = new ScriptEvent("formatter", "date");

		public static readonly ScriptEvent OnParser = new ScriptEvent("parser", "s");

		public string Separator { get; set; }

		public bool? ShowSeconds { get; set; }

		public int? Highlight { get; set; }

		public IList<IList<int>> Selections { get; set; }

		public TimeSpinner(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "timespinner";
			Selections = new List<IList<int>>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TimeSpinnerHtmlBuilder<TimeSpinner>(this);
		}
	}
}
