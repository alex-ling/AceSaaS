namespace Acesoft.Web.UI.Widgets.Html
{
	public class TreeGridHtmlBuilder : DataGridHtmlBuilder<TreeGrid>
	{
		public TreeGridHtmlBuilder(TreeGrid component)
			: base(component, "table")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.TreeField.HasValue())
			{
				base.Options["treeField"] = base.Component.TreeField;
			}
			if (base.Component.Animate.HasValue)
			{
				base.Options["animate"] = base.Component.Animate;
			}
			if (base.Component.Checkbox.HasValue)
			{
				base.Options["checkbox"] = base.Component.Checkbox;
			}
			if (base.Component.CascadeCheck.HasValue)
			{
				base.Options["cascadeCheck"] = base.Component.CascadeCheck;
			}
			if (base.Component.OnlyLeafCheck.HasValue)
			{
				base.Options["onlyLeafCheck"] = base.Component.OnlyLeafCheck;
			}
			if (base.Component.Lines.HasValue)
			{
				base.Options["lines"] = base.Component.Lines;
			}
		}
	}
}
