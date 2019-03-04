using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TimeSpinnerHtmlBuilder<Widget> : SpinnerHtmlBuilder<Widget> where Widget : TimeSpinner
	{
		public TimeSpinnerHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Separator.HasValue())
			{
				base.Options["separator"] = base.Component.Separator;
			}
			if (base.Component.ShowSeconds.HasValue)
			{
				base.Options["showSeconds"] = base.Component.ShowSeconds;
			}
			if (base.Component.Highlight.HasValue)
			{
				base.Options["highlight"] = base.Component.Highlight;
			}
			if (Enumerable.Any<IList<int>>((IEnumerable<IList<int>>)base.Component.Selections))
			{
				base.Options["selections"] = base.Component.Selections;
			}
		}
	}
}
