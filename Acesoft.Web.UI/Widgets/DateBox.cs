using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class DateBox : Combo
	{
		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "date");

		public string CurrentText { get; set; }

		public string CloseText { get; set; }

		public string OkText { get; set; }

		public IList<LinkButton> Buttons { get; set; }

		public string SharedCalendar { get; set; }

		public DateBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "datebox";
			Buttons = new List<LinkButton>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new DateBoxHtmlBuilder<DateBox>(this);
		}
	}
}
