namespace Acesoft.Web.UI.Widgets.Html
{
	public class LayoutItemHtmlBuilder : PanelHtmlBuilder<LayoutItem>
	{
		public LayoutItemHtmlBuilder(LayoutItem component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Region.HasValue)
			{
				base.Options["region"] = base.Component.Region;
			}
			if (base.Component.Split.HasValue)
			{
				base.Options["split"] = base.Component.Split;
			}
			if (base.Component.MinWidth.HasValue)
			{
				base.Options["minWidth"] = base.Component.MinWidth;
			}
			if (base.Component.MinHeight.HasValue)
			{
				base.Options["minHeight"] = base.Component.MinHeight;
			}
			if (base.Component.MaxWidth.HasValue)
			{
				base.Options["maxWidth"] = base.Component.MaxWidth;
			}
			if (base.Component.MaxHeight.HasValue)
			{
				base.Options["maxHeight"] = base.Component.MaxHeight;
			}
			if (base.Component.ExpandMode.HasValue)
			{
				base.Options["expandMode"] = base.Component.ExpandMode;
			}
			if (base.Component.CollapsedSize.HasValue)
			{
				base.Options["collapsedSize"] = base.Component.CollapsedSize;
			}
			if (base.Component.HideExpandTool.HasValue)
			{
				base.Options["hideExpandTool"] = base.Component.HideExpandTool;
			}
			if (base.Component.HideCollapsedContent.HasValue)
			{
				base.Options["hideCollapsedContent"] = base.Component.HideCollapsedContent;
			}
			if (base.Component.CollapsedContent.HasValue())
			{
				base.Options["collapsedContent"] = base.Component.CollapsedContent;
			}
		}
	}
}
