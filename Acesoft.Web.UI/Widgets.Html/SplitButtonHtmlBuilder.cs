namespace Acesoft.Web.UI.Widgets.Html
{
	public class SplitButtonHtmlBuilder<Widget> : LinkButtonHtmlBuilder<Widget> where Widget : SplitButton
	{
		public SplitButtonHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Menu.HasValue())
			{
				base.Options["menu"] = base.Component.Menu;
			}
			if (base.Component.Duration.HasValue)
			{
				base.Options["duration"] = base.Component.Duration;
			}
		}
	}
}
