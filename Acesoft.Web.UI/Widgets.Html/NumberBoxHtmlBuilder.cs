namespace Acesoft.Web.UI.Widgets.Html
{
	public class NumberBoxHtmlBuilder<Widget> : TextBoxHtmlBuilder<Widget> where Widget : NumberBox
	{
		public NumberBoxHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Min.HasValue)
			{
				base.Options["min"] = base.Component.Min;
			}
			if (base.Component.Max.HasValue)
			{
				base.Options["max"] = base.Component.Max;
			}
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
