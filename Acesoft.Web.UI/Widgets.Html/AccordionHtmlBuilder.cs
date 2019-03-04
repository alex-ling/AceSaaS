namespace Acesoft.Web.UI.Widgets.Html
{
	public class AccordionHtmlBuilder : ListHtmlBuilder<Accordion, AccordionItem>
	{
		public AccordionHtmlBuilder(Accordion component)
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
			if (base.Component.Border.HasValue)
			{
				base.Options["border"] = base.Component.Border;
			}
			if (base.Component.Animate.HasValue)
			{
				base.Options["animate"] = base.Component.Animate;
			}
			if (base.Component.Multiple.HasValue)
			{
				base.Options["multiple"] = base.Component.Multiple;
			}
			if (base.Component.Selected.HasValue)
			{
				base.Options["selected"] = base.Component.Selected;
			}
			if (base.Component.HAlign.HasValue)
			{
				base.Options["halign"] = base.Component.HAlign;
			}
		}
	}
}
