namespace Acesoft.Web.UI.Widgets.Html
{
	public class ComboHtmlBuilder<Widget> : TextBoxHtmlBuilder<Widget> where Widget : Combo
	{
		public ComboHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.PanelWidth.HasValue)
			{
				base.Options["panelWidth"] = base.Component.PanelWidth;
			}
			if (base.Component.PanelHeight.HasValue)
			{
				base.Options["panelHeight"] = base.Component.PanelHeight;
			}
			if (base.Component.PanelMinWidth.HasValue)
			{
				base.Options["panelMinWidth"] = base.Component.PanelMinWidth;
			}
			if (base.Component.PanelMinHeight.HasValue)
			{
				base.Options["panelMinHeight"] = base.Component.PanelMinHeight;
			}
			if (base.Component.PanelMaxWidth.HasValue)
			{
				base.Options["panelMaxWidth"] = base.Component.PanelMaxWidth;
			}
			if (base.Component.PanelMaxHeight.HasValue)
			{
				base.Options["panelMaxHeight"] = base.Component.PanelMaxHeight;
			}
			if (base.Component.PanelAlign.HasValue)
			{
				base.Options["panelAlign"] = base.Component.PanelAlign;
			}
			if (base.Component.Multiple.HasValue)
			{
				base.Options["multiple"] = base.Component.Multiple;
			}
			if (base.Component.Multivalue.HasValue)
			{
				base.Options["multivalue"] = base.Component.Multiline;
			}
			if (base.Component.Reversed.HasValue)
			{
				base.Options["reversed"] = base.Component.Reversed;
			}
			if (base.Component.SelectOnNavigation.HasValue)
			{
				base.Options["selectOnNavigation"] = base.Component.SelectOnNavigation;
			}
			if (base.Component.Separator.HasValue())
			{
				base.Options["separator"] = base.Component.Separator;
			}
			if (base.Component.HasDownArrow.HasValue)
			{
				base.Options["hasDownArrow"] = base.Component.HasDownArrow;
			}
		}
	}
}
