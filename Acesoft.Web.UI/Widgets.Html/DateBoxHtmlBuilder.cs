using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class DateBoxHtmlBuilder<Widget> : ComboHtmlBuilder<Widget> where Widget : DateBox
	{
		public DateBoxHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.CurrentText.HasValue())
			{
				base.Options["currentText"] = base.Component.CurrentText;
			}
			if (base.Component.CloseText.HasValue())
			{
				base.Options["closeText"] = base.Component.CloseText;
			}
			if (base.Component.OkText.HasValue())
			{
				base.Options["okText"] = base.Component.OkText;
			}
			if (Enumerable.Any<LinkButton>((IEnumerable<LinkButton>)base.Component.Buttons))
			{
				base.Options["buttons"] = base.Component.Buttons;
			}
			if (base.Component.SharedCalendar.HasValue())
			{
				base.Options["sharedCalendar"] = base.Component.SharedCalendar;
			}
		}
	}
}
