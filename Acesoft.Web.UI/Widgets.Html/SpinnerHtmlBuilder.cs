namespace Acesoft.Web.UI.Widgets.Html
{
	public class SpinnerHtmlBuilder<Widget> : TextBoxHtmlBuilder<Widget> where Widget : Spinner
	{
		public SpinnerHtmlBuilder(Widget component)
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
			if (base.Component.Increment.HasValue)
			{
				base.Options["increment"] = base.Component.Increment;
			}
			if (base.Component.SpinAlign.HasValue)
			{
				base.Options["spinAlign"] = base.Component.SpinAlign;
			}
		}
	}
}
