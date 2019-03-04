namespace Acesoft.Web.UI.Widgets.Html
{
	public class AccordionItemHtmlBuilder : PanelHtmlBuilder<AccordionItem>
	{
		public AccordionItemHtmlBuilder(AccordionItem component)
			: base(component)
		{
			base.TagName = "div";
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Selected.HasValue)
			{
				base.Options["selected"] = base.Component.Selected;
			}
		}
	}
}
