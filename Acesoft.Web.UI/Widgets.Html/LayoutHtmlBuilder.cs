namespace Acesoft.Web.UI.Widgets.Html
{
	public class LayoutHtmlBuilder : ListHtmlBuilder<Layout, LayoutItem>
	{
		public LayoutHtmlBuilder(Layout component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Fit.HasValue)
			{
				base.Options["fit"] = base.Component.Fit;
			}
		}
	}
}
