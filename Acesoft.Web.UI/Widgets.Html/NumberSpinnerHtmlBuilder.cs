namespace Acesoft.Web.UI.Widgets.Html
{
	public class NumberSpinnerHtmlBuilder<Widget> : SpinnerHtmlBuilder<Widget> where Widget : NumberSpinner
	{
		public NumberSpinnerHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Precision.HasValue)
			{
				base.Options["precision"] = base.Component.Precision;
			}
			if (base.Component.DecimalSeparator.HasValue())
			{
				base.Options["decimalSeparator"] = base.Component.DecimalSeparator;
			}
			if (base.Component.GroupSeparator.HasValue())
			{
				base.Options["groupSeparator"] = base.Component.GroupSeparator;
			}
			if (base.Component.Prefix.HasValue())
			{
				base.Options["prefix"] = base.Component.Prefix;
			}
			if (base.Component.Suffix.HasValue())
			{
				base.Options["suffix"] = base.Component.Suffix;
			}
		}
	}
}
