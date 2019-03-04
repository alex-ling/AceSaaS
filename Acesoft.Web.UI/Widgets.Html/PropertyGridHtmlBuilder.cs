namespace Acesoft.Web.UI.Widgets.Html
{
	public class PropertyGridHtmlBuilder : DataGridHtmlBuilder<PropertyGrid>
	{
		public PropertyGridHtmlBuilder(PropertyGrid component)
			: base(component, "table")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.ShowGroup.HasValue)
			{
				base.Options["showGroup"] = base.Component.ShowGroup;
			}
			if (base.Component.GroupField.HasValue())
			{
				base.Options["groupField"] = base.Component.GroupField;
			}
		}
	}
}
